namespace Regna.Telegram.Models
{
    public class TelegramMessage
    {
        public int message_id { get; set; }
        public TelegramUser from { get; set; }
        public int? date { get; set; }
        public string? text { get; set; }
    }
}
