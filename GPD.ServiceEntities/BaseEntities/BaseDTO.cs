using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "project")]
    public class BaseDTO
    {
        public BaseDTO() { }

        public BaseDTO(string id)
        {
            this.Id = id;
        }

        [DataMember(Name = "id", Order = 1)]
        public string Id;
    }
}