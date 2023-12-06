using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using Regna.Core;
using Regna.VM;
using Regna.VM.Enums;
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
            return View();
        }

        public IActionResult GamePlay()
        {
            return View();
        }

        #region Variable Builder
        public IActionResult VariableBuilder(long OCardId = 0)
        {
            if (OCardId == 0)
            {
                return RedirectToAction("Index");
            }
            ViewData["OCardId"] = OCardId;


            var dict = Enum.GetValues(typeof(VariableType))
               .Cast<VariableType>()
               .ToDictionary(t => (int)t, t => t.ToString());
            ViewData["VarTypes"] = dict;
            return View();
        }
        [HttpPost]
        public async Task<string> GetListOfOVariables(int jtStartIndex, int jtPageSize, long OCardId = 0)
        {
            List<OVariableVM> oVariables = new();
            try
            {
                var response = await _coreApiClient.ApiRequest("OVariable", "GetListOfOVariables", new GetListPVM { jtStartIndex = jtStartIndex, jtPageSize = jtPageSize, ParentId = OCardId });
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

        public async Task<string> AddOVariable(OVariableVM oVariableVM)
        {
            try
            {
                var response = await _coreApiClient.ApiRequest("OVariable", "AddOVariable", oVariableVM);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var addedOVariable = JsonConvert.DeserializeObject<OVariableVM>(responseBody);
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = addedOVariable });
                }
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }

        public async Task<string> UpdateOVariable(OVariableVM oVariableVM)
        {
            try
            {
                var response = await _coreApiClient.ApiRequest("OVariable", "UpdateOVariable", oVariableVM);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var addedOVariable = JsonConvert.DeserializeObject<OVariableVM>(responseBody);
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = addedOVariable });
                }
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }

        public async Task<string> DeleteOVariable(long oVariableId)
        {
            try
            {
                var response = await _coreApiClient.ApiRequest("OVariable", "DeleteOVariable", oVariableId);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK" });
                }
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }
        #endregion

        #region Card Builder
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
                var response = await _coreApiClient.ApiRequest("OCard", "GetListOfOCards", new GetListPVM { jtStartIndex = jtStartIndex, jtPageSize = jtPageSize });
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
                    return JsonConvert.SerializeObject(new { Result = "OK" });
                }
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }
        #endregion

        #region Generic OVariable Builder
        public IActionResult GenericVariable()
        {


            var dict = Enum.GetValues(typeof(VariableType))
               .Cast<VariableType>()
               .ToDictionary(t => (int)t, t => t.ToString());
            ViewData["VarTypes"] = dict;
            return View();
        }
        [HttpPost]
        public async Task<string> GetListOfGenericVariables(int jtStartIndex, int jtPageSize)
        {
            List<GenericVariableVM> oCards = new();
            try
            {
                var response = await _coreApiClient.ApiRequest("GenericVariable", "GetListOfGenericVariables", new GetListPVM { jtStartIndex = jtStartIndex, jtPageSize = jtPageSize });
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

        public async Task<string> AddGenericVariable(GenericVariableVM oCardVM)
        {
            try
            {
                var response = await _coreApiClient.ApiRequest("GenericVariable", "AddGenericVariable", oCardVM);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var addedGenericVariable = JsonConvert.DeserializeObject<GenericVariableVM>(responseBody);
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = addedGenericVariable });
                }
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }

        public async Task<string> UpdateGenericVariable(GenericVariableVM oCardVM)
        {
            try
            {
                var response = await _coreApiClient.ApiRequest("GenericVariable", "UpdateGenericVariable", oCardVM);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var addedGenericVariable = JsonConvert.DeserializeObject<GenericVariableVM>(responseBody);
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = addedGenericVariable });
                }
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }

        public async Task<string> DeleteGenericVariable(long GenericVariableId)
        {
            try
            {
                var response = await _coreApiClient.ApiRequest("GenericVariable", "DeleteGenericVariable", GenericVariableId);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK" });
                }
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }
        #endregion





    }
}