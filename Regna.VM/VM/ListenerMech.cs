using Regna.VM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class ListenerMech
    {
        public long ListenerMechId { get; set; }
        public Listener Listener { get; set; }
        public long MechanicId { get; set; }
    }
}
