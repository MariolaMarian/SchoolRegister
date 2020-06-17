using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Services
{
    public class SubjectService : BaseService, ISubjectService 
    {
        public SubjectService(IMapper mapper, ApplicationDbContext dbContext) : base(dbContext, mapper) { }

        public SubjectVM CreateOrUpdate(SubjectForCreateOrUpdateDTO subjectForCreateOrUpdateDTO)
        {
            if(subjectForCreateOrUpdateDTO == null)
            {
                throw new ArgumentNullException($"DTO of type is null");
            }
            var subjectEntity = _mapper.Map<Subject>(subjectForCreateOrUpdateDTO);
            if(subjectForCreateOrUpdateDTO.Id == null || subjectForCreateOrUpdateDTO.Id == 0)
            {
                _dbContext.Subjects.Add(subjectEntity);
            }
            else
            {
                _dbContext.Subjects.Update(subjectEntity);
            }
            _dbContext.SaveChanges();
            var subjectVM = _mapper.Map<SubjectVM>(subjectEntity);
            return subjectVM;
        }

        public SubjectVM GetSubject(Expression<Func<Subject, bool>> filterPredicate)
        {
            if(filterPredicate == null)
            {
                throw new ArgumentNullException($"Predicate is null");
            }
            Subject subjectEntity = _dbContext.Subjects
                                                .Include(s => s.Teacher)
                                                .Include(s => s.SubjectGroups)
                                                    .ThenInclude(sg => sg.Group)
                                                .FirstOrDefault(filterPredicate);
            SubjectVM subjectVM = _mapper.Map<SubjectVM>(subjectEntity);
            return subjectVM;
        }

        public IEnumerable<SubjectVM> GetSubjects(Expression<Func<Subject, bool>> filterPredicate = null)
        {
            var subjectEntities = _dbContext.Subjects
                                                .Include(s => s.Teacher)
                                                .Include(s => s.SubjectGroups)
                                                    .ThenInclude(sg => sg.Group)
                                                .AsQueryable();
            if(filterPredicate != null)
            {
                subjectEntities = subjectEntities.Where(filterPredicate);
            }
            IEnumerable<SubjectVM> subjectsVM = _mapper.Map<IEnumerable<SubjectVM>>(subjectEntities);
            return subjectsVM;
        }

        public bool Remove(int? id)
        {
            if(id == null || id == 0)
            {
                throw new ArgumentException("Invalid id", "id");
            }
            var subjectToRemove = _dbContext.Subjects.FirstOrDefault(s => s.Id == id);
            if(subjectToRemove == null)
            {
                throw new ArgumentException("Invalid id", "id");
            }
            _dbContext.Subjects.Remove(subjectToRemove);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
