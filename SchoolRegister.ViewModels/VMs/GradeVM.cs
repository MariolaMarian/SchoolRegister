using SchoolRegister.BLL.Entities;
using System;

namespace SchoolRegister.ViewModels.VMs
{
    public class GradeVM
    {
        public string SubjectName { get; set; }
        public GradeScale GradeValue { get; set; }
        public string StudentName { get; set; }
        public DateTime DateOfIssue { get; set; }
    }
}
