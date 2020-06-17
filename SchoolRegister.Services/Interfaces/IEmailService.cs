using SchoolRegister.ViewModels.DTOs;

namespace SchoolRegister.Services.Interfaces
{
    public interface IEmailService
    {
        bool SendEmailToParent(SendEmailToParentDTO sendEmailToParentDTO);
    }
}
