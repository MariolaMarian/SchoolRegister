using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.BLL.Entities
{
    public class Group
    {
        public int Id { get; set; }
        [Required]
        [StringLength(5)]
        public string Name { get; set; }
        public virtual IList<SubjectGroup> SubjectGroups { get; set; }
        public virtual IList<Student> Students { get; set; }
    }
}
