using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using System;
using System.Linq;
using System.Net.Mail;

namespace SchoolRegister.Services.Services
{
    public class EmailService : BaseService, IEmailService
    {
        private readonly UserManager<User> _userManager;
        private readonly SmtpClient _smtpClient;

        public EmailService(ApplicationDbContext dbContext, IMapper mapper, UserManager<User> userManager, SmtpClient smtpClient) : base(dbContext, mapper)
        {
            _userManager = userManager;
            _smtpClient = smtpClient;
        }

        public bool SendEmailToParent(SendEmailToParentDTO sendEmailToParentDTO)
        {
            try
            {
                if (sendEmailToParentDTO == null)
                {
                    throw new ArgumentNullException($"DTO is null");
                }
                var teacher = _dbContext.Users.OfType<Teacher>()
                                            .FirstOrDefault(x => x.Id == sendEmailToParentDTO.SenderId);
                if (teacher == null || _userManager.IsInRoleAsync(teacher, "Teacher").Result == false)
                {
                    throw new InvalidOperationException("Sender is not a teacher");
                }

                var student = _dbContext.Users.OfType<Student>()
                                            .FirstOrDefault(x => x.Id == sendEmailToParentDTO.StudentId);
                if (student == null || !_userManager.IsInRoleAsync(student, "Student").Result)
                {
                    throw new InvalidOperationException("Given user is not a student");
                }

                var mailMessage = new MailMessage(to: student.Parent.Email, subject: sendEmailToParentDTO.Title, body: sendEmailToParentDTO.Content, from: teacher.Email);
                _smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
