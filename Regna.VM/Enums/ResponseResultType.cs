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
        Drawed = 1,
        Played = 2,
        Actioned = 3,
        Damaged = 4,
        Healed = 5,
        Killed = 6,
        VariableChanged = 7,
    }
}
