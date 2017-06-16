using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{


    [DataContract(Namespace = "http://www.gpd.com", Name = "partner")]
    public class PartnerDTO
    {
        #region Constr
        public PartnerDTO() : base() { }
        #endregion Constr

        [DataMember(Name = "partnerId", Order = 1)]
        public string partnerId;

        [DataMember(Name = "name", Order = 2)]
        public string Name;

        [DataMember(Name = "url", Order = 3)]
        public string URL;

        [DataMember(Name = "shortDescription", Order = 4)]
        public string ShortDescription;

        [DataMember(Name = "description", Order = 5)]
        public string Description;

        [DataMember(Name = "isActive", Order = 6)]
        public bool IsActive;
    }
}
