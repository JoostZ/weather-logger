using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UsbLibrary;

namespace WeatherLoggerLib
{
    /**
     * @brief
     * USB Output Report to request a buffer from the memory of the station
     */
    class RequestBuffer : OutputReport
    {
        public int Offset { get; set; }

        private byte[] template = {
                              0xa1,
                              0x00,
                              0x00,
                              0x20,
                              0xa1,
                              0x00,
                              0x00,
                              0x20
                          };
        private const int lowPosition = 2;
        private const int highPosition = 3;
        private const int stride = 4;

        public RequestBuffer(WS4000Device device)
            : base(device)
        {
        }

        /**
         * @brief
         * Setup the data buffer for this request
         * 
         * @param offset
         * The offset within the memory to get 32 bytes from
         */
        protected override void SetData()
        {
            byte offsetHigh = (byte)(Offset >> 8 & 0xff);
            byte offsetLow = (byte)(Offset & 0xff);

            byte[] arrBuff = Buffer; //new byte[Buffer.Length];
            for (int i = 1; i < Buffer.Length; i++)
            {
                Buffer[i] = template[i];
            }
            Buffer[lowPosition] = Buffer[lowPosition + stride] = offsetLow;
            Buffer[highPosition] = Buffer[highPosition + stride] = offsetHigh;
        }
    }
}
