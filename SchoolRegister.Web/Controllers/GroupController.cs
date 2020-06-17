using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;

namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class GroupController : BaseController<GroupController>
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;

        public GroupController(IMapper mapper, IGroupService groupService, IStudentService studentService, ISubjectService subjectService,
            IStringLocalizer<GroupController> localizer, ILoggerFactory loggerFactory) : base(localizer, loggerFactory)
        {
            _mapper = mapper;
            _groupService = groupService;
            _studentService = studentService;
            _subjectService = subjectService;
        }

        [Authorize(Roles = "Admin,Teacher,Parent,Student")]
        public IActionResult Index()
        {
            var groupsVM = _groupService.GetGroups();
            return View(groupsVM);
        }

        public IActionResult Details(int id)
        {
            GroupVM groupVM = _groupService.GetGroup(g => g.Id == id);
            if(groupVM != null)
            {
                return View(groupVM);
            }
            return View("Error");
        }

        public IActionResult CreateOrUpdate(int? id = null)
        {
            if(id.HasValue)
            {
                var groupVM = _groupService.GetGroup(g => g.Id == id.Value);
                ViewBag.ActionType = "Edit";
                var groupDTO = _mapper.Map<GroupForCreateOrUpdateDTO>(groupVM);
                return View(groupDTO);
            }
            ViewBag.ActionType = "Add";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrUpdate(GroupForCreateOrUpdateDTO groupForCreateOrUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                _groupService.CreateOrUpdateGroup(groupForCreateOrUpdateDTO);
                return RedirectToAction("Index");
            }
            return View();
        }
        
    }
}