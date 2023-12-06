using Regna.VM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class GenericVariableVM : BaseEntity
    {
        public long GenericVariableId { get; set; }
        public string GenericVariableName { get; set; }
        public VariableType VariableType { get; set; }
        public string DefaultValue { get; set; }
    }
}
