using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherLoggerLib
{
    public partial class WeatherSnapshot
    {
        // Offsets in the buffer
        const int INTERVAL = 0;
        const int INTERNAL_HUMIDTY = 1;

        //public DateTime Date { 
        //    get; 
        //    internal set;
        //}

        //public int InternalHumidity { 
        //    get; 
        //    private set; 
        //}

        //public int Interval { 
        //    get; 
        //    private set; 
        //}

        public WeatherSnapshot(byte[] buffer, int offset)
        {
            Interval = buffer[INTERVAL + offset];
            IndoorHumidity = buffer[INTERNAL_HUMIDTY + offset];
        }

        public WeatherSnapshot()
        { }
    }
}
