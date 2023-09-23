using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Regna.Core;
using Regna.Web.Base;
using Regna.Web.Models;
using System.Diagnostics;
using System.Net.Http;

namespace Regna.Web.Controllers
{
    public class HomeController : RegnaController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
            : base()
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("GamePlay");
        }

        public async Task<IActionResult> Privacy()
        {
            List<WeatherForecast> w = new ();

            try
            {
                HttpClient httpClient = new HttpClient();
                var data = new { age = 30 };
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(ServiceUrl+ "/WeatherForecast/Get", 30);

                // Check if the request was successful (HTTP status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseBody = await response.Content.ReadAsStringAsync();
                    w = JsonConvert.DeserializeObject<List<WeatherForecast>>(responseBody);
                    // Process the response data
                    Console.WriteLine(responseBody);
                }
                else
                {
                    // Handle the error case if necessary
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return View();
        }

        public IActionResult GamePlay()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}