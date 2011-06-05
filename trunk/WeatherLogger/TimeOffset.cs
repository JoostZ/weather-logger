using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherLogger
{
    public class TimeOffset
    {
        public TimeOffset()
        {
            Interval = new TimeSpan();
            Name = "";
        }

        public TimeOffset(TimeSpan interval, String name)
        {
            Interval = interval;
            Name = name;
        }
        public TimeSpan Interval { get; set; }
        public String Name { get; set; }
        public override String ToString()
        {
            return Name;
        }
    }

    public class TimeOffsets
    {
        public TimeOffsets()
        {
        }

        public TimeOffset[] list = new TimeOffset[] {
                                new TimeOffset(new TimeSpan(1, 0, 0, 0), "Day"),
                                new TimeOffset(new TimeSpan(7, 0, 0, 0), "Week"),
                                new TimeOffset(new TimeSpan(30, 0, 0, 0), "Month"),
                                new TimeOffset(new TimeSpan(365, 0, 0, 0), "Year"),
                            };


       
    }
}
