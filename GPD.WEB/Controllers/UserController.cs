using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace GPD.WEB.Controllers
{
    using ServiceEntities.BaseEntities;

    /// <summary>
    /// User APIs List
    /// </summary>
    public class UserController : ApiController
    {
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

        
    }
}
