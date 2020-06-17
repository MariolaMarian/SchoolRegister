using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Linq;

namespace SchoolRegister.Services.Services
{
    public class GradeService : BaseService, IGradeService
    {
        private readonly UserManager<User> _userManager;
        public GradeService(IMapper mapper,ApplicationDbContext dbContext, UserManager<User> userManager):base(dbContext,mapper)
        {
            _userManager = userManager;
        }
        public GradeVM AddGradeToStudent(GradeForStudentAddDTO gradeForStudentAddDTO)
        {
            if(gradeForStudentAddDTO == null)
            {
                throw new ArgumentNullException($"DTO is null");
            }
            var teacher = _dbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == gradeForStudentAddDTO.TeacherId);
            var student = _dbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == gradeForStudentAddDTO.StudentId);
            var subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == gradeForStudentAddDTO.SubjectId);
            if(teacher == null)
            {
                throw new ArgumentNullException("Teacher is null");
            }
            if(!_userManager.IsInRoleAsync(teacher,"Teacher").Result)
            {
                throw new InvalidOperationException("Provided user is not a teacher");
            }
            if(student == null)
            {
                throw new ArgumentNullException("Student is null");
            }
            if (!_userManager.IsInRoleAsync(student, "Student").Result)
            {
                throw new InvalidOperationException("Provided user is not a student");
            }
            if(subject == null)
            {
                throw new ArgumentNullException("Subject is null");
            }
            var gradeEntity = _mapper.Map<Grade>(gradeForStudentAddDTO);
            _dbContext.Grades.Add(gradeEntity);
            _dbContext.SaveChanges();
            var gradeVM = _mapper.Map<GradeVM>(gradeEntity);
            return gradeVM;
        }

        public GradesReportVM GetGradesReportForStudent(GetGradesDTO getGradesDTO)
        {
            if(getGradesDTO == null)
            {
                throw new ArgumentNullException("DTO is null");
            }
            var getterUser = _dbContext.Users.FirstOrDefault(x => x.Id == getGradesDTO.GetterUserId);
            if(getterUser == null)
            {
                throw new ArgumentNullException("Getter user is null");
            }
            if(!_userManager.IsInRoleAsync(getterUser,"Teacher").Result &&
               !_userManager.IsInRoleAsync(getterUser, "Student").Result &&
               !_userManager.IsInRoleAsync(getterUser, "Parent").Result &&
               !_userManager.IsInRoleAsync(getterUser, "Admin").Result)
            {
                throw new InvalidOperationException("Getter user does not have permissions to read data");
            }
            var student = _dbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == getGradesDTO.StudentId);
            if(student == null)
            {
                throw new InvalidOperationException($"User is not a student");
            }
            var gradesReport = _mapper.Map<GradesReportVM>(student);
            return gradesReport;
        }

    }
}
