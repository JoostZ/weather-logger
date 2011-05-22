using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UsbLibrary;

namespace WeatherLoggerLib
{
    public class WS4000InputReport : InputReport
    {
        public WS4000InputReport(WS4000Device device)
            : base(device)
        {
        }


        public override void ProcessData()
        {
            throw new NotImplementedException();
        }
    }
}
