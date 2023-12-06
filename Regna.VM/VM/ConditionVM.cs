using Regna.VM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class ConditionVM : BaseEntity
    {
        public long ConditionId { get; set; }
        public string ConditionName { get; set; }
        public long MechanicId { get; set; }
        public CompareOPType CompareOPType { get; set; }
        public long FirstVariableId { get; set; }
        public long? SecondVariableId { get; set; }
        public string? SecondValue { get; set; }
    }
}
