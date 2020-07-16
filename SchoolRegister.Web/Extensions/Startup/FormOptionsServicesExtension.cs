using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolRegister.Web.Extensions.Startup
{
    public static class FormOptionsServicesExtension
    {
        public static IServiceCollection ConfigureFormOptions(this IServiceCollection services)
        {
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.KeyLengthLimit = int.MaxValue;
            });

            return services;
        }
    }
}
