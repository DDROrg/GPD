using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.ResponseEntities.ProjectsList
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "projects-list-response")]
    public class ProjectsListResponse
    {
        #region Constr
        public ProjectsListResponse()
        {
            this.ProjectList = new List<ProjectItem>();
        }
        #endregion Constr

        [DataMember(Name = "pageSize", Order = 1)]
        public int PageSize;

        [DataMember(Name = "pageIndex", Order = 2)]
        public int PageIndex;

        [DataMember(Name = "totalRecordCount", Order = 3)]
        public int TotalRecordCount;

        [DataMember(Name = "projects", Order = 4)]
        public List<ProjectItem> ProjectList { get; set; }
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "project")]
    public class ProjectItem : BaseEntities.BaseDTO
    {
        public ProjectItem() : base()
        {
            this.Location = new LocationItem();
        }

        public ProjectItem(string id) : base(id) {
            this.Location = new LocationItem();
        }

        [DataMember(Name = "author", Order = 2)]
        public string Author;

        [DataMember(Name = "building-name", Order = 3)]
        public string BuildingName;

        [DataMember(Name = "client", Order = 4)]
        public string Client;

        [DataMember(Name = "filename", Order = 5)]
        public string Filename;

        [DataMember(Name = "name", Order = 9)]
        public string Name;

        [DataMember(Name = "number", Order = 10)]
        public string Number;

        [DataMember(Name = "organization-description", Order = 11)]
        public string OrganizationDescription;

        [DataMember(Name = "organization-name", Order = 12)]
        public string OrganizationName;

        [DataMember(Name = "status", Order = 13)]
        public string Status;

        [DataMember(Name = "delete-status", Order = 14)]
        public string DeleteStatus;

        [DataMember(Name = "create-timestamp-formatted", Order = 15)]
        public string CreateTimestamp;

        [DataMember(Name = "partner-name", Order = 16)]
        public string PartnerName;

        [DataMember(Name = "user-email", Order = 17)]
        public string UserEmail;

        [DataMember(Name = "location", Order = 18)]
        public LocationItem Location;       
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "location")]
    public class LocationItem
    {
        public LocationItem() { }

        [DataMember(Name = "address1", Order = 1)]
        public string Address1;

        [DataMember(Name = "city", Order = 2)]
        public string City;

        [DataMember(Name = "state", Order = 3)]
        public string State;

        [DataMember(Name = "zip", Order = 4)]
        public string ZipCode;
    }
}
