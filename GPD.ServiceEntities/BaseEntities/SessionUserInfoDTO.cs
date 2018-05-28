using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "session-user-info")]
    public class SessionUserInfoDTO
    {
        #region Constr
        public SessionUserInfoDTO() : base() { }
        #endregion Constr

        [DataMember(Name = "Email", Order = 1)]
        public string Email;

        [DataMember(Name = "FName", Order = 2)]
        public string FirstName;

        [DataMember(Name = "LName", Order = 3)]
        public string LastName;
    }
}
