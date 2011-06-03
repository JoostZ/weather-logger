using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherLoggerLib
{
    public class AllSnapshotsReadArg : EventArgs
    {
        private readonly List<WeatherSnapshot> data;
        public List<WeatherSnapshot> Data
        {
            get
            {
                return data;
            }
        }

        public AllSnapshotsReadArg(List<WeatherSnapshot> data)
        {
            this.data = data;
        }
    }

    /// <summary>
    /// Read wheather snapshots from the weather station
    /// </summary>
    public class SnapshotReader
    {
        public event EventHandler<BufferReceivedEventArgs> BufferReceived; 
        private WS4000Device device;

        List<WeatherSnapshot> newSnapshots = new List<WeatherSnapshot>();

        public SnapshotReader(WS4000Device device)
        {
            this.device = device;
            this.AllSnapshotsRead += Persist;
            Context = new DatabaseEntities1();
        }

        private int  Count { get; set; } 

        public BufferPos CurrentPos { 
            get; 
            private set; 
        }

        public DateTime SnapshotDate { 
            get; 
            private set; 
        }

        public DateTime LimitDate { get; private set; }

        private DatabaseEntities1 Context { get; set; }

        public void readSnapshots()
        {
            DateTime last;
            //using (var context = new DatabaseEntities1())
            {
                int count = Context.WeatherSnapshots.Count();
                if (count == 0) {

                    last = new DateTime(2000, 1, 1, 0, 0, 0);
                } else {
                last = Context.WeatherSnapshots.Max(tr => tr.Timestamp);
                }
            }
            readSnapshots(last);
        }

        /// <summary>
        /// Read snapshots after a certain date-time
        /// </summary>
        /// <param name="afterDate">The date/time after which the snapshots should be read</param>
        public void readSnapshots(DateTime afterDate)
        {
            LimitDate = afterDate;
   
            // Start reading the global data
            device.BufferReceived += HandleBufferReceived;
            this.BufferReceived += GlobalBufferRead;
            device.getMemory(0, 64);

        }

        const int DATA_COUNT = 27;
        const int CURRENT_POS = 30;
        const int DATE_TIME = 43;

        private static ushort ToUShort(byte[] globalBuffer, int offset)
        {
            int lo = globalBuffer[offset];
            int hi = globalBuffer[offset + 1] << 8;
            ushort result = (ushort)(lo + hi);
            return result;
        }

        private static int bcdDecode(byte value) {
            int hi = (value >> 4) & 0x0f;
            int lo = value & 0x0f;
            return (hi * 10 + lo);
        }

        static DateTime toDateTime(byte[] buffer, int offset)
        {
            int year = bcdDecode(buffer[offset]) + 2000;
            int month = bcdDecode(buffer[offset + 1]);
            int day = bcdDecode(buffer[offset + 2]);
            int hour = bcdDecode(buffer[offset + 3]);
            int minute = bcdDecode(buffer[offset + 4]);

            return new DateTime(year, month, day, hour, minute, 0);
        }

        private WS4000Buffer CurrentBuffer { get; set; }

        private void GetBlock(int offset)
        {
            if (CurrentBuffer != null && CurrentBuffer.Contains(offset))
            {
                // We have the data in cache. We return it
                OnBufferReceived(CurrentBuffer);
            }
            else
            {
                // Read the data
                device.getMemory(offset);
            }
        }

        private void GetBlock(BufferPos currentPos)
        {
            GetBlock(currentPos.Offset);
        }

        void HandleBufferReceived(object sender, BufferReceivedEventArgs args)
        {
            CurrentBuffer = WS4000Buffer.Clone(args.Data);
            OnBufferReceived(CurrentBuffer);
        }

        private void OnBufferReceived(WS4000Buffer theBuffer) {
            if (BufferReceived != null) {
                BufferReceived(this, new BufferReceivedEventArgs(theBuffer));
            }
        }

        /// <summary>
        /// Handle the received global data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void GlobalBufferRead(object sender, BufferReceivedEventArgs args)
        {
            this.BufferReceived -= GlobalBufferRead;
            byte[] globalBuffer = args.Data.Buffer;
            Count = ToUShort(globalBuffer, DATA_COUNT) - 1; // The active buffer is also in Count
            SnapshotDate = toDateTime(globalBuffer, DATE_TIME);
            ushort pos = ToUShort(globalBuffer, CURRENT_POS);
            CurrentPos = new BufferPos(pos);
            CurrentBuffer = null;

            this.BufferReceived += ActiveSnapshotRead;
            GetBlock(CurrentPos);
        }

        void ActiveSnapshotRead(object sender, BufferReceivedEventArgs args)
        {
            this.BufferReceived -= ActiveSnapshotRead;

            byte[] buffer = args.Data.Buffer;
            int offset = 0;
            if (args.Data.Offset != CurrentPos.Position) {
                offset = 0x10;
            }

            int delay = buffer[offset];
            SnapshotDate -= new TimeSpan(0, delay, 0);
            if (SnapshotDate < LimitDate)
            {
                // TODO: Handle wrong LimitDate
            }
            else
            {
                CurrentPos--;
                this.BufferReceived += BufferedSnapshotRead;
                GetBlock(CurrentPos);
            }
        }


        void BufferedSnapshotRead(object sender, BufferReceivedEventArgs args)
        {
            byte[] buffer = args.Data.Buffer;
            int offset = 0;
            if (args.Data.Offset != CurrentPos.Position)
            {
                offset = 0x10;
            }
            Count--;
            WeatherSnapshot snapshot = new WeatherSnapshot(buffer, offset);
            SnapshotDate -= new TimeSpan(0, snapshot.Interval, 0);
            snapshot.Timestamp = SnapshotDate;


            if (Count <= 0 || snapshot.Timestamp <= LimitDate)
            {
                // Finished reading
                this.BufferReceived -= BufferedSnapshotRead;
                device.BufferReceived -= HandleBufferReceived;

                OnAllSnapshotsRead(newSnapshots);
                newSnapshots.Clear();
            }
            else
            {
                newSnapshots.Add(snapshot);

                GetBlock(--CurrentPos);
            }
        }

        public EventHandler<AllSnapshotsReadArg> AllSnapshotsRead;

        private void OnAllSnapshotsRead(List<WeatherSnapshot> data)
        {
            if (AllSnapshotsRead != null)
            {
                AllSnapshotsRead(this, new AllSnapshotsReadArg(data));
            }
        }

        private void Persist(object sender, AllSnapshotsReadArg args)
        {
            //using (var context = new DatabaseEntities1())
            {
                try
                {
                    foreach (WeatherSnapshot snapshot in args.Data)
                    {
                        Context.AddToWeatherSnapshots(snapshot);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                finally
                {
                    try
                    {
                        Context.SaveChanges();
                    }
                    catch ( Exception e)
                    {
                        
                        throw;
                    }
                }
            }
        }
    }

}
