using System;
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
        List<ProjectDTO> projectsList = new List<ProjectDTO>();
        

        /// <summary>
        /// Get list of all Projects
        /// </summary>
        /// <returns></returns>
        public List<ProjectDTO> Get()
        {
            return projectsList;
        }

        /// <summary>
        /// Get project by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectDTO Get(int id)
        {
            try
            {
                return projectsList[id];
            }
            catch
            {
                return new ProjectDTO();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectDTO"></param>
        /// <returns></returns>
        public AddProjectResponseDTO Add(ProjectDTO projectDTO)
        {
            projectsList.Add(projectDTO);
            return new AddProjectResponseDTO(true, projectsList.Count - 1);
        }
    }
}
