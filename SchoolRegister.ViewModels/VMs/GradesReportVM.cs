using SchoolRegister.BLL.Entities;
using System.Collections.Generic;
using System.ComponentModel;

namespace SchoolRegister.ViewModels.VMs
{
    public class GradesReportVM
    {
        [DisplayName("Student first name")]
        public string StudentFirstName { get; set; }
        [DisplayName("Student last name")]
        public string StudentLastName { get; set; }
        [DisplayName("Parent")]
        public string ParentName { get; set; }
        [DisplayName("Group")]
        public string GroupName { get; set; }
        [DisplayName("Grades")]
        public IDictionary<string,List<GradeScale>> StudentGradesPerSubject { get; set; }
        [DisplayName("Average grade")]
        public double AverageGrade { get; set; }
        [DisplayName("Average grade per subject")]
        public IDictionary<string, double> AverageGradePerSubject { get; set; }
    }
}
