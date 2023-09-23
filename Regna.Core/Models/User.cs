using Regna.VM;
using System.ComponentModel.DataAnnotations;

namespace Regna.Core.Models
{
    public class User : BaseEntity
    {
        [Key]
        public long UserId { get; set; }
        public long TelegramId { get; set; }
        public string? UserName { get; set; }

    }
}