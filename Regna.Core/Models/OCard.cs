using Regna.VM;

namespace Regna.Core.Models
{
    public class OCard : BaseEntity
    {
        public long OCardId { get; set; }
        public string OCardName { get; set; }
        //public virtual List<Variable> Variables { get; set; }
        //public virtual List<Mechanic> Mechanics { get; set; }
    }
}
