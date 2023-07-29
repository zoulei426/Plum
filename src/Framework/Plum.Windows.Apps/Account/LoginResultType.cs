using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Windows.Apps.Account
{
    public enum LoginResultType : byte
    {
        Success = 1,
        InvalidUserNameOrPassword = 2,
        NotAllowed = 3,
        LockedOut = 4,
        RequiresTwoFactor = 5
    }
}