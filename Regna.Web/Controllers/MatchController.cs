using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Regna.VM;
using Regna.Web.Base;

namespace Regna.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class MatchController : RegnaController
    {
        private readonly ICoreApiClient _coreApiClient;
        public MatchController(ICoreApiClient coreApiClient) : base()
        {
            _coreApiClient = coreApiClient;
        }
        [HttpPost]
        public async Task<string> GetMatch(long playerId, long matchId)
        {
            try
            {
                var response = await _coreApiClient.ApiRequest("Match", "GetMatch", new GetMatchPVM { PlayerId = playerId, MatchId = matchId });
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<MatchVM>(responseBody);
                    return JsonConvert.SerializeObject(new { Result = "OK", MatchVM = res });
                }
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
        }
    }
}
