using Regna.VM.Enums;
using Regna.VM;


namespace Regna.Core.Models
{
    public class OVariable : BaseEntity
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
