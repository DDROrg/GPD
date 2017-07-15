﻿using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

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
        /// <param name="pNumber">Project Number</param>
        /// <returns></returns>
        [Route("api/{partnerName}/Project/List/{pageSize:int}/{pageIndex:int}/{searchTerm?}/{pNumber?}")]
        [HttpGet]
        [AllowAnonymous]
        public ProjectsListResponse GetProjectsList(string partnerName, int pageSize, int pageIndex, string searchTerm = null, string pNumber = null)
        {
            pageSize = (pageSize == -1 || pageSize > Utility.ConfigurationHelper.API_ProjectsListPageMaxSize) ?
                Utility.ConfigurationHelper.API_ProjectsListPageSize : pageSize;
            pageIndex = (pageIndex < 1) ? 1 : pageIndex;

            ProjectsListResponse responseDTO = null;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                responseDTO = new Facade.ProjectFacade().GetProjectsList(partnerName, pageSize, pageIndex);
            }
            else
            {
                responseDTO = new Facade.ProjectFacade().GetProjectsList(partnerName, pageSize, pageIndex, searchTerm.Trim());
            }
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
    }
}
