using System.Collections.Generic;

namespace SchoolRegister.ViewModels.VMs
{
    public class GroupVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IList<StudentVM> Students { get; set; }
        public virtual IList<SubjectVM> Subjects { get; set; }
        public GroupVM()
        {
            Students = new List<StudentVM>();
            Subjects = new List<SubjectVM>();
        }
    }
}
