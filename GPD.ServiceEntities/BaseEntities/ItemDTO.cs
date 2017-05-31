using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "item")]
    public class ItemDTO
    {
        [DataMember(Name = "categories", Order = 1)]
        public List<CategoryDTO> Categories { get; set; }

        [DataMember(Name = "currency", Order = 2)]
        public string Currency;

        [DataMember(Name = "family", Order = 3)]
        public string Family;

        [DataMember(Name = "id", Order = 4)]
        public string Id;

        [DataMember(Name = "materials", Order = 5)]
        public List<MaterialDTO> Materials { get; set; }

        [DataMember(Name = "product", Order = 6)]
        public ItemProductDTO Product { get; set; }

        [DataMember(Name = "quantity", Order = 7)]
        public string Quantity;

        [DataMember(Name = "quantity-unit", Order = 8)]
        public string QuantityUnit;

        [DataMember(Name = "type", Order = 9)]
        public string Type;
    }
}
