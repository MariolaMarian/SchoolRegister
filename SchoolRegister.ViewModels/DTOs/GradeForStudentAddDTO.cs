using SchoolRegister.BLL.Entities;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class GradeForStudentAddDTO
    {
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name ="Student")]
        public int StudentId { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name ="Subject")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        public GradeScale GradeValue { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name ="Teacher")]
        public int TeacherId { get; set; }
    }
}
