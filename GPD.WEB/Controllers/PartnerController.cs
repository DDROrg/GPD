using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace GPD.WEB.Controllers
{
    using ServiceEntities.BaseEntities;

    /// <summary>
    /// 
    /// </summary>
    public class PartnerController : ApiController
    {
        /// <summary>
        /// Get All Partner Accounts
        /// </summary>
        /// <returns></returns>
        [Route("api/GetPartners")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public List<PartnerDTO> GetPartners()
        {
            return new Facade.SignInFacade().GetPartners();
        }

        /// <summary>
        /// Activate/Deactivated Partner Account
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [Route("api/ActDactPartner")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public string ActDactPartner(string partnerId, bool isActive)
        {
            return new Facade.SignInFacade().ActDactPartner(partnerId, isActive);
        }

        /// <summary>
        /// Add Partner Account
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        [Route("api/AddPartner")]
        [HttpPost]
        [Authorize]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public string AddPartner(PartnerDTO partner)
        {
            return new Facade.SignInFacade().AddPartner(partner);
        }
    }
}
