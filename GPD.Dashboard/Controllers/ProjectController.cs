using System.Collections.Generic;
using System.Web.Http.Description;
using System.Web.Http;

namespace GPD.Dashboard.Controllers
{
    using Facade.WebAppFacade;
    using ServiceEntities.BaseEntities;

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
        [Route("api/GetCategoriesChartData")]
        [HttpPost]
        [Authorize]
        public ChartDTO GetCategoriesChartData(string partner, string fromDate, string toDate)
        {
            return new Facade.ProjectFacade().GetCategoriesChartData(partner, fromDate, toDate);
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
    }
}
