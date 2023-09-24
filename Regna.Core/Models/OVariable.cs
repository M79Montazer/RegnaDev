using Regna.VM.Enums;
using Regna.VM;
using System.ComponentModel.DataAnnotations;

namespace Regna.Core.Models
{
    public class OVariable : BaseEntity
    {
        [Key]
        public long OVariableId { get; set; }
        public string OVariableName { get; set; }
        public long OCardID { get; set; }
        public VariableType VariableType { get; set; }
        public string InitialValue { get; set; }
        //public string VariableValue { get; set; }
        public bool IsGeneric { get; set; }
    }
}
