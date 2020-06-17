using System.Collections.Generic;

namespace SchoolRegister.ViewModels.VMs
{
    public class TeacherVM : UserVM
    {
        public string Title { get; set; }
        public IList<SubjectVM> Subjects { get; set; }
    }
}
