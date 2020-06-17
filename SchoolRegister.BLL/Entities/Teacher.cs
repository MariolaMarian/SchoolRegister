using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.BLL.Entities
{
    public class Teacher : User
    {
        [StringLength(50)]
        public string Title { get; set; }
        public virtual IList<Subject> Subjects { get; set; }
    }
}
