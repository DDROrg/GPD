using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "material-type")]
    public class MaterialTypeDTO
    {
        [DataMember(Name = "name", Order = 1)]
        public string Name;
    }
}
