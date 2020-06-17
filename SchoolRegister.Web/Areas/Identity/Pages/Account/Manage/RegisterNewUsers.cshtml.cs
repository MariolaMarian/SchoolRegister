using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.ViewModels.VMs;
using SchoolRegister.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SchoolRegister.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Admin")]
    public class RegisterNewUsersModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterNewUsersModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public RegisterNewUsersModel(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<RegisterNewUsersModel> logger,
             IEmailSender emailSender, ApplicationDbContext dbContext, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public void OnGet()
        {
            ViewData["Roles"] = new SelectList(_dbContext.Roles.Select(t => new
            {
                Text = t.Name,
                Value = t.Id
            }), "Value", "Text", _dbContext.Roles.FirstOrDefault(x => x.RoleValue == RoleValue.Parent).Id);

            ViewData["Parents"] = _mapper.Map<IEnumerable<SelectListItem>>(_mapper.Map<IEnumerable<ParentVM>>(_dbContext.Users.OfType<Parent>()));

            ViewData["Groups"] = _mapper.Map<IEnumerable<SelectListItem>>(_mapper.Map<IEnumerable<GroupVM>>(_dbContext.Groups));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var returnUrl = "~/Identity/Account/Manage/RegisterNewUsers";

            if(ModelState.IsValid)
            {
                var tupleUserRole = CreateUserBasedOnRole(Input);
                var result = await _userManager.CreateAsync(tupleUserRole.Item1, Input.Password);
                if(result.Succeeded)
                {
                    _logger.LogInformation("New user created");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(tupleUserRole.Item1);
                    var callbackUrl = Url.Page("/Account/ConfirmEmail",
                        null,new {userId = tupleUserRole.Item1.Id, code = code},
                        Request.Scheme
                        );

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking this link</a>");

                    await _userManager.AddToRoleAsync(tupleUserRole.Item1, tupleUserRole.Item2.Name);

                    return LocalRedirect(returnUrl);
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private Tuple<User, Role> CreateUserBasedOnRole(RegisterInputModel registerInputModel)
        {
            var role = _dbContext.Roles.FirstOrDefault(r => r.Id == registerInputModel.RoleId);

            if(role == null)
            {
                throw new InvalidOperationException("Role does not exist!");
            }

            switch(role.RoleValue)
            {
                case RoleValue.Student:
                    return new Tuple<User,Role>(new Student
                    {
                        UserName = registerInputModel.Email,
                        Email = registerInputModel.Email,
                        FirstName = registerInputModel.FirstName,
                        LastName = registerInputModel.LastName,
                        GroupId = registerInputModel.GroupId,
                        ParentId = registerInputModel.ParentId,
                        RegistrationDate = DateTime.Now
                    }, role);
                case RoleValue.Parent:
                    return new Tuple<User, Role>(new Parent
                    {
                        UserName = registerInputModel.Email,
                        Email = registerInputModel.Email,
                        FirstName = registerInputModel.FirstName,
                        LastName = registerInputModel.LastName,
                        RegistrationDate = DateTime.Now
                    }, role);
                case RoleValue.Teacher:
                    return new Tuple<User, Role>(new Teacher
                    {
                        UserName = registerInputModel.Email,
                        Email = registerInputModel.Email,
                        FirstName = registerInputModel.FirstName,
                        LastName = registerInputModel.LastName,
                        Title = registerInputModel.TeacherTitles,
                        RegistrationDate = DateTime.Now
                    }, role);
                case RoleValue.Admin:
                    return new Tuple<User, Role>(new User
                    {
                        UserName = registerInputModel.Email,
                        Email = registerInputModel.Email,
                        FirstName = registerInputModel.FirstName,
                        LastName = registerInputModel.LastName,
                        RegistrationDate = DateTime.Now
                    }, role);
                default: return null;
            }
        }
    }
}
