using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;
using SchoolRegister.Web.Configuration;

namespace SchoolRegister.Web.Extensions.Startup
{
    public static class DIContainerServicesExtension
    {
        public static IServiceCollection ConfigureServicesContainer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<DbContextOptions<ApplicationDbContext>>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ISubjectGroupService, SubjectGroupService>();
            services.AddScoped<IStudentGroupService, StudentGroupService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, Role>>();

            return services;
        }
    }
}
