using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class AttachDetachSubjectWIthTeacherDTO
    {
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name ="Subject")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }
    }
}
