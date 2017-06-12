using System.ComponentModel.DataAnnotations;

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
    public class UserRegistrationStatusDTO
    {
        public int UserId { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
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
}
