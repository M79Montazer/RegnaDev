using Regna.VM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class CardVM
    {
        public long CardId { get; set; }
        public string CardName { get; set; }
        public long MatchId { get; set; }
        public long UserId { get; set; }
        public long OCardId { get; set; }
        public CardLocation Location { get; set; }
        public int PositionNumber { get; set; }
        public virtual List<VariableVM> Variables { get; set; }
        public virtual List<MechanicVM> Mechanics { get; set; }
    }
}
