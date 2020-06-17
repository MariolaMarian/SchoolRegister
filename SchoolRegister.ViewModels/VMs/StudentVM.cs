using SchoolRegister.BLL.Entities;
using System.Collections.Generic;

namespace SchoolRegister.ViewModels.VMs
{
    public class StudentVM : UserVM
    {
        public string ParentName { get; set; }
        public int ParentId { get; set; }
        public string GroupName { get; set; }
        public double AverageGrade { get; set; }
        public string UserName { get; set; }
        public IDictionary<string, double> AverageGradePerSubject { get; set; }
        public IDictionary<string, List<GradeScale>> GradesPerSubject { get; set; }
    }
}
