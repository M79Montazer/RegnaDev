using Regna.VM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class EventVM : BaseEntity
    {
        public long EventID { get; set; }
        public string EventName { get; set; }
        public Target Target { get; set; }
        public long? Target_CardId { get; set; }
        public string? Target_VariableName { get; set; }
        public ActionOPType ActionOPType { get; set; }
        public Target Base { get; set; }
        public long? Base_CardId { get; set; }
        public string? Base_VariableName { get; set; }
        public string? Amount { get; set; }
        public long MechanicId { get; set; }
        public int? Priority { get; set; }
        public ResponseResultType ResponseResultType { get; set; }
    }
}
