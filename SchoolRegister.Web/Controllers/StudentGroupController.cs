using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;

namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class StudentGroupController : BaseController<StudentGroupController>
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;
        private readonly IStudentGroupService _studentGroupService;

        public StudentGroupController(IMapper mapper, IGroupService groupService, IStudentService studentService, IStudentGroupService studentGroupService,
            IStringLocalizer<StudentGroupController> localizer, ILoggerFactory loggerFactory) : base(localizer, loggerFactory)
        {
            _mapper = mapper;
            _groupService = groupService;
            _studentService = studentService;
            _studentGroupService = studentGroupService;
        }

        public IActionResult AttachStudentToGroup(int id)
        {
            var studentsVM = _studentService.GetStudents(s => s.GroupId  == null || s.GroupId == 0);
            ViewBag.StudentsSelectList = _mapper.Map<IEnumerable<SelectListItem>>(studentsVM);

            var groupVM = _groupService.GetGroup(g => g.Id == id);
            if (groupVM == null)
            {
                return View("Error");
            }
            ViewBag.GroupName = groupVM.Name;

            return View(new AttachDetachStudentWithGroupDTO() { GroupId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AttachStudentToGroup(AttachDetachStudentWithGroupDTO attachDetachStudentWithGroupDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var studentVM = _studentGroupService.AttachStudentWithGroup(attachDetachStudentWithGroupDTO);
                    if (studentVM != null)
                    {
                        return RedirectToAction("Details", "Group", new { id = attachDetachStudentWithGroupDTO.GroupId });
                    }
                }
                catch (Exception)
                {
                    return View("Error");
                }

            }

            return View();
        }

        public IActionResult DetachStudentFromGroup(int id)
        {
            var groupVM = _groupService.GetGroup(g => g.Id == id);
            if (groupVM == null)
            {
                return View("Error");
            }
            ViewBag.GroupName = groupVM.Name;

            var studentsVM = groupVM.Students;
            ViewBag.StudentsSelectList = _mapper.Map<IEnumerable<SelectListItem>>(studentsVM);


            return View(new AttachDetachStudentWithGroupDTO() { GroupId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetachStudentFromGroup(AttachDetachStudentWithGroupDTO attachDetachStudentWithGroupDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var studentVM = _studentGroupService.DetachStudentFromGroup(attachDetachStudentWithGroupDTO);
                    if (studentVM != null)
                    {
                        return RedirectToAction("Details", "Group", new { id = attachDetachStudentWithGroupDTO.GroupId });
                    }
                }
                catch (Exception)
                {
                    return View("Error");
                }

            }

            return View();
        }
    }
}