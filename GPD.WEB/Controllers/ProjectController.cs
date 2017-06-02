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
    public class ProjectController : ApiController
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Projects List
        /// </summary>
        /// <param name="partnerName">Partner Name</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageIndex">Page Index</param>
        /// <returns></returns>
        [Route("api/{partnerName}/Project/List/{pageSize:int}/{pageIndex:int}")]
        [HttpGet]
        public ProjectsListResponse GetProjectsList(string partnerName, int pageSize = -1, int pageIndex = -1)
        {
            pageSize = (pageSize == -1 || pageSize > Utility.ConfigurationHelper.API_ProjectsListPageMaxSize) ?
                Utility.ConfigurationHelper.API_ProjectsListPageSize : pageSize;
            pageIndex = (pageIndex < 1) ? 1 : pageIndex;

            List<ProjectItem> projectsList = new Facade.ProjectFacade().GetProjectsList(partnerName, pageSize, pageIndex);
            ProjectsListResponse responseDTO = new ProjectsListResponse(pageSize, pageIndex);
            if (projectsList != null || projectsList.Count > 0) { responseDTO.ProjectList = projectsList; }

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
        public AddProjectResponse AddProject(string partnerName, ProjectDTO projectDTO)
        {
            return new Facade.ProjectFacade().AddProject(partnerName, projectDTO);
        }
    }
}
