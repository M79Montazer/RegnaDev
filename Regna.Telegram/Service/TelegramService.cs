using Regna.Telegram.IService;
using Regna.Telegram.Models;
using Regna.Telegram.Models.TelegramMethodsVM;
using Regna.Telegram.Models.TelegramModels;

namespace Regna.Telegram.Service
{
    public class TelegramService : ITelegramService
    {
        private string uri = "https://api.telegram.org/bot6363934670:AAFNfKvY-KDSQCDPzw6zHk2V80Mcpx_4DMU";
        public bool SendMessage(int id, string text, List<List<string>> options = null, string webAppUrl = "")
        {
            try
            {

                var m = new SendMessageVM();
                var client = new HttpClient() { BaseAddress = new Uri(uri + "/SendMessage") };
                m.chat_id = id;
                m.text = text;

                if (!String.IsNullOrEmpty(webAppUrl) && options != null)
                {
                    var k = new List<List<KeyboardButton>>();
                    var k2 = new List<KeyboardButton>();
                    k2.Add(new KeyboardButton
                    {
                        text = options.FirstOrDefault().FirstOrDefault(),
                        web_app = new WebAppInfo
                        {
                            url = webAppUrl
                        }
                    });
                    k.Add(k2);
                    m.reply_markup = new ReplyKeyboardMarkup
                    {
                        keyboard = k
                    };
                }
                else
                {
                    if (options == null)
                    {
                        m.reply_markup = new ReplyKeyboardRemove { remove_keyboard = true };


                    }
                    else
                    {
                        m.reply_markup = new ReplyKeyboardMarkup
                        {
                            keyboard = options.Select(a => a.Select(b => new KeyboardButton { text = b })
                            .ToList()).ToList()
                        };

                    }
                }
                var apiResponse = client.PostAsJsonAsync("", m).Result.Content.ReadAsStringAsync().Result;

                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
