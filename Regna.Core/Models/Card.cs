using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.Core.Models
{
    public class Card
    {
        public long CardId { get; set; }
        public string CardName { get; set; }
        public long MatchId { get; set; }
        public long UserId { get; set; }
        //public virtual List<Variable> Variables { get; set; }
        //public virtual List<Mechanic> Mechanics { get; set; }
    }
}
