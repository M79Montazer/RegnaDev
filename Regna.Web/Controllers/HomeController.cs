using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Regna.Core;
using Regna.VM;
using Regna.Web.Base;
using Regna.Web.Models;
using System.Diagnostics;
using System.Net.Http;

namespace Regna.Web.Controllers
{
    public class HomeController : RegnaController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICoreApiClient _coreApiClient;
        //private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, ICoreApiClient coreApiClient)
            : base()
        {
            _coreApiClient = coreApiClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //return RedirectToAction("GamePlay");
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            //List<OCardVM> oCards = new();

            //try
            //{
            //    HttpClient httpClient = new HttpClient();
            //    var data = new { age = 30 };
            //    var url = ServiceUrl + "/OCard/GetAllOCards";
            //    HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, data);

            //    // Check if the request was successful (HTTP status code 200-299)
            //    if (response.IsSuccessStatusCode)
            //    {
            //        // Read the response content as a string
            //        string responseBody = await response.Content.ReadAsStringAsync();
            //        oCards = JsonConvert.DeserializeObject<List<OCardVM>>(responseBody);
            //        // Process the response data
            //        //Console.WriteLine(responseBody);
            //    }
            //    else
            //    {
            //        // Handle the error case if necessary
            //        //Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //}
            return View();
        }

        public IActionResult GamePlay()
        {
            return View();
        }
        public IActionResult CardBuilder()
        {
            return View();
        }
        [HttpPost]
        public async Task<string> GetListOfOCards(int jtStartIndex, int jtPageSize)
        {
            List<OCardVM> oCards = new();
            try
            {
                var response = await _coreApiClient.ApiRequest("OCard", "GetListOfOCards", new GetListPVM{ jtStartIndex = jtStartIndex, jtPageSize = jtPageSize });
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ApiResponseWithRecordsPVM>(responseBody);
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = res.Res, TotalRecordCount = res.TotalRecordsCount });
                }
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }

        public async Task<string> AddOCard(OCardVM oCardVM)
        {
            try
            {
                var response = await _coreApiClient.ApiRequest("OCard", "AddOCard", oCardVM);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var addedOCard = JsonConvert.DeserializeObject<OCardVM>(responseBody);
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = addedOCard });
                }
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }
        
        public async Task<string> UpdateOCard(OCardVM oCardVM)
        {
            try
            {
                var response = await _coreApiClient.ApiRequest("OCard", "UpdateOCard", oCardVM);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var addedOCard = JsonConvert.DeserializeObject<OCardVM>(responseBody);
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = addedOCard });
                }
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }
        
        public async Task<string> DeleteOCard(long oCardId)
        {
            try
            {
                var response = await _coreApiClient.ApiRequest("OCard", "DeleteOCard", oCardId);
                if (response.IsSuccessStatusCode)
                {
                    //string responseBody = await response.Content.ReadAsStringAsync();
                    //var addedOCard = JsonConvert.DeserializeObject<OCardVM>(responseBody);
                    return JsonConvert.SerializeObject(new { Result = "OK"});
                }
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }


    }
}