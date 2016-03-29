using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Devices.I2c;
using Newtonsoft.Json;
using SparkFun;

namespace IoTBackUp
{
    class DesktopWeather :  ISparkFunWeatherSheild
    {
        private float _humidity;
        private float _temperature;

        public Task<I2cDevice> InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public float Humidity
        {
            get { return _humidity; }
        }

        public float Temperature
        {
            get { return _temperature; }
        }

        public async Task<bool> Setup()
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=Sydney&units=metric&APPID=4b81d2e206e639bf8c46ccc86ed04d3f");
                var jsonWeatherObj = JsonConvert.DeserializeObject<OpenWeatherMapData>(await response.Content.ReadAsStringAsync());

                _humidity = jsonWeatherObj.main.humidity;
                _temperature = jsonWeatherObj.main.temp;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
