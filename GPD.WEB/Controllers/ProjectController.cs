using System.Collections.Generic;
using System.Web.Http;

namespace GPD.WEB.Controllers
{
    using ServiceEntities.BaseEntities;
    using ServiceEntities.ResponseEntities.AddProject;
    using ServiceEntities.ResponseEntities.ProjectsList;

    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ProjectController : ApiController
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Projects List
        /// </summary>
        /// <param name="partnerName">Partner Name</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="searchTerm">Search Term</param>
        /// <returns></returns>
        [Route("api/{partnerName}/Project/List/{pageSize:int}/{pageIndex:int}/{searchTerm}")]
        [HttpGet]
        [AllowAnonymous]
        public ProjectsListResponse GetProjectsList(string partnerName, int pageSize = -1, int pageIndex = -1 , string searchTerm = "")
        {
            pageSize = (pageSize == -1 || pageSize > Utility.ConfigurationHelper.API_ProjectsListPageMaxSize) ?
                Utility.ConfigurationHelper.API_ProjectsListPageSize : pageSize;
            pageIndex = (pageIndex < 1) ? 1 : pageIndex;

            ProjectsListResponse responseDTO = new Facade.ProjectFacade().GetProjectsList(partnerName, pageSize, pageIndex);
            log.Debug("ProjectsListResponse items-count: " + responseDTO.ProjectList.Count);

            return responseDTO;
        }

        /// <summary>
        /// Get Project Details
        /// </summary>
        /// <param name="partnerName">Partner Name</param>
        /// <param name="projectId">Project Id</param>
        /// <returns></returns>
        [Route("api/{partnerName}/Project/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public ProjectDTO GetProjectDetails(string partnerName, string projectId)
        {
            return new Facade.ProjectFacade().GetProjectById(partnerName, projectId);
        }

        /// <summary>
        /// Add Poject
        /// </summary>
        /// <param name="partnerName">Partner Name</param>
        /// <param name="projectDTO">Project Details</param>
        /// <returns></returns>
        [Route("api/{partnerName}/Project")]
        [HttpPost]
        [AllowAnonymous]
        public AddProjectResponse AddProject(string partnerName, ProjectDTO projectDTO)
        {
            return new Facade.ProjectFacade().AddProject(partnerName, projectDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [Route("api/GetUserProfile")]
        [HttpPost]
        [Authorize]
        public SignInResponseDTO GetUserProfile(string userEmail)
        {
            return new Facade.SignInFacade().GetUserRole(userEmail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/GetPartners")]
        [HttpPost]
        [Authorize]
        public List<PartnerDTO> GetPartners()
        {
            return new Facade.SignInFacade().GetPartners();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/GetGroups")]
        [HttpPost]
        [Authorize]
        public List<GroupDTO> GetGroups()
        {
            return new Facade.SignInFacade().GetGroups();
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [Route("api/GetUsers")]
        [HttpPost]
        [Authorize]
        public List<UserDTO> GetUsers(string searchTerm)
        {
            return new Facade.SignInFacade().GetUsers(searchTerm);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [Route("api/ActDactPartner")]
        [HttpPost]
        [Authorize]
        public string ActDactPartner(string partnerId, bool isActive)
        {
            return new Facade.SignInFacade().ActDactPartner(partnerId, isActive);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        [Route("api/SavePartner")]
        [HttpPost]
        [Authorize]
        public string SavePartner(PartnerDTO partner)
        {
            return new Facade.SignInFacade().SavePartner(partner);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [Route("api/ActDactUser")]
        [HttpPost]
        [Authorize]
        public string ActDactUser(int userId, bool isActive)
        {
            return new Facade.SignInFacade().ActDactUser(userId, isActive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("api/GetUserRoles")]
        [HttpPost]
        [Authorize]
        public List<UserRoleDTO> GetUserRoles(int userId)
        {
            return new Facade.SignInFacade().GetUserRoles(userId);
        }

        

    }
}
