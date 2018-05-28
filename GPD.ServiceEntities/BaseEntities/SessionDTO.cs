using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "session")]
    public class SessionDTO
    {
        [DataMember(Name = "type", Order = 1)]
        public string Type;

        [DataMember(Name = "application", Order = 2)]
        public ApplicationDTO Application;

        [DataMember(Name = "platform", Order = 3)]
        public string Platform;

        [DataMember(Name = "session-user-info", Order = 4)]
        public SessionUserInfoDTO UserInfo;
    }
}
