using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "signInResponse")]
    public class SignInResponseDTO
    {
        #region Constr
        public SignInResponseDTO() : base()
        {
            Roles = new List<UserRoleDTO>();
            PartnerNames = new List<string>();
        }
        #endregion Constr

        [DataMember(Name = "userId", Order = 1)]
        public string UserId;

        [DataMember(Name = "firstName", Order = 2)]
        public string FirstName;

        [DataMember(Name = "lastName", Order = 3)]
        public string LastName;

        [DataMember(Name = "email", Order = 4)]
        public string Email;

        [DataMember(Name = "roles", Order = 5)]
        public List<UserRoleDTO> Roles;

        [DataMember(Name = "partnerNames", Order = 6)]
        public List<string> PartnerNames;

        [DataMember(Name = "selectedPartner", Order = 7)]
        public string SelectedPartner;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "userRole")]
    public class UserRoleDTO
    {

        [DataMember(Name = "userId", Order = 1)]
        public int UserId;

        [DataMember(Name = "partnerId", Order = 2)]
        public string PartnerId;

        [DataMember(Name = "partnerNames", Order = 3)]
        public string PartnerName;

        [DataMember(Name = "partnerImageUrl", Order = 4)]
        public string PartnerImageUrl;

        [DataMember(Name = "groupId", Order = 5)]
        public int GroupId;

        [DataMember(Name = "groupName", Order = 6)]
        public string GroupName;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "reset-user-password-response")]
    public class ResetUserPasswordResponse
    {
        #region Constr
        public ResetUserPasswordResponse()
        {
            this.Status = false;
            this.Message = string.Empty;
        }
        #endregion Constr

        [DataMember(Name = "status", Order = 1)]
        public bool Status;

        [DataMember(Name = "message", Order = 2)]
        public string Message;
    }
}
