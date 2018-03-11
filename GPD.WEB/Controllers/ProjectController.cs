using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Linq;
using System.Web.Security;
using System.Xml.Linq;
using System.Text;
//using System.Web.Http.Description;

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
            string apiKeyId = null;

            try
            {
                fromDateTime = Convert.ToDateTime(fromDate);
                toDateTime = Convert.ToDateTime(toDate);

                if (DateTime.Compare(fromDateTime.AddMonths(Utility.ConfigurationHelper.API_ProjectsList_MaxMonthsHistory), toDateTime) == -1)
                    toDateTime = fromDateTime.AddMonths(Utility.ConfigurationHelper.API_ProjectsList_MaxMonthsHistory);
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

                // Service Authentication logic
                this.ServiceAuthentication(this.Request.Headers.Authorization, HttpContext.Current.Request.Cookies, out userId, out apiKeyId);

                // get partners list based on user/partner-key value
                XDocument partnersXDoc = Facade.WebAppFacade.UserDetailsFacade.GetPartnerListAccessTo(userId, apiKeyId, partnerName);

                searchTerm = (string.IsNullOrWhiteSpace(searchTerm)) ? null : searchTerm.Trim();
                pIdentifier = (string.IsNullOrWhiteSpace(pIdentifier)) ? null : pIdentifier.Trim();


                // projects list
                return new Facade.ProjectFacade().GetProjectsList(partnerName, userId, pageSize, pageIndex,  string.Format("{0:yyyy-MM-dd}", fromDateTime), 
                    string.Format("{0:yyyy-MM-dd}", toDateTime), partnersXDoc, searchTerm, pIdentifier);
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

        private void ServiceAuthentication(System.Net.Http.Headers.AuthenticationHeaderValue auth, HttpCookieCollection cookieCollection, out int userId, out string apiKeyId)
        {
            userId = -1;
            apiKeyId = null;

            if (auth != null && (new string[] { "Basic", "Partner" }).Any(T => T.Equals(auth.Scheme, StringComparison.InvariantCultureIgnoreCase)))
            {
                string authValue = UTF8Encoding.UTF8.GetString(Convert.FromBase64String(auth.Parameter));

                if (string.Compare(auth.Scheme, "Partner", StringComparison.OrdinalIgnoreCase) == 0)
                { 
                    apiKeyId = authValue;
                    return;
                }

                string userEmail = authValue.Substring(0, authValue.IndexOf(':'));
                string userPassword = authValue.Substring(authValue.IndexOf(':') + 1);

                // get user-id
                Facade.WebAppFacade.UserDetailsFacade.AuthenticateUser(userEmail, userPassword, out userId);
            }
            else
            {
                string formsCookieName = cookieCollection[FormsAuthentication.FormsCookieName].Value;
                userId = int.Parse(FormsAuthentication.Decrypt(formsCookieName).Name);
            }
        }
    }
}
