
using SchoolRegister.BLL.Entities;

namespace SchoolRegister.ViewModels.VMs
{
    public class ChatMessageVM
    {
        public string RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }

        public void SetAuthor(User user)
        {
            this.AuthorName = $"{user.FirstName} {user.LastName}";
            this.AuthorId = user.Id.ToString();
        }
    }
}
