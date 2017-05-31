using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "material")]
    public class MaterialDTO : BaseDTO
    {
        [DataMember(Name = "product", Order = 2)]
        public MaterialProductDTO Product { get; set; }

        [DataMember(Name = "type", Order = 3)]
        public MaterialTypeDTO Type { get; set; }
    }
}
