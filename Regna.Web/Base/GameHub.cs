using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Regna.VM;

namespace Regna.Web.Base
{
    public class GameHub : Hub
    {
        private readonly ICoreApiClient _coreApiClient;
        public GameHub(ICoreApiClient coreApiClient) : base()
        {
            _coreApiClient = coreApiClient;
        }
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            //await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public async Task Test(string groupName, string move)
        {
            // Contains logic for a player's move
            // Send the move to the other player in the group
            await Clients.OthersInGroup(groupName).SendAsync("Test", move);
        }
        public async Task Action(long userId, long matchId, long cardId, long targetId)
        {
            var message = ""; 
            try
            {
                var response = await _coreApiClient.ApiRequest("Match", "Action", new ActionPVM { MatchId = matchId, CardId = cardId, UserId = userId, TargetId =targetId  });
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ResponseVM>(responseBody);
                    message = JsonConvert.SerializeObject(new { Result = "OK", ResponseVM = res });
                }
            }
            catch (Exception ex)
            {
                message = JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
            }
            await Clients.Group(matchId.ToString()).SendAsync("Actioned", message);
        }
        
        public async Task Play(long userId, long matchId, long cardId)
        {
            // Contains logic for a player's move
            // Send the move to the other player in the group
            var message = "";
            try
            {
                var response = await _coreApiClient.ApiRequest("Match", "Play", new PlayPVM { MatchId = matchId, CardId = cardId, UserId = userId });
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ResponseVM>(responseBody);
                    message = JsonConvert.SerializeObject(new { Result = "OK", ResponseVM = res });
                }
            }
            catch (Exception ex)
            {
                message = JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
            }
            await Clients.Group(matchId.ToString()).SendAsync("Played", message);
        }
        
        public async Task Pass(long userId, long matchId)
        {
            // Contains logic for a player's move
            // Send the move to the other player in the group
            var message = "";
            try
            {
                var response = await _coreApiClient.ApiRequest("Match", "Pass", new PassPVM { MatchId = matchId, PlayerId= userId });
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ResponseVM>(responseBody);
                    message = JsonConvert.SerializeObject(new { Result = "OK", ResponseVM = res });
                }
            }
            catch (Exception ex)
            {
                message = JsonConvert.SerializeObject(new { Result = "ERROR", Message = "there was an error" });
            }
            await Clients.Group(matchId.ToString()).SendAsync("Passed", message);
        }

    }
}
