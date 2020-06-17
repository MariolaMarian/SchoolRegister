using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SubjectGroupController : BaseController<SubjectGroupController>
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly ISubjectService _subjectService;
        private readonly ISubjectGroupService _subjectGroupService;

        public SubjectGroupController(IMapper mapper, IGroupService groupService, ISubjectService subjectService, ISubjectGroupService subjectGroupService,
            IStringLocalizer<SubjectGroupController> localizer, ILoggerFactory loggerFactory) : base(localizer, loggerFactory)
        {
            _mapper = mapper;
            _groupService = groupService;
            _subjectService = subjectService;
            _subjectGroupService = subjectGroupService;
        }
        public IActionResult AttachSubjectToGroup(int id)
        {
            var groupVM = _groupService.GetGroup(g => g.Id == id);
            if (groupVM == null)
            {
                return View("Error");
            }
            ViewBag.GroupName = groupVM.Name;

            var subjectsVM = _subjectService.GetSubjects().Where(s => groupVM.Subjects.All(x => x.Id != s.Id));
            ViewBag.SubjectsSelectList = _mapper.Map<IEnumerable<SelectListItem>>(subjectsVM);

            return View(new AttachDetachSubjectWithGroupDTO { GroupId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AttachSubjectToGroup(AttachDetachSubjectWithGroupDTO attachDetachSubjectWithGroupDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var groupVM = _subjectGroupService.AttachSubjectWithGroup(attachDetachSubjectWithGroupDTO);
                    if (groupVM != null)
                    {
                        return RedirectToAction("Details", "Group" , new { id = attachDetachSubjectWithGroupDTO.GroupId });
                    }
                }
                catch (Exception)
                {
                    return View("Error");
                }

            }

            return View();
        }

        public IActionResult DetachSubjectFromGroup(int id)
        {
            var groupVM = _groupService.GetGroup(g => g.Id == id);
            if (groupVM == null)
            {
                return View("Error");
            }
            ViewBag.GroupName = groupVM.Name;

            var subjectsVM = groupVM.Subjects;
            ViewBag.SubjectsSelectList = _mapper.Map<IEnumerable<SelectListItem>>(subjectsVM);

            return View(new AttachDetachSubjectWithGroupDTO { GroupId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetachSubjectFromGroup(AttachDetachSubjectWithGroupDTO attachDetachSubjectWithGroupDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var groupVM = _subjectGroupService.DetachSubjectFromGroup(attachDetachSubjectWithGroupDTO);
                    if (groupVM != null)
                    {
                        return RedirectToAction("Details", "Group", new { id = attachDetachSubjectWithGroupDTO.GroupId });
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