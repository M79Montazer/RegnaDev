using Regna.VM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class OVariableVM : BaseEntity
    {
        public long OVariableId { get; set; }
        public string OVariableName { get; set; }
        public long OCardID { get; set; }
        public VariableType VariableType { get; set; }
        public string InitialValue { get; set; }
        //public string VariableValue { get; set; }
        public bool IsGeneric { get; set; }
    }
}
