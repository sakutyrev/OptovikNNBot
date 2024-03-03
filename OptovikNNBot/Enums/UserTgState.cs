using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptovikNNBot.Enums
{
    public enum UserTgState
    {
        None,
        WaitingForNewAdminId,
        WaitingForExcelFile,
        WaitingForMailing,
        ChoosingGoods
    }
}
