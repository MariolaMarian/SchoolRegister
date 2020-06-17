using SchoolRegister.BLL.Entities;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        GroupVM GetGroup(Expression<Func<Group, bool>> filterPredicate);
        IEnumerable<GroupVM> GetGroups(Expression<Func<Group, bool>> filterPredicate = null);
        GroupVM CreateOrUpdateGroup(GroupForCreateOrUpdateDTO groupForCreateOrUpdateDTO);
    }
}
