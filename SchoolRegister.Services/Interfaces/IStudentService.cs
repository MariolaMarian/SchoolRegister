using SchoolRegister.BLL.Entities;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        IEnumerable<StudentVM> GetStudents(Expression<Func<Student, bool>> filterPredicate = null);
        StudentVM GetStudent(Expression<Func<Student, bool>> filterPredicate);
    }
}
