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
        public double FirstPlayerRS { get; set; }
        public double SecondPlayerRS { get; set; }
        public double FirstPlayerMorale { get; set; }
        public double SecondPlayerMorale { get; set; }
        public UserVM FirstPlayer { get; set; }
        public UserVM SecondPlayer { get; set; }
        public List<CardVM> Cards { get; set; }
        public bool FirstPlayerPassed { get; set; }
        public bool SecondPlayerPassed { get; set; }

    }
}
