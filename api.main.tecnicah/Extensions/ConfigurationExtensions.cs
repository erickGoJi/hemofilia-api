using api.main.tecnicah.ActionFilter;
using biz.main.tecnicah.Repository.User;
using biz.main.tecnicah.Services.Email;
using biz.main.tecnicah.Services.Logger;
using dal.main.tecnicah.Repository.User;
using dal.main.tecnicah.Services.Email;
using dal.main.tecnicah.Services.Logger;

namespace api.rebel_wings.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("http://localhost")
                .WithOrigins("http://localhost:4200")
                .WithOrigins("http://localhost:8100")
                .WithOrigins("http://demo-minimalist.com")
                .WithOrigins("http://34.237.214.147")
                .WithOrigins("https://my.premierds.com/")
                .WithOrigins("Ionic://localhost")
                .WithOrigins("capacitor://localhost")
                .WithOrigins("http://localhost:63410")
                .AllowCredentials();
            }));
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
        }
    }
}
