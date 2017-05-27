﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GPD.WEB.Controllers
{
    using ServiceEntities;

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
        public ProjectsListResponseDTO GetProjectsList(string partnerName, int pageSize = -1, int pageIndex = -1)
        {
            pageSize = (pageSize == -1 || pageSize > 100) ? 100 : pageSize;
            pageIndex = (pageIndex < 1) ? 1 : pageIndex;

            List<ProjectDTO_Extended> projectsList = new Facade.ProjectFacde().GetProjectsList(partnerName, pageSize, pageIndex);
            ProjectsListResponseDTO responseDTO = new ProjectsListResponseDTO(pageSize, pageIndex);
            if(projectsList != null || projectsList.Count > 0) { responseDTO.ProjectsList = projectsList; }

            return responseDTO;
        }

        /// <summary>
        /// Get Project Details
        /// </summary>
        /// <param name="partnerName">Partner Name</param>
        /// <param name="projectId">Project Id</param>
        /// <returns></returns>
        public ProjectDTO_Extended GetProjectDetails(string partnerName, string projectId)
        {
            return new Facade.ProjectFacde().GetProjectById(partnerName, projectId);
        }

        /// <summary>
        /// Add Poject
        /// </summary>
        /// <param name="partnerName">Partner Name</param>
        /// <param name="projectData">Project Details</param>
        /// <returns></returns>
        public AddProjectResponseDTO AddProject(string partnerName, ProjectDTO projectData)
        {
            return new Facade.ProjectFacde().Add(partnerName, projectData);
        }
    }
}
