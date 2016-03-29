using System;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace SparkFun
{
    internal interface ISparkFunWeatherSheild
    {
        Task<Boolean> Setup();
        Task<I2cDevice> InitializeAsync();
        float Humidity { get; }
        float Temperature { get; }
    }
}