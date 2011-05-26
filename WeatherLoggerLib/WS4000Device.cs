using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UsbLibrary;

namespace WeatherLoggerLib
{
    public class WS4000Device : HIDDevice
    {

        public override InputReport CreateInputReport()
        {
            return new WS4000InputReport(this);
        }
    }
}
