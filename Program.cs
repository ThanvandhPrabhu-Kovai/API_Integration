using System;
using System.Net.Http;
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
