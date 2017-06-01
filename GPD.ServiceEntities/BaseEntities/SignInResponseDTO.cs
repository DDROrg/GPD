﻿using System.Collections.Generic;
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

        [DataMember(Name = "partnerNames", Order = 4)]
        public List<string> PartnerNames;

        [DataMember(Name = "groupName", Order = 5)]
        public string GroupName;

        [DataMember(Name = "signInStatus", Order = 6)]
        public int SignInStatus;
    }
}
