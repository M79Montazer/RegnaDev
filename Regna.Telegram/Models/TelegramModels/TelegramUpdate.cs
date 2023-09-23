namespace Regna.Telegram.Models
{
    public class TelegramUpdate
    {
        public int update_id { get; set; }
        public TelegramMessage message { get; set; }

    }
}
