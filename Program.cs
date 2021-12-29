using System;
using System.Net.Http;

namespace API_Integration
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            getResults();
            Console.ReadLine();
        }

        static async void getResults() {
            string result = await client.GetStringAsync("https://api.openweathermap.org/data/2.5/weather?q=Coimbatore&appid=5d6edcfd015c6df6d879f1bca2fc2344");
            Console.Write(result);
        }
    }
}
