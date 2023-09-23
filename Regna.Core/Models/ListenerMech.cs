using Regna.VM.Enums;

namespace Regna.Core.Models
{
    public class ListenerMech
    {
        public long ListenerMechId { get; set; }
        public Listener Listener { get; set; }
        public long MechanicId { get; set; }
    }
}
