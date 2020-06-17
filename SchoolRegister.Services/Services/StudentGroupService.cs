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
    public class StudentGroupService : BaseService, IStudentGroupService
    {
        private readonly UserManager<User> _userManager;
        public StudentGroupService(ApplicationDbContext dbContext, IMapper mapper, UserManager<User> userManager) : base(dbContext, mapper) 
        {
            _userManager = userManager;
        }

        public StudentVM AttachStudentWithGroup(AttachDetachStudentWithGroupDTO attachStudentWithGroupDTO)
        {
            if (attachStudentWithGroupDTO == null)
            {
                throw new ArgumentNullException($"DTO is null");
            }
            var student = _dbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == attachStudentWithGroupDTO.StudentId);
            if (student == null || !_userManager.IsInRoleAsync(student, "Student").Result)
            {
                throw new ArgumentNullException($"Student is null or user is not student");
            }
            var group = _dbContext.Groups.FirstOrDefault(x => x.Id == attachStudentWithGroupDTO.GroupId);
            if (group == null)
            {
                throw new ArgumentNullException($"Group is null");
            }
            student.GroupId = group.Id;
            student.Group = group;
            _dbContext.SaveChanges();
            var studentVM = _mapper.Map<StudentVM>(student);
            return studentVM;
        }

        public StudentVM DetachStudentFromGroup(AttachDetachStudentWithGroupDTO detachStudentWithGroupDTO)
        {
            if (detachStudentWithGroupDTO == null)
            {
                throw new ArgumentNullException($"DTO is null");
            }
            var student = _dbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == detachStudentWithGroupDTO.StudentId);
            if (student == null || !_userManager.IsInRoleAsync(student, "Student").Result)
            {
                throw new ArgumentNullException($"Student is null or user is not a student");
            }
            var group = _dbContext.Groups.FirstOrDefault(x => x.Id == detachStudentWithGroupDTO.GroupId);
            if (group == null)
            {
                throw new ArgumentNullException($"Group is null");
            }
            student.GroupId = null;
            student.Group = null;
            group.Students.Remove(student);
            _dbContext.SaveChanges();
            var studentVm = _mapper.Map<StudentVM>(student);
            return studentVm;
        }
    }
}
