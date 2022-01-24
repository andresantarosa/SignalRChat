using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRChat.Domain.Interfaces.Persistence.Repository;
using SignalRChat.Domain.Interfaces.Persistence.UoW;
using SignalRChat.Domain.Interfaces.Services.Chat;
using SignalRChat.Persistence.Data;
using SignalRChat.Persistence.Repository;
using SignalRChat.Persistence.UoW;
using SignalRChat.Service.Chat;

namespace SignalRChat.IoC
{
    public class UiInitializer
    {
        private readonly IServiceCollection _services;

        public UiInitializer(IServiceCollection serviceDescriptors)
        {
            _services = serviceDescriptors;
        }

        public UiInitializer Initialize(IConfiguration configuration)
        {
            _services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            _services.AddDatabaseDeveloperPageExceptionFilter();

            _services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            _services.AddSingleton<IChatUsersService, ChatUsersService>();
            _services.AddScoped<IChatService, ChatService>();

            _services.AddScoped<IUnitOfWork, UnitOfWork>();
            _services.AddScoped<IPostRepository, PostRepository>();
            _services.AddSingleton<IChatConfigurationService, ChatConfigurationService>();

            _services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Index");
            }).AddNewtonsoftJson();
            _services.AddSignalR();

            _services.AddHttpClient("StockGateway", client =>
            {
                client.BaseAddress = new Uri(configuration["Urls:StockGateway"]);
            });

            return this;
        }

        public UiInitializer StartConsumer<T>() where T: BackgroundService
        {
            _services.AddHostedService<T>();
            return this;
        }

    }
}