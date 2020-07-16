using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Web.Models
{
    public class RegisterInputModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long!", MinimumLength =6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirm password must be the same!")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long!", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name ="First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long!", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name ="Role")]
        public int RoleId { get; set; }

        [Display(Name ="Parent")]
        public int? ParentId { get; set; }

        [Display(Name ="Teacher titles")]
        [StringLength(50)]
        public string TeacherTitles { get; set; }

        [Display(Name ="Group")]
        public int? GroupId { get; set; }
    }
}
