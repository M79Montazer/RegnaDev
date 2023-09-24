using Regna.VM;
using System.ComponentModel.DataAnnotations;

namespace Regna.Core.Models
{
    public class MechOCard
    {
        [Key]
        public long MechOCardId{ get; set; }
        public long OCardId { get; set; }
        public long MechanicId { get; set; }

    }
}
