using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRChat.Domain.Interfaces.Services.Chat;
using SignalRChat.Persistence.Data;
using SignalRChat.Service.Chat;

namespace SignalRChat.IoC
{
    public class UiInitializer
    {
        public void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSingleton<IChatUsersService, ChatUsersService>();
            services.AddScoped<IChatConfigurationService, ChatConfigurationService>();

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Index");
            });
            services.AddSignalR();
        }

    }
}