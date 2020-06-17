using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class AttachDetachSubjectWithGroupDTO
    {
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name ="Subject")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name = "Group")]
        public int GroupId { get; set; }
    }
}
