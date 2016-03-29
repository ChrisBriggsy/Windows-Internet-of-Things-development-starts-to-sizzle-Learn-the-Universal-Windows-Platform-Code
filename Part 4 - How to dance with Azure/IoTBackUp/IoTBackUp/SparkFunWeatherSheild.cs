using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace SparkFun 
{
    class SparkFunWeatherSheild
    {
        private const ushort HTDU21D_I2C_ADDRESS = 0x0040;
        private const byte COMMAND_TEMPERATURE_HOLD = 0xE3;
        private const byte COMMAND_HUMIDITY_HOLD = 0xE5;

        private I2cDevice htdu21d;

        public async Task<Boolean> Setup()
        {
            htdu21d = await InitializeAsync();

            if(htdu21d != null)
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<I2cDevice> InitializeAsync()
        {
            string advanced_query_syntax = I2cDevice.GetDeviceSelector("I2C1");
            DeviceInformationCollection device_information_collection = await DeviceInformation.FindAllAsync(advanced_query_syntax);
            string deviceId = device_information_collection[0].Id;

            I2cConnectionSettings htdu21d_connection = new I2cConnectionSettings(HTDU21D_I2C_ADDRESS);
            htdu21d_connection.BusSpeed = I2cBusSpeed.FastMode;
            htdu21d_connection.SharingMode = I2cSharingMode.Shared;

            return await I2cDevice.FromIdAsync(deviceId, htdu21d_connection);
        }


        public float Humidity
        {
            get
            {
                ushort raw_humidity_data = 0;
                byte[] i2c_humidity_data = new byte[3];

                htdu21d.WriteRead(new byte[] { COMMAND_HUMIDITY_HOLD }, i2c_humidity_data);

                raw_humidity_data = (ushort)(i2c_humidity_data[0] << 8);
                raw_humidity_data |= (ushort)(i2c_humidity_data[1] & 0xFC);

                bool humidity_data = (0x00 != (0x02 & i2c_humidity_data[1]));
                if (!humidity_data) { return 0; }

                bool valid_data = ValidHtdu21dCyclicRedundancyCheck(raw_humidity_data, (byte)(i2c_humidity_data[2] ^ 0x62));
                if (!valid_data) { return 0; }

                double humidity_RH = (((125.0 * raw_humidity_data) / 65536) - 6.0);

                return Convert.ToSingle(humidity_RH);
            }
        }

        public float Temperature
        {
            get
            {
                ushort raw_temperature_data = 0;
                byte[] i2c_temperature_data = new byte[3];

                htdu21d.WriteRead(new byte[] { COMMAND_TEMPERATURE_HOLD }, i2c_temperature_data);

                raw_temperature_data = (ushort)(i2c_temperature_data[0] << 8);
                raw_temperature_data |= (ushort)(i2c_temperature_data[1] & 0xFC);

                bool temperature_data = (0x00 == (0x02 & i2c_temperature_data[1]));
                if (!temperature_data) { return 0; }

                bool valid_data = ValidHtdu21dCyclicRedundancyCheck(raw_temperature_data, i2c_temperature_data[2]);
                if (!valid_data) { return 0; }

                double temperature_C = (((175.72 * raw_temperature_data) / 65536) - 46.85);

                return Convert.ToSingle(temperature_C);
            }
        }

        private bool ValidHtdu21dCyclicRedundancyCheck(
            ushort data_,
            byte crc_
        )
        {

            const int CRC_BIT_LENGTH = 8;
            const int DATA_LENGTH = 16;
            const ushort GENERATOR_POLYNOMIAL = 0x0131;

            int crc_data = data_ << CRC_BIT_LENGTH;

            for (int i = (DATA_LENGTH - 1); 0 <= i; --i)
            {
                if (0 == (0x01 & (crc_data >> (CRC_BIT_LENGTH + i)))) { continue; }
                crc_data ^= (GENERATOR_POLYNOMIAL << i);
            }

            return (crc_ == crc_data);
        }
    }
}
