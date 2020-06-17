using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class GroupForCreateOrUpdateDTO
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        [StringLength(5, ErrorMessage = "Length cannot be more than {1} characters")]
        public string Name { get; set; }
    }
}
