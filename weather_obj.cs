using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Integration
{
    public class Weather
    {

        private static readonly HttpClient client = new HttpClient();
        private static class Constants
        {
            public static string unknown = "unknown";

            public static string coordinates = "coord";
            public static string coordLon = "lon";
            public static string coordLat = "lat";

            public static string weather = "weather";
            public static string weatherCurrentStatus = "main";
            public static string weatherDescription = "description";

            public static string main = "main";
            public static string mainAvgTemp = "temp";
            public static string mainTempMin = "temp_min";
            public static string mainTempMax = "temp_max";
            public static string mainPressure = "pressure";
            public static string mainHumidity = "humidity";

            public static string wind = "wind";
            public static string windSpeed = "speed";
            public static string windDeg = "deg";

            public static string sys = "sys";
            public static string sysCountry = "country";
            public static string sysSunRise = "sunrise";
            public static string sysSunSet = "sunset";

            public static string name = "name";

        }

        public class CoordinateDetails
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public CoordinateDetails()
            {
                Latitude = 0;
                Longitude = 0;
            }
            public CoordinateDetails(double latitude, double longitude)
            {
                Latitude = latitude;
                Longitude = longitude;
            }

        }

        public class WeatherDetails
        {
            public string CurrentWeather { get; set; }
            public string Desc { get; set; }

            public WeatherDetails()
            {
                CurrentWeather = Constants.unknown;
                Desc = Constants.unknown;
            }

            public WeatherDetails(string currentWeather, string desc)
            {
                CurrentWeather = currentWeather;
                Desc = desc;
            }
        }

        public class MainParams
        {
            public double Temp { get; set; }
            public double MinTemp { get; set; }
            public double MaxTemp { get; set; }
            public int Pressure { get; set; }
            public int Humidity { get; set; }

            public MainParams()
            {
                Temp = 0;
                MinTemp = 0;
                MaxTemp = 0;
                Pressure = 0;
                Humidity = 0;
            }
            public MainParams(double temp, double minTemp, double maxTemp, int pressure, int humidity)
            {
                Temp = temp;
                MinTemp = minTemp;
                MaxTemp = maxTemp;
                Pressure = pressure;
                Humidity = humidity;
            }
        }

        public class Wind
        {
            public double Speed { get; set; }

            public double Deg { get; set; }

            public Wind()
            {
                Speed = 0;
                Deg = 0;
            }

            public Wind(double speed, double deg)
            {
                Speed = speed;
                Deg = deg;
            }

        }

        public class SysDetails
        {
            public string Country { get; set; }
            public TimeSpan SunRise { get; set; }
            public TimeSpan SunSet { get; set; }
            public SysDetails()
            {
                Country = Constants.unknown;
                SunRise = TimeSpan.MinValue;
                SunSet = TimeSpan.MaxValue;
            }

            public SysDetails(string country, TimeSpan sunRise, TimeSpan sunSet)
            {
                Country = country;
                SunRise = sunRise;
                SunSet = sunSet;
            }
        }

        public string Name { get; set; }
        public CoordinateDetails Coordinates { get; set; }

        public WeatherDetails WeatherDetailsVar { get; set; }

        public MainParams MainParamsDetails { get; set; }

        public Wind WindDetails { get; set; }

        public SysDetails Sys { get; set; }

        public Weather()
        {
            Name = Constants.unknown;
            Coordinates = new CoordinateDetails();
            WeatherDetailsVar = new WeatherDetails();
            MainParamsDetails = new MainParams();
            WindDetails = new Wind();
            Sys = new SysDetails();
        }

        public Weather(string name, CoordinateDetails coordinate, WeatherDetails weatherDetailsVar, MainParams mainParams, Wind wind, SysDetails sys)
        {
            Name = name;
            Coordinates = coordinate;
            WeatherDetailsVar = weatherDetailsVar;
            MainParamsDetails = mainParams;
            WindDetails = wind;
            Sys = sys;
        }


        public override string ToString()
        {
            string nameStr = "\n\n" + Name;
            string coordStr = $"\n\nCoordinates:\n\n\tLatitude: {Coordinates.Latitude}\n\n\tLongitude: {Coordinates.Longitude}";
            string weatherDetailsStr = $"\n\nCurrentWeather: {WeatherDetailsVar.CurrentWeather}\n\nDescription: {WeatherDetailsVar.Desc}";
            string mainParamsStr = $"\n\nTemp:\n\n\tMin - {MainParamsDetails.MinTemp}\n\n\tMax - {MainParamsDetails.MaxTemp}\n\n\tAvg. - {MainParamsDetails.Temp}\n\nPressure: {MainParamsDetails.Pressure}\n\nHumidity: {MainParamsDetails.Humidity}";
            string windDetailsStr = $"\n\nWind:\n\n\tSpeed: {WindDetails.Speed}\n\n\tDegree: {WindDetails.Deg}";
            string sysDetailsStr = $"\n\nCountry: {Sys.Country}";
            string result = nameStr + coordStr + weatherDetailsStr + mainParamsStr + windDetailsStr + sysDetailsStr;
            return result;
        }

        public static async Task<Weather> GetWeather()
        {
            Console.Write("\n\nPlease input a city name to get weather for : ");
            string cityName = Console.ReadLine();
            Console.Write("\n\n");
            string result;
            try
            {
                result = await client.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid=5d6edcfd015c6df6d879f1bca2fc2344");
            }
            catch
            {
                Console.Write("Invalid city details detected, Please try again!");
                return null;
            }

            Dictionary<string, dynamic> jsonAsDictionary = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(result);

            var coord = jsonAsDictionary[Constants.coordinates];
            double lat = double.Parse(coord[Constants.coordLat].ToString());
            double lon = double.Parse(coord[Constants.coordLon].ToString());
            CoordinateDetails coordinateObj = new CoordinateDetails(lat, lon);

            var weatherDetails = jsonAsDictionary[Constants.weather];
            string currentWeather = weatherDetails[0][Constants.weatherCurrentStatus];
            string desc = weatherDetails[0][Constants.weatherDescription];
            WeatherDetails weatherDetailsObj = new WeatherDetails(currentWeather: currentWeather, desc: desc);

            var mainParams = jsonAsDictionary[Constants.main];
            double avgTemp = double.Parse(mainParams[Constants.mainAvgTemp].ToString());
            double minTemp = double.Parse(mainParams[Constants.mainTempMin].ToString());
            double maxTemp = double.Parse(mainParams[Constants.mainTempMax].ToString());
            int pressure = Convert.ToInt32(mainParams[Constants.mainPressure].ToString());
            int humidity = Convert.ToInt32(mainParams[Constants.mainHumidity].ToString());
            MainParams mainParamsObj = new MainParams(temp: avgTemp, minTemp: minTemp, maxTemp: maxTemp, pressure: pressure, humidity: humidity);

            var windDetails = jsonAsDictionary[Constants.wind];
            double speed = double.Parse(windDetails[Constants.windSpeed].ToString());
            double deg = double.Parse(windDetails[Constants.windDeg].ToString());
            Wind windObj = new Wind(speed: speed, deg: deg);

            var sysDetails = jsonAsDictionary[Constants.sys];
            string country = sysDetails[Constants.sysCountry];
            TimeSpan sunRise = TimeSpan.FromMilliseconds(Convert.ToInt32(sysDetails[Constants.sysSunRise].ToString()));
            TimeSpan sunSet = TimeSpan.FromMilliseconds(Convert.ToInt32(sysDetails[Constants.sysSunSet].ToString()));
            SysDetails sysObj = new SysDetails(country: country, sunRise: sunRise, sunSet: sunSet);
            string name = jsonAsDictionary[Constants.name];

            Weather weather = new Weather(name: name, coordinate: coordinateObj, weatherDetailsVar: weatherDetailsObj, mainParams: mainParamsObj, wind: windObj, sys: sysObj);
            return weather;
        }

    }
}