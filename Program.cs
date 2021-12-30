using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using API_Integration;
using System.Threading.Tasks;

namespace API_Integration
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            MainAsync().Wait();
            Console.ReadLine();
        }

        static async Task MainAsync()
        {
            Weather weather = await API_Integration.Weather.GetWeather();
            string result = weather != null ? weather.ToString() : "";
            Console.Write(result);
        }
    }
}
