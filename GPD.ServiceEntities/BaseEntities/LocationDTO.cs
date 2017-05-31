using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "location")]
    public class LocationDTO
    {
        [DataMember(Name = "address1", Order = 1)]
        public string Address1;

        [DataMember(Name = "city", Order = 2)]
        public string City;

        [DataMember(Name = "state", Order = 3)]
        public string State;

        [DataMember(Name = "zip", Order = 4)]
        public string Zip;
    }
}
