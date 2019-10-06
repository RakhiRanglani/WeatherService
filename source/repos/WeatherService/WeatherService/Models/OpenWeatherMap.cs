using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherService.Models
{
    public class OpenWeatherMap
    {
        public string apiResponse { get; set; }

    }
    public class CityList
    {
        public int cityId { get; set; }
        public string cityName { get; set; }
    }
}

