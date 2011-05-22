using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using UsbLibrary;

namespace WeatherLoggerLib
{
    public partial class WS4000HidPort : UsbHidPort
    {
        public override int ProductId
        {
            get { return 0x1401; }
        }

        public override int VendorId
        {
            get
            {
                return 0x8201;
            }
        }
        public override Type DeviceType
        {
            get
            {
                return typeof(WS4000Device);
            }
        }
        public WS4000HidPort()
        {
            InitializeComponent();
        }

        protected override HIDDevice CreateDevice()
        {
            return new WS4000Device();
        }
    }
}
