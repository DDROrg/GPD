﻿using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace GPD.Dashboard.Controllers
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
        /// Get User Profile by Email
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [Route("api/GetUserProfile")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public SignInResponseDTO GetUserProfile(string userEmail)
        {
            return new Facade.SignInFacade().GetUserRole(userEmail);
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
        public List<LineChartDTO> GetProjectChartData(string partner, string fromDate, string toDate)
        {
            return new Facade.ProjectFacade().GetProjectChartData(partner, fromDate, toDate);
        }

    }
}