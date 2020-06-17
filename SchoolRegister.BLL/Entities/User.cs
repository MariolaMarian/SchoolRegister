using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.BLL.Entities
{
    public class User : IdentityUser<int>
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
