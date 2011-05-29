using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UsbLibrary;

namespace WeatherLoggerLib
{
    public class BufferReceivedEventArgs : EventArgs
    {
        private readonly WS4000Buffer data;
        public WS4000Buffer Data
        {
            get
            {
                return data;
            }
        }

        public BufferReceivedEventArgs(WS4000Buffer data)
        {
            this.data = data;
        }
    }

    public class WS4000Device : HIDDevice
    {

        public delegate void BufferReceivedEventHandler(object sender, BufferReceivedEventArgs args);

        public event EventHandler<BufferReceivedEventArgs> BufferReceived;

        private void OnBufferReceived(WS4000Buffer theBuffer)
        {
            if (BufferReceived != null)
            {
                BufferReceived(this, new BufferReceivedEventArgs(theBuffer));
            }
        }

        public WS4000Device()
        {
            DataReceived += new DataReceivedEventHandler(handleInputReport);
        }
        private WS4000Buffer Buffer { get; set; }

        public override InputReport CreateInputReport()
        {
            return new WS4000InputReport(this);
        }

        /// <summary>
        /// Start the reading of a 32 byte part of the WS4000 memory starting
        /// at address offset
        /// </summary>
        /// <param name="offset"></param>
        public void getMemory(int offset, int length = 0x20)
        {
            if (Buffer != null)
            {
                throw new HIDDeviceException("Reading memory is in progress");
            }

            Buffer = new WS4000Buffer(offset, length);
            RequestBuffer request = new RequestBuffer(this);
            request.Offset = offset;
            request.Send();
        }

        void handleInputReport(object sender, DataReceivedEventArgs args)
        {
            if (Buffer != null)
            {
                Buffer.add(args.data.Buffer);
                if (Buffer.Full)
                {
                    WS4000Buffer buffer = Buffer;
                    Buffer = null;
                    OnBufferReceived(buffer);
                }
                else if (Buffer.Cursor % 0x20 == 0)
                {
                    RequestBuffer request = new RequestBuffer(this);
                    request.Offset = Buffer.Cursor;
                    request.Send();
                }
            }
        }
    }
}
