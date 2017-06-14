using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "item-product")]
    public class ItemProductDTO : BaseDTO
    {
        [DataMember(Name = "image-url", Order = 1)]
        public string ImageUrl;

        [DataMember(Name = "manufacturer", Order = 2)]
        public string Manufacturer;

        [DataMember(Name = "model", Order = 3)]
        public string Model;

        [DataMember(Name = "name", Order = 4)]
        public string Name;

        [DataMember(Name = "url", Order = 5)]
        public string URL;
    }
}
