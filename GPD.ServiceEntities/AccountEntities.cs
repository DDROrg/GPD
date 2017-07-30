﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace GPD.ServiceEntities
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginDTO
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.gpd.com", Name = "userRegistrationStatus")]
    public class UserRegistrationStatusDTO
    {
        [DataMember(Name = "userId", Order = 1)]
        public int UserId;

        [DataMember(Name = "status", Order = 2)]
        public bool Status;

        [DataMember(Name = "message", Order = 3)]
        public string Message;
    }

    /// <summary>
    /// User Registration form object
    /// </summary>
    public class UserRegistrationDTO
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Company")]
        public string Company { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Business Phone")]
        [Phone]
        public string BusinessPhone { get; set; }

        [Display(Name = "Home Phone")]
        [Phone]
        public string HomePhone { get; set; }

        [Display(Name = "Mobile Phone")]
        [Phone]
        public string MobilePhone { get; set; }

        [Display(Name = "Fax")]
        [Phone]
        public string Fax { get; set; }

        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "ZIP")]
        public string Zip { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.gpd.com", Name = "userRegistration2")]
    public class UserRegistration2DTO
    {
        #region Constr
        public UserRegistration2DTO() : base()
        {
            Company = new CompanyDTO();
        }
        #endregion Constr

        [DataMember(Name = "firstName", Order = 1)]
        public string FirstName;

        [DataMember(Name = "lastName", Order = 2)]
        public string LastName;

        [DataMember(Name = "email", Order = 3)]
        public string Email;

        [DataMember(Name = "jobTitle", Order = 4)]
        public string JobTitle;

        [DataMember(Name = "company", Order = 5)]
        public CompanyDTO Company;

        [DataMember(Name = "password", Order = 6)]
        public string Password;

        [DataMember(Name = "confirmPassword", Order = 7)]
        public string ConfirmPassword;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "company")]
    public class CompanyDTO
    {
        #region Constr
        public CompanyDTO() : base()
        {

        }
        #endregion Constr

        [DataMember(Name = "name", Order = 1)]
        public string Name;

        [DataMember(Name = "website", Order = 2)]
        public string WebSite;

        [DataMember(Name = "country", Order = 4)]
        public string Country;

        [DataMember(Name = "address", Order = 5)]
        public string Address;

        [DataMember(Name = "address2", Order = 6)]
        public string Address2;

        [DataMember(Name = "city", Order = 7)]
        public string City;

        [DataMember(Name = "state", Order = 8)]
        public string State;

        [DataMember(Name = "postalCode", Order = 9)]
        public string PostalCode;

        [DataMember(Name = "defaultIndustry", Order = 10)]
        public string DefaultIndustry;

        [DataMember(Name = "phone", Order = 10)]
        public string Phone;
    }
}
