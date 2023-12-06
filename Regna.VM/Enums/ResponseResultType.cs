using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM.Enums
{
    public enum ResponseResultType
    {
        Passed = -1,
        None = 0,
        Played = 1,
        Actioned = 2,
        Damaged = 3,
        Healed = 4,
        Killed = 5,
        VariableChanged = 6,
    }
}
