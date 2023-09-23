using Regna.VM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class MechanicVM : BaseEntity
    {
        public long MechanicId { get; set; }
        public string MechanicName { get; set; }
        public bool IsGeneric { get; set; }
        public List<Listener> Listeners { get; set; }
        public List<ConditionVM> Conditions { get; set; }
        public List<EventVM> Events { get; set; }
    }
}
