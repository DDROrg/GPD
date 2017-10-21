using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Linq;
using System.Web.Security;
using System.Web.Http.Description;

namespace GPD.Dashboard.Controllers
{
    using Facade.WebAppFacade;
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
        /// Get User Profile by User ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("api/GetUserProfile")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public SignInResponseDTO GetUserProfile(int userId)
        {
            return UserDetailsFacade.GetUserRole(userId);
        }

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
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("api/GetProjectChartData")]
        [HttpPost]
        [Authorize]
        public ChartDTO GetProjectChartData(string partner, string fromDate, string toDate)
        {
            return new Facade.ProjectFacade().GetProjectChartData(partner, fromDate, toDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("api/GetTopProductChartData")]
        [HttpPost]
        [Authorize]
        public ChartDTO GetTopProductChartData(string partner, string fromDate, string toDate)
        {
            return new Facade.ProjectFacade().GetTopProductChartData(partner, fromDate, toDate);
        }

        /// <summary>
        /// Get project count based on app name
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("api/GetAppChartData")]
        [HttpPost]
        [Authorize]
        public ChartDTO GetAppChartData(string partner, string fromDate, string toDate)
        {
            return new Facade.ProjectFacade().GetAppChartData(partner, fromDate, toDate);
        }

        /// <summary>
        /// Get project count based on customer name
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("api/GetTopCustomerChartData")]
        [HttpPost]
        [Authorize]
        public ChartDTO GetTopCustomerChartData(string partner, string fromDate, string toDate)
        {
            return new Facade.ProjectFacade().GetTopCustomerChartData(partner, fromDate, toDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        [Route("api/GetUniqueUserCount")]
        [HttpPost]
        [Authorize]
        public int GetUniqueUserCount(string partner)
        {
            return new Facade.ProjectFacade().GetUniqueUserCount(partner);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("api/GetProjectCount")]
        [HttpPost]
        [Authorize]
        public int GetProjectCount(string partner, string fromDate, string toDate)
        {
            return new Facade.ProjectFacade().GetProjectCount(partner, fromDate, toDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("api/GetBPMCount")]
        [HttpPost]
        [Authorize]
        public int GetBPMCount(string partner, string fromDate, string toDate)
        {
            return new Facade.ProjectFacade().GetBPMCount(partner, fromDate, toDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/GetPartnerCount")]
        [HttpPost]
        [Authorize]
        public int GetPartnerCount()
        {
            return new Facade.ProjectFacade().GetPartnerCount();
        }

        /// <summary>
        /// Get percentage of project with ProductTAG data
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("api/GetPctProjectWithProductTAG")]
        [HttpPost]
        [Authorize]
        public int GetPctProjectWithProductTAG(string partner, string fromDate, string toDate)
        {
            return new Facade.ProjectFacade().GetPctProjectWithProductTAG(partner, fromDate, toDate);
        }

        /// <summary>
        /// Get percentage of project with manufacturer data
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("api/GetPctProjectWithManufacturer")]
        [HttpPost]
        [Authorize]
        public int GetPctProjectWithManufacturer(string partner, string fromDate, string toDate)
        {
            return new Facade.ProjectFacade().GetPctProjectWithManufacturer(partner, fromDate, toDate);
        }



    }
}
