using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.BLL.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual List<Group> Groups { get; set; }
        public virtual IList<Grade> Grades { get; set; }
        public virtual IList<SubjectGroup> SubjectGroups { get; set; }
    }
}
