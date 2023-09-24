using Regna.VM;
using Regna.VM.Enums;

namespace Regna.Core.Models
{
    public class Match : BaseEntity
    {
        public long MatchId { get; set; }
        public long FirstPlayerId { get; set; }
        public long SecondPlayerId { get; set; }
        public int PlayerTurn { get; set; }
        public GamePhase Phase { get; set; }
        public float FirstPlayerRS { get; set; }
        public float SecondPlayerRS { get; set; }
        public float FirstPlayerMorale { get; set; }
        public float SecondPlayerMorale { get; set; }
    }
}
