using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentGroupService
    {
        StudentVM AttachStudentWithGroup(AttachDetachStudentWithGroupDTO attachStudentWithGroupDTO);
        StudentVM DetachStudentFromGroup(AttachDetachStudentWithGroupDTO detachStudentWithGroupDTO);
    }
}
