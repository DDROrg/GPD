using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Web.Security;
//using System.Web.Http.Description;

namespace GPD.WEB.Controllers
{
    using ServiceEntities.BaseEntities;
    using ServiceEntities.ResponseEntities;
    using ServiceEntities.ResponseEntities.ProjectsList;
    using System.Web;

    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ProjectController : ApiController
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ///// <summary>
        ///// Get Projects List
        ///// </summary>
        ///// <param name="partnerName">Partner Name</param>
        ///// <returns></returns>
        //[Route("api/{partnerName}/Project/List")]
        //[HttpGet]
        //[AllowAnonymous]
        //public ProjectsListResponse GetProjectsList(string partnerName)
        //{
        //    ProjectsListResponse responseDTO = new Facade.ProjectFacade().GetProjectsList(partnerName, Utility.ConfigurationHelper.API_ProjectsListPageMaxSize, 1, "", "");
        //    log.Debug("ProjectsListResponse items-count: " + responseDTO.ProjectList.Count);
        //    return responseDTO;
        //}

        /// <summary>
        /// Get Projects List
        /// </summary>
        /// <param name="partnerName">Partner Name</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="searchTerm">Search Term</param>
        /// <param name="fromDate">From Date as string in UTC format (2017-08-03T140:00:00.000Z)</param>
        /// <param name="toDate">To Date as string in UTC format (2017-08-03T140:00:00.000Z)</param>
        /// <param name="pIdentifier">Project Identifier</param>
        /// <returns></returns>
        [Route("api/{partnerName}/Project/List/{pageSize:int}/{pageIndex:int}/{searchTerm?}")]
        [HttpGet]
        [AllowAnonymous]
        public ProjectsListResponse GetProjectsList(
            string partnerName, 
            int pageSize, 
            int pageIndex, 
            string fromDate, 
            string toDate, 
            string searchTerm = null, 
            string pIdentifier = null)
        {
            DateTime fromDateTime = DateTime.MinValue, toDateTime = DateTime.MinValue;
            int userId = -1;

            try
            {
                fromDateTime = Convert.ToDateTime(fromDate);
                toDateTime = Convert.ToDateTime(toDate);

                if (DateTime.Compare(fromDateTime.AddMonths(3), toDateTime) == -1)
                    toDateTime = fromDateTime.AddMonths(3);
            }
            catch
            {
                toDateTime = DateTime.Now;
                fromDateTime = DateTime.Now.AddMonths(-3);
            }

            try
            {
                pageIndex = (pageIndex < 1) ? 1 : pageIndex;
                pageSize = (pageSize == -1 || pageSize > Utility.ConfigurationHelper.API_ProjectsListPageMaxSize) ?
                    Utility.ConfigurationHelper.API_ProjectsListPageSize : pageSize;

                string encryptedValue = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value;
                userId = int.Parse(FormsAuthentication.Decrypt(encryptedValue).Name);

                searchTerm = (string.IsNullOrWhiteSpace(searchTerm)) ? null : searchTerm.Trim();
                pIdentifier = (string.IsNullOrWhiteSpace(pIdentifier)) ? null : pIdentifier.Trim();
                return new Facade.ProjectFacade().GetProjectsList(partnerName, userId, pageSize, pageIndex, 
                    string.Format("{0:yyyy-MM-dd}", fromDateTime), string.Format("{0:yyyy-MM-dd}", toDateTime), searchTerm, pIdentifier);
            }
            catch (Exception exc)
            {
                log.Error(exc);
                return new ProjectsListResponse();
            }            
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
            int userIdentifier, userId = -1;

            try
            {
                System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

                if (headers.Contains("user-identifier") && int.TryParse(headers.GetValues("user-identifier").First(), out userIdentifier))
                    userId = (userIdentifier > 0) ? userIdentifier : -1;
            }
            catch {
                userId = -1;
            }            

            // add project inot DB
            return new Facade.ProjectFacade().AddProject(partnerName, userId, projectDTO);
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
        /// Activate/Deactivated Projects based on ids list
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

        /// <summary>
        /// Delete Projects based on ids list
        /// </summary>
        /// <param name="projectList">List Of Project IDs</param>
        /// <param name="deleteFlag">Project Delete Flag</param>
        /// <returns></returns>
        [Route("api/DeleteProjectList")]
        [HttpPost]
        [Authorize]
        public DeleteProjectListResponse DeleteProjectList(List<string> projectList, bool deleteFlag)
        {
            if (projectList == null || projectList.Count == 0)
                return new DeleteProjectListResponse() { Message = "Project List is Empty." };

            return new Facade.ProjectFacade().DeleteProjectList(projectList, deleteFlag);
        }
    }
}
