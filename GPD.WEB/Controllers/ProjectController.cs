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
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
        static List<ProjectDTO> projectsList = new List<ProjectDTO>();


        /// <summary>
        /// Get list of all Projects
        /// </summary>
        /// <returns></returns>
        public List<ProjectDTO> Get()
        {
            log.Debug("Test log message");
            string userId = "";
            return new Facade.ProjectFacde().GetAll(userId);
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
            return new Facade.ProjectFacde().Add(projectDTO); 
        }
    }
}
