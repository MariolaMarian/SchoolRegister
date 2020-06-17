using System.Collections.Generic;
using System.ComponentModel;

namespace SchoolRegister.ViewModels.VMs
{
    public class SubjectVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TeacherId { get; set; }
        [DisplayName("Teacher")]
        public string TeacherName { get; set; }
        public IList<GroupVM> Groups { get; set; }
    }
}
