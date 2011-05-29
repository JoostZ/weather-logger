using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherLoggerLib
{
    /// <summary>
    /// Position in the Circular Buffer of the Weather Station
    /// </summary>
    public class BufferPos
    {
        const int BUFFER_START = 0x1000;
        const int BUFFER_END = 0x10000;

        public int Position { 
            get; 
            private set;
        }


        public int Offset
        {
            get { 
                return (Position / 0x20) * 0x20;
            }
        }
        

        public BufferPos(ushort position)
        {
            Position = position;
        }

        public void Decrement(int size)
        {
            Position -= size;
            if (Position < BUFFER_START)
            {
                Position = BUFFER_END - 0x10;
            }
        }

        static public BufferPos operator --(BufferPos pos)
        {
            pos.Decrement(0x10);
            return pos;
        }
    }
}
