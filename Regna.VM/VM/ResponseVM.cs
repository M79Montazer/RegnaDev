using Regna.VM.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM
{
    public class ResponseVM 
    {
        public ResponseVM()
        {
            ChildResponses = new List<ResponseVM>();
        }
        public List<ResponseVM> ChildResponses { get; set; }
        public CardVM Actor { get; set; }
        //public VariableVM Actor_Variable { get; set; }
        public CardVM Target { get; set; }
        public VariableVM Target_Variable { get; set; }
        //public string FinalAmount { get; set; }
        public ResponseResultType ResponseResultType { get; set; }
    }
}
