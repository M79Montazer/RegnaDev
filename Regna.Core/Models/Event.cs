﻿using Regna.VM.Enums;
using Regna.VM;
using System.ComponentModel.DataAnnotations;

namespace Regna.Core.Models
{
    public class Event : BaseEntity
    {
        [Key]
        public long EventID { get; set; }
        public string EventName { get; set; }
        public long MechanicId { get; set; }
        public Target Target { get; set; }
        public long? Target_CardId { get; set; }
        public string? Target_VariableName { get; set; }
        public ActionOPType ActionOPType { get; set; }
        public Target Base { get; set; }
        public long? Base_CardId { get; set; }
        public string? Base_VariableName { get; set; }
        public string? Amount { get; set; }
        public int? Priority { get; set; }
        public ResponseResultType ResponseResultType { get; set; }

    }
}
