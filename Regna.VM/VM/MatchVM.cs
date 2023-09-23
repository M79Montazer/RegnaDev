using Regna.VM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class MatchVM : BaseEntity
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
        public UserVM FirstPlayer { get; set; }
        public UserVM SecondPlayer { get; set; }
    }
}
