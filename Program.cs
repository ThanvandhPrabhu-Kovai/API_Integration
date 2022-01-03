using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_Integration
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await GetCityNameFromUserAndShowWeather();
            Console.ReadLine();
        }

        static async Task GetCityNameFromUserAndShowWeather()
        {
            string cityName = Weather.getCityName();
            Weather weather = await Weather.GetWeather(cityName);
            string result = (weather != null) ? weather.ToString() : "";
            Console.Write(result);
        }
    }
}
