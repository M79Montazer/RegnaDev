using Regna.Telegram.Models.TelegramModels;

namespace Regna.Telegram.Models.TelegramMethodsVM
{
    public class SendMessageVM
    {
        public int chat_id { get; set; }
        public string text { get; set; }
        public object reply_markup { get; set; }
    }
}
