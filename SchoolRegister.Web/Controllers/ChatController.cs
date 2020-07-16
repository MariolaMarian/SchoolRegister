using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolRegister.Web.Controllers
{
    [Authorize]
    public class ChatController : BaseController<ChatController>
    {
        private IStudentService _studentService;
        private IGroupService _groupService;
        private IMapper _mapper;

        public ChatController(IStringLocalizer<ChatController> localizer, ILoggerFactory loggerFactory, 
            IStudentService studentService, IGroupService groupService, IMapper mapper) : base(localizer, loggerFactory)
        {
            _studentService = studentService;
            _groupService = groupService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var students = _studentService.GetStudents().ToList();
            students.Add(new StudentVM()
            {
                FirstName = "All students",
            });

            var chatGroups = _groupService.GetGroups();
            var studentsListItems = _mapper.Map<IEnumerable<SelectListItem>>(students);
            var chatGroupListItems = _mapper.Map<IEnumerable<SelectListItem>>(chatGroups);

            return View(new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>(studentsListItems, chatGroupListItems));
        }
    }
}
