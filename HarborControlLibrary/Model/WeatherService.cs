using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HarborControl.Model
{
    public class WeatherService
    {
        private static string YOUR_API_KEY = "eeab705f2d394f62873a04f85181f448";
        public CurrentWeather GetCurrentWeather()
        {
            var json = RunAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<CurrentWeather>(json);
        }

        private async Task<string> RunAsync()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://samples.openweathermap.org/data/2.5/weather?q=Durban&appid=" + YOUR_API_KEY);
          
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var result = "";
            try
            {
                result = await GetWeather(client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        private static async Task<string> GetWeather(HttpClient cons)
        {
            string weather = "";
            using (cons)
            {
                HttpResponseMessage res = await cons.GetAsync("");
                res.EnsureSuccessStatusCode();
                if (res.IsSuccessStatusCode)
                {
                    weather = await res.Content.ReadAsStringAsync();
                }
            }
            return weather;
        }
    }
}
