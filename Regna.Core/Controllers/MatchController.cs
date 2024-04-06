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
        public string Play([FromBody] PlayPVM playPVM)
        {
            var res = _matchService.Play(playPVM.UserId, playPVM.MatchId, playPVM.CardId);
            var r = JsonConvert.SerializeObject(res);
            return r;
        }
        [HttpPost]
        public string Action([FromBody] ActionPVM actionPVM)
        {
            var res = _matchService.Action(actionPVM.UserId, actionPVM.MatchId, actionPVM.CardId, actionPVM.TargetId);
            var r = JsonConvert.SerializeObject(res);
            return r;
        }
        [HttpPost]
        public string Pass([FromBody] PassPVM passPVM)
        {
            var res = _matchService.Pass(passPVM.PlayerId, passPVM.MatchId);
            var r = JsonConvert.SerializeObject(res);
            return r;
        }
        [HttpPost]
        public string StartMatch(long firstPlayerId, long sedoncPlayerId)
        {
            var res = _matchService.StartMatch(firstPlayerId, sedoncPlayerId);
            var r = JsonConvert.SerializeObject(res);
            return r;
        }
        [HttpPost]
        public string GetMatch([FromBody] GetMatchPVM getMatchPVM)
        {
            var res = _matchService.GetMatchVM(getMatchPVM.PlayerId, getMatchPVM.MatchId);
            var r = JsonConvert.SerializeObject(res);
            return r;
        }
        [HttpPost]
        public string ClearBoard(long matchId)
        {
            var res = _matchService.ClearBoard(matchId);
            var r = JsonConvert.SerializeObject(res);
            return r;
        }
    }
}
