
namespace SchoolRegister.BLL.Entities
{
    public class SubjectGroup
    {
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
