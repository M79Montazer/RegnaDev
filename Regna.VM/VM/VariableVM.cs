using Regna.VM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class VariableVM
    {
        public long VariableId { get; set; }
        public string VariableName { get; set; }
        public long CardId { get; set; }
        public VariableType VariableType { get; set; }
        //public string InitialValue { get; set; }
        public string VariableValue { get; set; }
        //public bool IsGeneric { get; set; }
    }
}
