using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;

namespace SchoolRegister.Web.Extensions.Startup
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<SignInManager<User>>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
