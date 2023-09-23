using Regna.VM.Enums;
using Regna.VM;

namespace Regna.Core.Models
{
    public class Mechanic : BaseEntity
    {
        public long MechanicId { get; set; }
        public string MechanicName { get; set; }
        public bool IsGeneric { get; set; }
        //public List<Listener> Listeners { get; set; }
        //public List<Condition> Conditions { get; set; }
        //public List<Event> Events { get; set; }
    }
}
