using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.BLL.Entities;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VMs;
using SchoolRegister.Web.Extensions;

namespace SchoolRegister.Web.Controllers
{
    public class StudentController : BaseController<StudentController>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStudentService _studentService;
        private readonly IGroupService _groupService;
        private readonly ISubjectService _subjectService;
        public StudentController(UserManager<User> userManager, IMapper mapper, IStudentService studentService,
            IGroupService groupService, ISubjectService subjectService,
            IStringLocalizer<StudentController> stringLocalizer, ILoggerFactory loggerFactory) : base(stringLocalizer, loggerFactory)
        {
            _userManager = userManager;
            _mapper = mapper;
            _studentService = studentService;
            _groupService = groupService;
            _subjectService = subjectService;
        }
        public IActionResult Index(string filterValue = null)
        {
            Expression<Func<Student, bool>> filterPredicate = null;
            if (!string.IsNullOrWhiteSpace(filterValue))
            {
                filterPredicate = x => (x.FirstName + " " + x.LastName).Contains(filterValue);
            }
            var user = _userManager.GetUserAsync(User).Result;
            IEnumerable<StudentVM> studentsVM = null;

            if (_userManager.IsInRoleAsync(user, "Student").Result)
            {
                studentsVM = new List<StudentVM> { _mapper.Map<StudentVM>(user) };
            }
            else if (_userManager.IsInRoleAsync(user, "Parent").Result)
            {
                studentsVM = _studentService.GetStudents(s => s.ParentId == user.Id);
            }
            else if (_userManager.IsInRoleAsync(user, "Teacher").Result)
            {
                var teacher = user as Teacher;

                Expression<Func<Student, bool>> filterByTeacherPredicate =
                    s => s.Group.SubjectGroups.Any(sg => teacher.Subjects.Select(sub => sub.Id).Contains(sg.SubjectId));

                var finalFilter = filterPredicate != null ?
                    Expression.Lambda<Func<Student, bool>>(
                    Expression.AndAlso(filterPredicate.Body,
                    new ExpressionParameterReplacer(filterByTeacherPredicate.Parameters, filterPredicate.Parameters)
                    .Visit(filterByTeacherPredicate.Body)),
                    filterPredicate.Parameters) : filterByTeacherPredicate;

                studentsVM = _studentService.GetStudents(finalFilter);
            }
            else if (_userManager.IsInRoleAsync(user, "Admin").Result)
            {
                studentsVM = _studentService.GetStudents(filterPredicate);
            }

            if (studentsVM == null)
            {
                return View("Error");
            }

            bool isAjax = HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_StudentsTablePartial", studentsVM);
            }
            return View(studentsVM);
        }
    }
}