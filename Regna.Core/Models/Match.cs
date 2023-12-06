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
        public double FirstPlayerRS { get; set; }
        public double SecondPlayerRS { get; set; }
        public double FirstPlayerMorale { get; set; }
        public double SecondPlayerMorale { get; set; }
        public bool FirstPlayerPassed { get; set; }
        public bool SecondPlayerPassed { get; set; }
    }
}
