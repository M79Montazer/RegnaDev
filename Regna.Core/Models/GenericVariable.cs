using Regna.VM;
using Regna.VM.Enums;

namespace Regna.Core.Models
{
    public class GenericVariable : BaseEntity
    {
        public long GenericVariableId { get; set; }
        public string GenericVariableName { get; set; }
        public VariableType VariableType { get; set; }
        public string DefaultValue { get; set; }
    }
}
