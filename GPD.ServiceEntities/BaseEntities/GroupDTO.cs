using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{


    [DataContract(Namespace = "http://www.gpd.com", Name = "group")]
    public class GroupDTO
    {
        #region Constr
        public GroupDTO() : base() { }
        #endregion Constr

        [DataMember(Name = "groupId", Order = 1)]
        public int GroupId;

        [DataMember(Name = "name", Order = 2)]
        public string Name;

        [DataMember(Name = "description", Order = 3)]
        public string Description;

        [DataMember(Name = "isActive", Order = 4)]
        public bool IsActive;
    }
}
