using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{


    [DataContract(Namespace = "http://www.gpd.com", Name = "signInResponse")]
    public class SignInResponseDTO
    {
        #region Constr
        public SignInResponseDTO() : base() { PartnerNames = new List<string>(); }
        #endregion Constr

        [DataMember(Name = "userId", Order = 1)]
        public string UserId;

        [DataMember(Name = "firstName", Order = 2)]
        public string FirstName;

        [DataMember(Name = "lastName", Order = 3)]
        public string LastName;

        [DataMember(Name = "email", Order = 4)]
        public string Email;

        [DataMember(Name = "partnerNames", Order = 5)]
        public List<string> PartnerNames;

        [DataMember(Name = "groupName", Order = 6)]
        public string GroupName;

        [DataMember(Name = "selectedPartner", Order = 7)]
        public string SelectedPartner;
    }
}
