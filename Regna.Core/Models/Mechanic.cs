using Regna.VM.Enums;
using Regna.VM;
using System.ComponentModel.DataAnnotations;

namespace Regna.Core.Models
{
    public class Mechanic : BaseEntity
    {
        [Key]
        public long MechanicId { get; set; }
        public string MechanicName { get; set; }
        public bool IsGeneric { get; set; }
        //public List<Listener> Listeners { get; set; }
        //public List<Condition> Conditions { get; set; }
        //public List<Event> Events { get; set; }
    }
}
