using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class AttachDetachStudentWithGroupDTO
    {
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name ="Student")]
        public int StudentId { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name ="Group")]
        public int GroupId { get; set; }
    }
}
