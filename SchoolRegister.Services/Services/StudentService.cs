using AutoMapper;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Services
{
    public class StudentService : BaseService, IStudentService
    {
        public StudentService(IMapper mapper, ApplicationDbContext dbContext) : base(dbContext, mapper) { }
        public StudentVM GetStudent(Expression<Func<Student, bool>> filterPredicate)
        {
            if (filterPredicate == null)
            {
                throw new ArgumentNullException($"Filter predicate is null");
            }

            var studentEntity = _dbContext.Users.OfType<Student>().FirstOrDefault(filterPredicate);
            var studentVM = _mapper.Map<StudentVM>(studentEntity);
            return studentVM;
        }

        public IEnumerable<StudentVM> GetStudents(Expression<Func<Student, bool>> filterPredicate = null)
        {
            var studentsEntities = _dbContext.Users.OfType<Student>().AsQueryable();
            if(filterPredicate != null)
            {
                studentsEntities = studentsEntities.Where(filterPredicate);
            }
            var studentsVM = _mapper.Map<IEnumerable<StudentVM>>(studentsEntities);
            return studentsVM;
        }
    }
}
