using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class SubjectForCreateOrUpdateDTO
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        [StringLength(50, ErrorMessage = "Length cannot be more than {1} characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        [StringLength(100,ErrorMessage = "Length cannot be more than {1} characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        public int TeacherId { get; set; }
    }
}
