using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.BLL.Entities;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using SchoolRegister.Web.Extensions;

namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles ="Admin, Teacher")]
    public class SubjectController : BaseController<SubjectController>
    {
        private readonly ISubjectService _subjectService;
        private readonly ITeacherService _teacherService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public SubjectController(ISubjectService subjectService, ITeacherService teacherService, UserManager<User> userManager, IMapper mapper,
            IStringLocalizer<SubjectController> localizer, ILoggerFactory loggerFactory) : base(localizer, loggerFactory)
        {
            _subjectService = subjectService;
            _teacherService = teacherService;
            _userManager = userManager;
            _mapper = mapper;
        }
        
        public IActionResult Index(string filterValue = null)
        {
            Expression<Func<Subject, bool>> filterPredicate = null;
            if(!string.IsNullOrWhiteSpace(filterValue))
            {
                filterPredicate = x => x.Name.Contains(filterValue);
            }

            bool isAjax = HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";

            var user = _userManager.GetUserAsync(User).Result;
            IEnumerable<SubjectVM> subjectsVM = null;
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
            {
                subjectsVM = _subjectService.GetSubjects(filterPredicate);
            }
            else if (_userManager.IsInRoleAsync(user, "Teacher").Result)
            {
                var teacher = _userManager.GetUserAsync(User).Result as Teacher;

                Expression<Func<Subject, bool>> filterByTeacher = x => x.TeacherId == teacher.Id;

                var finalFilter = filterPredicate != null ?
                    Expression.Lambda<Func<Subject, bool>>(
                        Expression.AndAlso(filterPredicate.Body,
                        new ExpressionParameterReplacer(filterByTeacher.Parameters, filterPredicate.Parameters)
                        .Visit(filterByTeacher.Body)),
                        filterPredicate.Parameters) : filterByTeacher;

                subjectsVM = _subjectService.GetSubjects(finalFilter);
            }

            if(subjectsVM == null)
            {
                return View("Error");
            }

            if(isAjax)
            {
                return PartialView("_SubjectsTablePartial", subjectsVM);
            }

            return View(subjectsVM);

        }

        public IActionResult Details(int id)
        {
            var subjectVM = _subjectService.GetSubject(x => x.Id == id);
            return View(subjectVM);
        }

        public IActionResult CreateOrUpdateSubject(int? id = null)
        {
            var teachersVM = _teacherService.GetTeachers();
            ViewBag.TeachersSelectList = _mapper.Map<IEnumerable<SelectListItem>>(teachersVM);

            if (id.HasValue)
            {
                var subjectVM = _subjectService.GetSubject(x => x.Id == id);
                ViewBag.ActionType = _localizer["Edit"];
                return View(_mapper.Map<SubjectForCreateOrUpdateDTO>(subjectVM));
            }
            ViewBag.ActionType = _localizer["Add"];
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrUpdateSubject(SubjectForCreateOrUpdateDTO subjectForCreateOrUpdateDTO)
        {
            if(ModelState.IsValid)
            {
                _subjectService.CreateOrUpdate(subjectForCreateOrUpdateDTO);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}