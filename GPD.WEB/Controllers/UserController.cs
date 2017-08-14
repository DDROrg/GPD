using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Http;
using System.Xml.Linq;

namespace GPD.WEB.Controllers
{
    using Facade.WebAppFacade;
    using ServiceEntities;
    using ServiceEntities.BaseEntities;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [Route("api/GetCompanies")]
        [HttpPost]
        [AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public List<CompanyDetailsDTO> GetCompanies(string searchTerm)
        {
            return new Facade.SignInFacade().GetCompanies(searchTerm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [Route("api/GetCompanyDetails")]
        [HttpPost]
        [AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public CompanyDetailsDTO GetCompanyDetails(int countryId)
        {
            return new Facade.SignInFacade().GetCompanyProfile(countryId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>AuthenticateUserStatusDTO</returns>
        [Route("api/AuthenticateUser")]
        [HttpGet]
        [AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public AuthenticateUserStatusDTO AuthenticateUser()
        {
            AuthenticateUserStatusDTO authenticateUser = new AuthenticateUserStatusDTO()
            {
                Message = "Request Not Authenticated."
            };

            try
            {
                var request = System.Web.HttpContext.Current.Request;
                var authHeader = request.Headers["Authorization"];
                var authHeaderVal = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(authHeader);

                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
                {
                    string credentials = Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(authHeaderVal.Parameter));
                    string userEmail = credentials.Substring(0, credentials.IndexOf(':'));
                    string userPassword = credentials.Substring(credentials.IndexOf(':') + 1);

                    int userId = -1;
                    var result = UserDetailsFacade.AuthenticateUser(userEmail, userPassword, out userId);

                    if (userId != -1)
                    {
                        // get user profile
                        var userProfile = new Facade.SignInFacade().GetUserRole(userEmail);

                        if (userProfile != null)
                        {
                            authenticateUser.UserId = userId;
                            authenticateUser.UserEmail = userEmail;
                            authenticateUser.FirstName = userProfile.FirstName;
                            authenticateUser.LastName = userProfile.LastName;
                            authenticateUser.Status = true;
                            authenticateUser.Message = string.Empty;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                log.Error(exc);
            }

            return authenticateUser;
        }
    }
}
