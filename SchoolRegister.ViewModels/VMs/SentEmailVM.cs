using System.ComponentModel;

namespace SchoolRegister.ViewModels.VMs
{
    public class SentEmailVM
    {
        [DisplayName("Sent by")]
        public string SenderName { get; set; }
        [DisplayName("Sent to parent")]
        public string ParentName { get; set; }

        [DisplayName("of student")]
        public string StudentName { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Content")]
        public string Content { get; set; }
    }
}
