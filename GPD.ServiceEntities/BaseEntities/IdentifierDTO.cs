using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "identifier")]
    public class IdentifierDTO
    {
        [DataMember(Name = "identifier", Order = 1)]
        public string Identifier;

        [DataMember(Name = "system", Order = 2)]
        public string SystemName;
    }
}
