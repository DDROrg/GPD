using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace GPD.WEB.Controllers
{
    using ServiceEntities.BaseEntities;
    using ServiceEntities.ResponseEntities;
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
        /// <returns></returns>
        [Route("api/{partnerName}/Project/List")]
        [HttpGet]
        [AllowAnonymous]
        public ProjectsListResponse GetProjectsList(string partnerName)
        {
            ProjectsListResponse responseDTO = new Facade.ProjectFacade().GetProjectsList(partnerName, Utility.ConfigurationHelper.API_ProjectsListPageMaxSize, 1);
            log.Debug("ProjectsListResponse items-count: " + responseDTO.ProjectList.Count);

            return responseDTO;
        }

        /// <summary>
        /// Get Projects List
        /// </summary>
        /// <param name="partnerName">Partner Name</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="searchTerm">Search Term</param>
        /// <param name="pIdentifier">Project Identifier</param>
        /// <returns></returns>
        [Route("api/{partnerName}/Project/List/{pageSize:int}/{pageIndex:int}/{searchTerm?}/{pNumber?}")]
        [HttpGet]
        [AllowAnonymous]
        public ProjectsListResponse GetProjectsList(string partnerName, int pageSize, int pageIndex, string searchTerm = null, string pIdentifier = null)
        {
            pageSize = (pageSize == -1 || pageSize > Utility.ConfigurationHelper.API_ProjectsListPageMaxSize) ?
                Utility.ConfigurationHelper.API_ProjectsListPageSize : pageSize;
            pageIndex = (pageIndex < 1) ? 1 : pageIndex;

            ProjectsListResponse responseDTO = null;
            if (string.IsNullOrWhiteSpace(searchTerm) && string.IsNullOrWhiteSpace(pIdentifier))
                responseDTO = new Facade.ProjectFacade().GetProjectsList(partnerName, pageSize, pageIndex);
            else
            {
                searchTerm = (string.IsNullOrWhiteSpace(searchTerm)) ? null : searchTerm.Trim();
                pIdentifier = (string.IsNullOrWhiteSpace(pIdentifier)) ? null : pIdentifier.Trim();
                responseDTO = new Facade.ProjectFacade().GetProjectsList(partnerName, pageSize, pageIndex, searchTerm, pIdentifier);
            }

            log.Debug("ProjectsListResponse items-count: " + responseDTO.ProjectList.Count);

            return responseDTO;
        }

        /// <summary>
        /// Get Project Details
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns></returns>
        [Route("api/Project/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public ProjectDTO GetProjectDetails(string projectId)
        {
            return new Facade.ProjectFacade().GetProjectById(projectId);
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
        /// <param name="projectId"></param>
        /// <param name="projectDTO"></param>
        /// <returns></returns>
        [Route("api/UpdateProject/{projectId}")]
        [HttpPost]
        [AllowAnonymous]
        public UpdateProjectResponse UpdateProject(string projectId, ProjectDTO projectDTO)
        {
            return new Facade.ProjectFacade().UpdateProject(projectId, projectDTO);
        }


        /// <summary>
        /// Activate/Deactivated Project List
        /// </summary>
        /// <param name="projectList"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [Route("api/ActivateProjectList")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public ActivateProjectListResponse ActivateProjectList(List<string> projectList, bool isActive)
        {
            if (projectList == null || projectList.Count == 0)
                return new ActivateProjectListResponse() { Message = "Project List is Empty." };

            return new Facade.ProjectFacade().ActivateProjectList(projectList, isActive);
        }
    }
}
