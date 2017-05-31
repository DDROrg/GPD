using System.Runtime.Serialization;
using System.Collections.Generic;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "project")]
    public class ProjectDTO
    {
        #region Constr
        public ProjectDTO() : base() { }
        #endregion Constr

        [DataMember(Name = "author", Order = 2)]
        public string Author;

        [DataMember(Name = "building-name", Order = 3)]
        public string BuildingName;

        [DataMember(Name = "client", Order = 4)]
        public string Client;

        [DataMember(Name = "filename", Order = 5)]
        public string Filename;

        [DataMember(Name = "identifiers", Order = 6)]
        public List<IdentifierDTO> Identifiers { get; set; }

        [DataMember(Name = "items", Order = 7)]
        public List<ItemDTO> Items { get; set; }

        [DataMember(Name = "location", Order = 8)]
        public LocationDTO Location { get; set; }

        [DataMember(Name = "name", Order = 9)]
        public string Name;

        [DataMember(Name = "number", Order = 10)]
        public string Number;

        [DataMember(Name = "organization-description", Order = 11)]
        public string OrganizationDescription;

        [DataMember(Name = "organization-name", Order = 12)]
        public string OrganizationName;

        [DataMember(Name = "session", Order = 13)]
        public SessionDTO Session;

        [DataMember(Name = "status", Order = 14)]
        public string Status;
    }
}