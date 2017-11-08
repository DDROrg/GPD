using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.ResponseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "users-list-response")]
    public class UsersListResponse
    {
        #region Constr
        public UsersListResponse()
        {
            this.UserList = new List<UserDTO>();
        }
        #endregion Constr

        [DataMember(Name = "users", Order = 1)]
        public List<UserDTO> UserList { get; set; }
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "user")]
    public class UserDTO
    {
        #region Constr
        public UserDTO() : base() { }
        #endregion Constr

        [DataMember(Name = "userId", Order = 1)]
        public int UserId;

        [DataMember(Name = "firstName", Order = 2)]
        public string FirstName;

        [DataMember(Name = "lastName", Order = 3)]
        public string LastName;

        [DataMember(Name = "email", Order = 4)]
        public string Email;

        [DataMember(Name = "company", Order = 5)]
        public string FirmName;

        [DataMember(Name = "isActive", Order = 6)]
        public bool IsActive;

        [DataMember(Name = "createdOn", Order = 7)]
        public string CreatedOn;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "update-user-response")]
    public class UpdateUserResponse
    {
        #region Constr
        public UpdateUserResponse(bool status, int userId)
        {
            this.Status = status;
            this.Message = string.Empty;
            this.UserId = userId;
        }
        #endregion Constr

        [DataMember(Name = "status", Order = 1)]
        public bool Status;

        [DataMember(Name = "message", Order = 2)]
        public string Message;

        [DataMember(Name = "user-id", Order = 3)]
        public int UserId;
    }
}
