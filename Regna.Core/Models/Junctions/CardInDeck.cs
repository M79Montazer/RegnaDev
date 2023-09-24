using System.ComponentModel.DataAnnotations;

namespace Regna.Core.Models
{
    public class CardInDeck
    {
        [Key]
        public long CardInDeckId { get; set; }
        public long UserId { get; set; }
        public long OCardId { get; set; }
    }
}
