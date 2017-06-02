using System.Runtime.Serialization;

namespace GPD.ServiceEntities.ResponseEntities.AddProject
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "add-project-response")]
    public class AddProjectResponse
    {
        #region Constr
        public AddProjectResponse() : this(false, "-1") { }
        public AddProjectResponse(bool status, string projectId)
        {
            this.Status = status;
            this.Message = string.Empty;
            this.ProjectId = projectId;
        }
        #endregion Constr

        [DataMember(Name = "status", Order = 1)]
        public bool Status;

        [DataMember(Name = "message", Order = 2)]
        public string Message;

        [DataMember(Name = "project-id", Order = 3)]
        public string ProjectId;
    }
}
