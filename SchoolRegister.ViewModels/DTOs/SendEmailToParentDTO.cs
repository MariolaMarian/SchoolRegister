using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class SendEmailToParentDTO
    {
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name ="Sender")]
        public int SenderId { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        [Display(Name = "Student")]
        public int StudentId { get; set; }
        [Required(ErrorMessage = "This field is mandatory"), MinLength(5, ErrorMessage = "Length should be at least {1} characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        public string Content { get; set; }
    }
}
