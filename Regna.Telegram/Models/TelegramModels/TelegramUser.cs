namespace Regna.Telegram.Models
{
    public class TelegramUser
    {
        public int id { get; set; }
        public bool is_bot { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? username { get; set; }
    }
}
