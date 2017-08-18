using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "location")]
    public class LocationDTO
    {
        [DataMember(Name = "country", Order = 0)]
        public string Country;

        [DataMember(Name = "address1", Order = 1)]
        public string AddressLine1;

        [DataMember(Name = "address2", Order = 2)]
        public string AddressLine2;

        [DataMember(Name = "city", Order = 3)]
        public string City;

        [DataMember(Name = "state", Order = 4)]
        public string State;

        [DataMember(Name = "zip", Order = 5)]
        public string PostalCode;
    }
}
