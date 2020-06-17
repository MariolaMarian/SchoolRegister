using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;

namespace SchoolRegister.Services.Interfaces
{
    public interface ISubjectGroupService
    {
        GroupVM AttachSubjectWithGroup(AttachDetachSubjectWithGroupDTO attachSubjectWithGroupDTO);
        GroupVM DetachSubjectFromGroup(AttachDetachSubjectWithGroupDTO detachSubjectWithGroupDTO);
    }
}
