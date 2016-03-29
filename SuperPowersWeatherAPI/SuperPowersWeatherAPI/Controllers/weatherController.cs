using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SuperPowersWeatherAPI.Controllers
{
    public class WeatherController : ApiController
    {
        private static List<WeatherData> _weatherData = new List<WeatherData>();


        public List<WeatherData> Get()
        {
            return _weatherData;
        }

        public void Post([FromBody]WeatherData value)
        {
            _weatherData.RemoveAll(x => x.City == value.City);
            _weatherData.Add(value);
        }

        public void Delete()
        {
            _weatherData = new List<WeatherData>();
        }
    }

    public class WeatherData
    {
        //public Guid DeviceID { get; set; }
        //public string DeviceName { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float Humidity { get; set; }
        public float Temperature { get; set; }
        public string City { get; set; }
    }
}
