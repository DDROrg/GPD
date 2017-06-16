using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{


    [DataContract(Namespace = "http://www.gpd.com", Name = "user")]
    public class UserDTO
    {
        #region Constr
        public UserDTO() : base() { }
        #endregion Constr

        [DataMember(Name = "userId", Order = 1)]
        public string UserId;

        [DataMember(Name = "firstName", Order = 2)]
        public string FirstName;

        [DataMember(Name = "lastName", Order = 3)]
        public string LastName;

        [DataMember(Name = "email", Order = 4)]
        public string Email;

        [DataMember(Name = "company", Order = 5)]
        public string Company;

        [DataMember(Name = "jobTitle", Order = 6)]
        public string JobTitle;

        [DataMember(Name = "businessPhone", Order = 7)]
        public string BusinessPhone;

        [DataMember(Name = "homePhone", Order = 8)]
        public string HomePhone;

        [DataMember(Name = "mobilePhone", Order = 9)]
        public string MobilePhone;

        [DataMember(Name = "fax", Order = 10)]
        public string FAX;

        [DataMember(Name = "addressLine1", Order = 11)]
        public string AddressLine1;

        [DataMember(Name = "addressLine2", Order = 12)]
        public string AddressLine2;

        [DataMember(Name = "city", Order = 13)]
        public string City;

        [DataMember(Name = "state", Order = 14)]
        public string State;

        [DataMember(Name = "zip", Order = 15)]
        public string ZIP;

        [DataMember(Name = "country", Order = 16)]
        public string Country;

        [DataMember(Name = "isActive", Order = 17)]
        public bool IsActive;
        
    }
}
