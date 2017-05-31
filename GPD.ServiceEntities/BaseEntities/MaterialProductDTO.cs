using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "material-product")]
    public class MaterialProductDTO
    {
        [DataMember(Name = "manufacturer", Order = 1)]
        public string Manufacturer;

        [DataMember(Name = "model", Order = 2)]
        public string Model;

        [DataMember(Name = "name", Order = 3)]
        public string Name;
    }
}
