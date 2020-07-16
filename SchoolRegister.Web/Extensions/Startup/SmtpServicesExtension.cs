using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace SchoolRegister.Web.Extensions.Startup
{
    public static class SmtpServicesExtension
    {
        public static IServiceCollection ConfigureSmtp(this IServiceCollection services)
        {

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

            return services;
        }
    }
}
