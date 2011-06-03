using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherLoggerLib
{
    public partial class WeatherSnapshot
    {
        // Offsets in the buffer
        enum Offsets
        {
            INTERVAL = 0,
            INTERNAL_HUMIDTY = 1,
            INTERNAL_TEMPERATURE = 2,
            OUTDOOR_HUMIDITY = 4,
            OUTDOOR_TEMPERATURE = 5,
            PRESSURE = 7,
            WIND_AVERAGE = 9,
            WIND_GUST = 10,
            WIND_DIR = 12,
            RAIN = 13,
            STATUS = 15
        };

        private byte ToByte(byte[] buffer, int shift, Offsets offset)
        {
            return buffer[(int)offset + shift];
        }

        private float ToFloat(byte[] buffer, int shift, Offsets offset)
        {
            int lo = buffer[(int)offset + shift];
            int hi = buffer[(int)offset + 1 + shift];
            int result = (hi << 8) + lo;
            return (float) result;
        }

        private int FromSplit(byte[] buffer, int shift, Offsets offset, int nibbleOffset)
        {
            int result;
            int low = buffer[(int)offset + shift];
                int hi = buffer[(int)offset + nibbleOffset + shift];
                if (nibbleOffset == 2)
                {
                    result = ((hi & 0x0F) << 8) + low;
                }
                else
                {
                    result = ((hi & 0xF0) << 4) + low;
                }
                return result;
        }

        public WeatherSnapshot(byte[] buffer, int offset)
        {
            Status = ToByte(buffer, offset, Offsets.STATUS);
            Interval = ToByte(buffer, offset, Offsets. INTERVAL);
            IndoorHumidity = ToByte(buffer, offset, Offsets.INTERNAL_HUMIDTY);
            float value = ToFloat(buffer, offset, Offsets.INTERNAL_TEMPERATURE);
            IndoorTemperature = 0.1 * value;
            Pressure = 0.1 * ToFloat(buffer, offset, Offsets.PRESSURE);

            if ((Status & 0x40) != 0)
            {
                // No connection available Set all outdoor values to 0
                OutdoorHumidity = 0;
                OutdoorTemperature = 0.0;
                WindAverage = 0.0;
                WindGust = 0.0;
                WindDirectionId = 0;
                Rain = 0.0;
            }
            else
            {
                OutdoorHumidity = ToByte(buffer, offset, Offsets.OUTDOOR_HUMIDITY);
                OutdoorTemperature = 0.1 * ToFloat(buffer, offset, Offsets.OUTDOOR_TEMPERATURE);
                WindAverage = 0.1 * FromSplit(buffer, offset, Offsets.WIND_AVERAGE, 2);
                WindGust = 0.1 * FromSplit(buffer, offset, Offsets.WIND_GUST, 1);
                WindDirectionId = ToByte(buffer, offset, Offsets.WIND_DIR);
                if (WindDirectionId < 0 || WindDirectionId > 15)
                {
                    WindDirectionId = 0;
                }
                Rain = 0.3 * ToFloat(buffer, offset, Offsets.RAIN);
            }
        }

        public WeatherSnapshot()
        { }
    }
}

