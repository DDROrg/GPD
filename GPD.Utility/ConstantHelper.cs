using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPD.Utility.ConstantHelper
{
    /// <summary>
    /// Sign in status
    /// </summary>
    public static class SignInStatus
    {
        public const int Success = 0;
        public const int LockedOut = 1;
        public const int RequiresVerification = 2;
        public const int Failure = 3;
    }

}
