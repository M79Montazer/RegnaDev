using Microsoft.AspNetCore.Mvc;
//using Regna.Core.IServices;
//using Regna.Core.Services;
using Regna.Telegram.IService;
using Regna.Telegram.Models;
using Regna.Telegram.Models.TelegramModels;
using System.Text.Json;

namespace Regna.Telegram.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TelegramController : ControllerBase
    {
        private int last_updateId = 0;

        private readonly ILogger<TelegramController> _logger;
        private readonly ITelegramService _telegramService;

        public TelegramController(ILogger<TelegramController> logger, ITelegramService telegramService)
        {
            _logger = logger;
            _telegramService = telegramService;
        }


        [HttpPost]
        public void Get(TelegramUpdate u)
        {
            var k = new List<List<string>>();
            k.Add(new List<string>());
            k.FirstOrDefault().Add("OPEN");
            //web_app = new WebAppInfo { url = } https://www.oathofregna.freehost.io/WebView/Index.html
            //});
            _telegramService.SendMessage(u.message.from.id, u.message.text, k, "https://4954-5-113-132-42.ngrok-free.app");
            return;
        }

        [HttpPost]
        public async Task<IActionResult> TriggerUpdateAsync()
        {

            var url = "https://api.telegram.org/bot6363934670:AAFNfKvY-KDSQCDPzw6zHk2V80Mcpx_4DMU/getupdates?offset=" + last_updateId.ToString();
            using HttpClient client = new HttpClient();

            string json = await (client.GetAsync(url)).Result.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<Response>(json);
            List<TelegramUpdate> updates = response.result;
            if (updates != null && updates.Count > 0)
            {
                last_updateId = updates.FirstOrDefault().update_id + 1;
                Get(updates.FirstOrDefault());
            }
            return new JsonResult("Done");
        }

        private class Response
        {
            public List<TelegramUpdate> result { get; set; }
        }
    }
}