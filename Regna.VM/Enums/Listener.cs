using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regna.VM.Enums
{
    public enum Listener
    {
        OnStartOfMatch = 0,
        OnAction = 1,
        OnPlay = 2,
        OnDeath = 3,
        OnStratOfRound = 4,
        OnKill = 5,
        //OnAnyAlly
    }
}
