using System.Collections.Generic;
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

namespace SchoolRegister.Web.Controllers
{
    [Authorize]
    public class GradeController : BaseController<GradeController>
    {
        private readonly IMapper _mapper;
        private readonly IGradeService _gradeService;
        private readonly ISubjectService _subjectService;
        private readonly IStudentService _studentService;
        private readonly UserManager<User> _userManager;

        public GradeController(IMapper mapper, IGradeService gradeService, ISubjectService subjectService, IStudentService studentService,
            UserManager<User> userManager, IStringLocalizer<GradeController> localizer, ILoggerFactory loggerFactory) : base(localizer, loggerFactory)
        {
            _mapper = mapper;
            _gradeService = gradeService;
            _subjectService = subjectService;
            _studentService = studentService;
            _userManager = userManager;
        }

        public IActionResult Index(int? id = null)
        {
            if(!id.HasValue)
            {
                return View("Error");
            }
            var user = _userManager.GetUserAsync(User).Result;
            return View(_gradeService.GetGradesReportForStudent(new GetGradesDTO { GetterUserId = user.Id, StudentId = id.Value }));
        }

        [Authorize(Roles ="Teacher")]
        public IActionResult Create(int? id = null)
        {
            if (!id.HasValue)
            {
                return View("Error");
            }

            var teacher = _userManager.GetUserAsync(User).Result as Teacher;
            var gradeForCreate = new GradeForStudentAddDTO() { TeacherId = teacher.Id };

            var subjectsVM = _mapper.Map<IEnumerable<SubjectVM>>(teacher.Subjects);
            ViewBag.SubjectsSelectList = _mapper.Map<IEnumerable<SelectListItem>>(subjectsVM);

            var studentVM = _studentService.GetStudent(x => x.Id == id.Value);
            ViewBag.Student = studentVM;

            return View(gradeForCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public IActionResult Create(GradeForStudentAddDTO gradeForStudentAddDTO)
        {
            if (ModelState.IsValid)
            {
                _gradeService.AddGradeToStudent(gradeForStudentAddDTO);
                return RedirectToAction("Index", new { id = gradeForStudentAddDTO.StudentId });
            }
            return View();
        }
    }
}