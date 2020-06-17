using System;
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
    [Authorize(Roles = "Admin, Teacher")]
    public class EmailController : BaseController<EmailController>
    {
        private readonly IStudentService _studentService;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public EmailController(IStudentService studentService, IEmailService emailService, UserManager<User> userManager, IMapper mapper,
            IStringLocalizer<EmailController> localizer, ILoggerFactory loggerFactory) : base(localizer, loggerFactory)
        {
            _studentService = studentService;
            _emailService = emailService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var studentsVM = _studentService.GetStudents();
            ViewBag.StudentsSelectList = _mapper.Map<IEnumerable<SelectListItem>>(studentsVM);

            var user = _userManager.GetUserAsync(User).Result;
            return View(new SendEmailToParentDTO() { SenderId = user.Id });
        }

        [HttpPost]
        public IActionResult Index(SendEmailToParentDTO sendEmailToParentDTO)
        {
            if (ModelState.IsValid)
            {
                /*if(_emailService.SendEmailToParent(sendEmailToParentDTO))
                {*/
                    return RedirectToAction("Details", new
                    {
                        SenderId = sendEmailToParentDTO.SenderId,
                        StudentId = sendEmailToParentDTO.StudentId,
                        Title = sendEmailToParentDTO.Title,
                        Content = sendEmailToParentDTO.Content
                    });
                /*
                }*/
            }
            return View("Error");
        }

        public IActionResult Details(SendEmailToParentDTO sendEmailToParentDTO)
        {
            try
            {
                var student = _userManager.FindByIdAsync(sendEmailToParentDTO.StudentId.ToString()).Result as Student;
                var parent = _userManager.FindByIdAsync(student.ParentId.ToString()).Result as Parent;
                var sender = _userManager.FindByIdAsync(sendEmailToParentDTO.SenderId.ToString()).Result;
                var emailToDisplay = new SentEmailVM()
                {
                    SenderName = $"{sender.FirstName} {sender.LastName}",
                    ParentName = $"{parent.FirstName} {parent.LastName}",
                    StudentName = $"{student.FirstName} {student.LastName}",
                    Title = sendEmailToParentDTO.Title,
                    Content = sendEmailToParentDTO.Content
                };
                return View(emailToDisplay);
            }
            catch(Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}