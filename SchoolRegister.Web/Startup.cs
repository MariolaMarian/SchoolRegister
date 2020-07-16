using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.DAL.EF;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Reflection;
using SchoolRegister.Web.Hubs;
using SchoolRegister.Web.Extensions.Startup;

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

            services.ConfigureCookies();

            services.ConfigureLocalization();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
            });

            var connStr = new ConnectionStringDTO() { ConnectionString = _connectionString };
            services.AddSingleton(connStr);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_connectionString);
            });

            services.ConfigureIdentity();

            services.ConfigureServicesContainer();

            services.AddSignalR();

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            services.ConfigureFormOptions();

            services.ConfigureSmtp();

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
                endpoints.MapHub<ChatHub>("/chathub");
            });

        }
    }
}
