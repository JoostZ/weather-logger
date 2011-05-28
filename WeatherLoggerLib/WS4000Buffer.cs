using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherLoggerLib
{
    /// <summary>
    /// Value of part of the memory in the WS4000 device
    /// </summary>
    public class WS4000Buffer
    {
        /// <summary>
        /// Address from which the memory is read
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Number of bytes in the buffer
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The actual buffer
        /// </summary>
        public byte[] Buffer { get; set; }

        public int Cursor {
            get; 
            private set; 
        }

        public bool Full
        {
            get
            {
                return Cursor >= Size;
            }
        }

        public WS4000Buffer(int offset, int size)
        {
            Offset = offset;
            Size = size;
            Buffer = new byte[size];
        }

        public void add(byte[] buffer)
        {
            for (int i = 1; i < 9 && !Full; i++)
            {
                Buffer[Cursor++] = buffer[i];
            }
        }
    }
}
