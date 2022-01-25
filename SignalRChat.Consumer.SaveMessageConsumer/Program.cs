using Microsoft.EntityFrameworkCore;
using SignalRChat.Consumer.SaveMessageConsumer;
using SignalRChat.Domain.Interfaces.Persistence.Repository;
using SignalRChat.Domain.Interfaces.Persistence.UoW;
using SignalRChat.Persistence.Data;
using SignalRChat.Persistence.Repository;
using SignalRChat.Persistence.UoW;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHostedService<InsertMessageConsumer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();
