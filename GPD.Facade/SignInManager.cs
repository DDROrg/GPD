using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GPD.Facade
{
    using ENUM = GPD.Utility.EnumHelper;

    /// <summary>
    /// 
    /// </summary>
    public class SignInManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public ENUM.SignInStatus PasswordSignIn(string email, string password)
        {
            return ENUM.SignInStatus.Success;
        }
    }
}
