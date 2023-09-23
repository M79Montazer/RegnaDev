namespace Regna.Telegram.IService
{
    public interface ITelegramService
    {
        public bool SendMessage(int id, string text, List<List<string>> options = null, string webAppUrl = "");
    }
}
