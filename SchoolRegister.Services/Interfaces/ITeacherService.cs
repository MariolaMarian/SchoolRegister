using SchoolRegister.BLL.Entities;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        IEnumerable<TeacherVM> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null);
        TeacherVM GetTeacher(Expression<Func<Teacher, bool>> filterPredicate);
        IEnumerable<GroupVM> GetTeacherGroups(GetTeacherGroupsDTO getTeacherGroupsDTO);

    }
}
