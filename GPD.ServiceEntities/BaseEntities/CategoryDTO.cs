using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "category")]
    public class CategoryDTO
    {
        [DataMember(Name = "taxonomy", Order = 1)]
        public string Taxonomy;

        [DataMember(Name = "title", Order = 2)]
        public string Title;
    }
}
