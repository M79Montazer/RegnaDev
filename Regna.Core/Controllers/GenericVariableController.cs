using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Regna.Core.IServices;
using Regna.VM;

namespace Regna.Core.Controllers
{
    [Route("[controller]/[action]")]
    public class GenericVariableController : ControllerBase
    {
        private readonly IAssetService _assetService;
        public GenericVariableController(IAssetService assetService)
        {
            _assetService = assetService;
        }
        [HttpPost]
        public JsonResult GetAllGenericVariables()
        {
            var res = _assetService.GetAllGenericVariables();
            return new JsonResult(res);
        }
        [HttpPost]
        public string GetListOfGenericVariables([FromBody] GetListPVM getListPVM)
        {
            int totalRecordsCount = 0;
            var res = _assetService.GetListOfGenericVariables(getListPVM.jtStartIndex, getListPVM.jtPageSize, ref totalRecordsCount);
            var r = JsonConvert.SerializeObject(new ApiResponseWithRecordsPVM { Res = res, TotalRecordsCount = totalRecordsCount });
            return r;

        }

        [HttpPost]
        public JsonResult AddGenericVariable([FromBody] GenericVariableVM GenericVariableVM)
        {

            var res = _assetService.AddGenericVariable(GenericVariableVM);
            return new JsonResult(res);
        }
        [HttpPost]
        public JsonResult UpdateGenericVariable([FromBody] GenericVariableVM GenericVariableVM)
        {
            var res = _assetService.UpdateGenericVariable(GenericVariableVM);
            return new JsonResult(res);
        }
        [HttpPost]
        public JsonResult DeleteGenericVariable([FromBody] long GenericVariableId)
        {
            var res = _assetService.DeleteGenericVariable(GenericVariableId);
            return new JsonResult(res);
        }
    }
}
