using SchoolRegister.BLL.Entities;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Interfaces
{
    public interface ISubjectService
    {
        SubjectVM CreateOrUpdate(SubjectForCreateOrUpdateDTO subjectForCreateOrUpdateDTO);
        bool Remove(int? id);
        SubjectVM GetSubject(Expression<Func<Subject, bool>> filterPredicate);
        IEnumerable<SubjectVM> GetSubjects(Expression<Func<Subject, bool>> filterPredicate = null);
    }
}
