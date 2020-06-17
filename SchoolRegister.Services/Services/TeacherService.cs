using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly UserManager<User> _userManager;
        private readonly IGroupService _groupService;
        public TeacherService(IMapper mapper, ApplicationDbContext dbContext, UserManager<User> userManager, IGroupService groupService):base(dbContext, mapper)
        {
            _userManager = userManager;
            _groupService = groupService;
        }
        public TeacherVM GetTeacher(Expression<Func<Teacher, bool>> filterPredicate)
        {
            var teacherEntity = _dbContext.Users.OfType<Teacher>().FirstOrDefault();
            if(teacherEntity == null)
            {
                throw new InvalidOperationException("Teacher not found");
            }
            var teacherVM = _mapper.Map<TeacherVM>(teacherEntity);
            return teacherVM;
        }

        public IEnumerable<GroupVM> GetTeacherGroups(GetTeacherGroupsDTO getTeacherGroupsDTO)
        {
            if(getTeacherGroupsDTO == null)
            {
                throw new ArgumentNullException(@"DTO is null");
            }
            var teacher = _userManager.Users.OfType<Teacher>().FirstOrDefault(x => x.Id == getTeacherGroupsDTO.TeacherId);
            var teacherGroups = _groupService.GetGroups(g => teacher.Subjects.SelectMany(s => s.SubjectGroups.Select(gr => gr.Group)).Any(x => x.Id == g.Id));
            return teacherGroups;
        }

        public IEnumerable<TeacherVM> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null)
        {
            var teachersEntities = _dbContext.Users.OfType<Teacher>().AsQueryable();
            if(filterPredicate != null)
            {
                teachersEntities = teachersEntities.Where(filterPredicate);
            }
            var teachersVMs = _mapper.Map<IEnumerable<TeacherVM>>(teachersEntities);
            return teachersVMs;
        }

    }
}
