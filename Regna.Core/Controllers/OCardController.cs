using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Regna.Core.IServices;
using Regna.VM;

namespace Regna.Core.Controllers
{
    [Route("[controller]/[action]")]
    public class OCardController : ControllerBase
    {
        private readonly IAssetService _assetService;
        public OCardController(IAssetService assetService)
        {
            _assetService = assetService;
        }
        [HttpPost]
        public JsonResult GetAllOCards()
        {
            var res = _assetService.GetAllOCards();
            return new JsonResult(res);
        }
        [HttpPost]
        public string GetListOfOCards([FromBody] GetListPVM getListPVM)
        {
            int totalRecordsCount = 0;
            var res = _assetService.GetListOfOCards(getListPVM.jtStartIndex, getListPVM.jtPageSize, ref totalRecordsCount);
            var r = JsonConvert.SerializeObject(new ApiResponseWithRecordsPVM { Res = res, TotalRecordsCount = totalRecordsCount });
            return r;

        }

        [HttpPost]
        public JsonResult AddOCard([FromBody] OCardVM OCardVM)
        {

            var res = _assetService.AddOCard(OCardVM);
            return new JsonResult(res);
        }
        [HttpPost]
        public JsonResult UpdateOCard([FromBody] OCardVM OCardVM)
        {
            var res = _assetService.UpdateOCard(OCardVM);
            return new JsonResult(res);
        }
        [HttpPost]
        public JsonResult DeleteOCard([FromBody] long OCardId)
        {
            var res = _assetService.DeleteOCard(OCardId);
            return new JsonResult(res);
        }
    }
}
