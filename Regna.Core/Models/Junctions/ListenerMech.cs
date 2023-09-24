using Regna.VM.Enums;
using System.ComponentModel.DataAnnotations;

namespace Regna.Core.Models
{
    public class ListenerMech
    {
        [Key]
        public long ListenerMechId { get; set; }
        public Listener Listener { get; set; }
        public long MechanicId { get; set; }
    }
}
