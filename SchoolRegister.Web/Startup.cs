using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using SchoolRegister.BLL.Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.DAL.EF;
using System.Net.Mail;
using System.Net;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;
using AutoMapper;
using SchoolRegister.Web.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SchoolRegister.Web
{
    public class Startup
    {
        private readonly string _connectionString;
        public IHostEnvironment HostingEnvironment { get; set; }
        public IServiceCollection Services { get; private set; }
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(env.ContentRootPath)
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                                .AddEnvironmentVariables();

            Configuration = builder.Build();

            if(env.IsEnvironment("Development"))
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
                builder.AddEnvironmentVariables();
                Configuration = builder.Build();
            }

            _connectionString = Configuration.GetConnectionString("DefaultConnection");
            HostingEnvironment = env;
            Configuration = configuration;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddApplicationInsightsTelemetry(Configuration);
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("pl-PL")
                };
                options.DefaultRequestCulture = new RequestCulture("en", "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
            });
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_connectionString);
            });
            services.AddIdentity<User, Role>()
                    .AddRoles<Role>()
                    .AddRoleManager<RoleManager<Role>>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddSignInManager<SignInManager<User>>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, Role>>();
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.KeyLengthLimit = int.MaxValue;
            });

            services.AddScoped((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                return new SmtpClient()
                {
                    Host = config.GetValue<string>("Email:Smtp:Host"),
                    Port = config.GetValue<int>("Email:Smtp:Port"),
                    Credentials = new NetworkCredential(
                        config.GetValue<string>("Email:Smtp:Username"),
                        config.GetValue<string>("Email:Smtp:Password")
                        )
                };
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddViewLocalization(s=>s.ResourcesPath = "Resources")
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(SchoolRegister.ViewModels.DTOs.SendEmailToParentDTO).GetTypeInfo().Assembly.FullName);
                        return factory.Create("Translations", assemblyName.Name);
                    };
                });

            var connStr = new ConnectionStringDTO() { ConnectionString = _connectionString };
            services.AddSingleton(connStr);
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
            Services = services;
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            loggerFactory.AddFile("Log/SchoolRegister-{Date}.txt");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
