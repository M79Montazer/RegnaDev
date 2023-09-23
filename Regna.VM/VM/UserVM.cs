namespace Regna.VM
{
    public class UserVM : BaseEntity
    {
        public long UserId { get; set; }
        public long TelegramId { get; set; }
        public string? UserName { get; set; }

    }
}