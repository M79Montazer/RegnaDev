using Regna.VM.Enums;
using Regna.VM;
using System.ComponentModel.DataAnnotations;

namespace Regna.Core.Models
{
    public class Condition : BaseEntity
    {
        [Key]
        public long ConditionId { get; set; }
        public string ConditionName { get; set; }
        public long MechanicId { get; set; }
        public CompareOPType CompareOPType { get; set; }
        public long FirstVariableId { get; set; }
        public long? SecondVariableId { get; set; }
        public string? SecondValue { get; set; }
    }
}
