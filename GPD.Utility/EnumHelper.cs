using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPD.Utility.EnumHelper
{
    /// <summary>
    /// Sign in status
    /// </summary>
    public enum SignInStatus
    {
        /// <summary>
        /// Sign in was successful
        /// </summary>
        Success = 0,

        /// <summary>
        /// User is locked out
        /// </summary>
        LockedOut = 1,

        /// <summary>
        /// Sign in requires addition verification (i.e. two factor)
        /// </summary>
        RequiresVerification = 2,

        /// <summary>
        /// Sign in failed
        /// </summary>
        Failure = 3
    }

}
