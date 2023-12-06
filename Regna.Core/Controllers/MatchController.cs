using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Regna.Core.IServices;
using Regna.VM;

namespace Regna.Core.Controllers
{
    [Route("[controller]/[action]")]
    public class MatchController : ControllerBase
    {
        private IMatchService _matchService;
        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }
        [HttpPost]
        public IActionResult Play(long userId, long matchId, long cardId)
        {
            var res = _matchService.Play(userId, matchId, cardId);
            var r = JsonConvert.SerializeObject(new ApiResponseWithRecordsPVM { Res = res, TotalRecordsCount = 0 });
            return new JsonResult(r);
        }
        [HttpPost]
        public string Action(long userId, long matchId, long cardId, long targetId = 0)
        {
            var res = _matchService.Action(userId, matchId, cardId, targetId);
            var r = JsonConvert.SerializeObject(new ApiResponseWithRecordsPVM { Res = res, TotalRecordsCount = 0 });
            return r;
        }
        [HttpPost]
        public string Pass(long playerId, long matchId)
        {
            var res = _matchService.Pass(playerId, matchId);
            var r = JsonConvert.SerializeObject(new ApiResponseWithRecordsPVM { Res = res, TotalRecordsCount = 0 });
            return r;
        }
        [HttpPost]
        public string StartMatch(long firstPlayerId, long sedoncPlayerId)
        {
            var res = _matchService.StartMatch(firstPlayerId, sedoncPlayerId);
            var r = JsonConvert.SerializeObject(new ApiResponseWithRecordsPVM { Res = res, TotalRecordsCount = 0 });
            return r;
        }
    }
}
