using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.ResponseEntities.ProjectsList
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "projects-list-response")]
    public class ResponseDTO
    {
        #region Constr
        public ResponseDTO() { }
        public ResponseDTO(int pageSize, int pageIndex)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.ProjectList = new List<ProjectDTO>();
        }
        #endregion Constr

        [DataMember(Name = "pageSize", Order = 1)]
        public int PageSize;

        [DataMember(Name = "pageIndex", Order = 2)]
        public int PageIndex;

        [DataMember(Name = "projects", Order = 3)]
        public List<ProjectDTO> ProjectList { get; set; }
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "project")]
    public class ProjectDTO : BaseEntities.BaseDTO
    {
        public ProjectDTO() : base() { }
        public ProjectDTO(string id) : base(id) { }

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

        [DataMember(Name = "create-timestamp-formatted", Order = 14)]
        public string CreateTimestamp;
    }
}
