using System;
using System.Collections.Generic;
using System.Web.Http;

namespace GPD.WEB.Controllers
{
    using ServiceEntities.BaseEntities;
    using ServiceEntities;
    using System.Xml.Linq;
    using System.Runtime.Serialization;
    using Facade.WebAppFacade;

    /// <summary>
    /// User APIs List
    /// </summary>
    public class UserController : ApiController
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get User Profile by Email
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [Route("api/GetUserProfile")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public SignInResponseDTO GetUserProfile(string userEmail)
        {
            return new Facade.SignInFacade().GetUserRole(userEmail);
        }

        /// <summary>
        /// Get All Users List
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [Route("api/GetUsers")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public List<UserDTO> GetUsers(string searchTerm)
        {
            return new Facade.SignInFacade().GetUsers(searchTerm);
        }

        /// <summary>
        /// Activate/Deactivated User Account
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [Route("api/ActDactUser")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public string ActDactUser(int userId, bool isActive)
        {
            return new Facade.SignInFacade().ActDactUser(userId, isActive);
        }

        /// <summary>
        /// Get a List of Roles for a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("api/GetUserRoles")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public List<UserRoleDTO> GetUserRoles(int userId)
        {
            return new Facade.SignInFacade().GetUserRoles(userId);
        }

        /// <summary>
        ///  Get a List of Available Roles for a User
        /// </summary>
        /// <returns></returns>
        [Route("api/GetGroups")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public List<GroupDTO> GetGroups()
        {
            return new Facade.SignInFacade().GetGroups();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="partnerId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Route("api/DeleteUserRole")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public string DeleteUserRole(int userId, string partnerId, int groupId)
        {
            return new Facade.SignInFacade().DeleteUserRole(userId, partnerId, groupId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="partnerId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Route("api/AddUserRole")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public string AddUserRole(int userId, string partnerId, int groupId)
        {
            return new Facade.SignInFacade().AddUserRole(userId, partnerId, groupId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("api/RegisterUser")]
        [HttpPost]
        [AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public UserRegistrationStatusDTO RegisterUser(UserDetailsTDO user)
        {
            UserRegistrationStatusDTO retVal = new UserRegistrationStatusDTO() { Status = false };
            XDocument xDoc = new XDocument();
            int errorCode;
            string errorMsg, requestIpAddress = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(user.CompanyDetails.Name))
                    user.CompanyDetails = new CompanyDetailsDTO();

                // get XML based on UserRegistrationDTO object
                using (var writer = xDoc.CreateWriter())
                {
                    var serializer = new DataContractSerializer(user.GetType());
                    serializer.WriteObject(writer, user);
                }                

                try { requestIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress; }
                catch { }

                // add a user to repository
                int userId = UserDetailsFacade.AddUserDetails(xDoc, requestIpAddress, out errorCode, out errorMsg);

                if (userId != -1)
                {
                    retVal = new UserRegistrationStatusDTO()
                    {
                        UserId = userId,
                        Status = true
                    };

                    return retVal;
                }

                if (errorCode == 0 && !string.IsNullOrEmpty(errorMsg))
                    retVal.Message = errorMsg;
                else
                    throw new Exception(string.Format("Add new UserDeatils() - Database ERROR: ErrorCode: {0}, ErrorMsg: {1}", errorCode, errorMsg));
            }
            catch (Exception exc)
            {
                log.Error(exc);
                retVal.Message = "The server encountered an error processing registration. Please try again later.";
            }

            return retVal;
        }
    }
}
