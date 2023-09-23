using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class OCardVM : BaseEntity
    {
        public long OCardId { get; set; }
        public string OCardName { get; set; }
        public virtual List<OVariableVM> OVariables { get; set; }
        public virtual List<MechanicVM> Mechanics { get; set; }
    }
}
