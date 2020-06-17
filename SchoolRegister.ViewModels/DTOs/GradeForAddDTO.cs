using SchoolRegister.BLL.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class GradeForAddDTO
    {
        public DateTime? DateOfIssue { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        public GradeScale GradeValue { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        public int SubjectId { get; set; }
    }
}
