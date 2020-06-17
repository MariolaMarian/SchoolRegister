using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class GetGradesDTO
    {
        [Required(ErrorMessage = "This field is mandatory")]
        public int StudentId { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        public int GetterUserId { get; set; }
    }
}
