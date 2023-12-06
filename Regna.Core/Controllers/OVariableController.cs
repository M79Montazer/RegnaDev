using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Regna.Core.IServices;
using Regna.VM;

namespace Regna.Core.Controllers
{
    [Route("[controller]/[action]")]
    public class OVariableController : ControllerBase
    {
        private readonly IAssetService _assetService;
        public OVariableController(IAssetService assetService)
        {
            _assetService = assetService;
        }
        [HttpPost]
        public JsonResult GetAllOVariables()
        {
            var res = _assetService.GetAllOVariables();
            return new JsonResult(res);
        }
        [HttpPost]
        public string GetListOfOVariables([FromBody] GetListPVM getListPVM)
        {
            int totalRecordsCount = 0;
            var res = _assetService.GetListOfOVariables(getListPVM.jtStartIndex, getListPVM.jtPageSize, ref totalRecordsCount, getListPVM.ParentId ?? 0);
            var r = JsonConvert.SerializeObject(new ApiResponseWithRecordsPVM { Res = res, TotalRecordsCount = totalRecordsCount });
            return r;

        }

        [HttpPost]
        public JsonResult AddOVariable([FromBody] OVariableVM OVariableVM)
        {

            var res = _assetService.AddOVariable(OVariableVM);
            return new JsonResult(res);
        }
        [HttpPost]
        public JsonResult UpdateOVariable([FromBody] OVariableVM OVariableVM)
        {
            var res = _assetService.UpdateOVariable(OVariableVM);
            return new JsonResult(res);
        }
        [HttpPost]
        public JsonResult DeleteOVariable([FromBody] long OVariableId)
        {
            var res = _assetService.DeleteOVariable(OVariableId);
            return new JsonResult(res);
        }
    }
}
