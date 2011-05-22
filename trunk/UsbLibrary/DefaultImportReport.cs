using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsbLibrary
{
    class DefaultImportReport : InputReport
    {

        DefaultImportReport(HIDDevice device) : base(device)
        {
        }

        public override void ProcessData()
        {
            // Nothing to do. We will use the Buffer
            // property to get at the received data
        }
    }
}
