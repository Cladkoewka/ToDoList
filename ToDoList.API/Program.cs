using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ToDoList.Application.Services.Implementations;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Application.Services.Mapping;
using ToDoList.Application.Validation.User;
using ToDoList.Domain.Interfaces;
using ToDoList.Infrastructure.DbContext;
using ToDoList.Infrastructure.Repositories;



var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console());



var services = builder.Services;
var configuration = builder.Configuration;


// DbContext configuration
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

const string policyName = "AllowTestOrigin";
services.AddCors(options =>
{
    options.AddPolicy(policyName, builder =>
    {
        builder.WithOrigins("http://localhost:5500", "http://127.0.0.1:5500")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


// Add repositories
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<ITagRepository, TagRepository>();
services.AddScoped<ITaskRepository, TaskRepository>();

// Add mappers
services.AddScoped<IUserMapper, UserMapper>();
services.AddScoped<ITagMapper, TagMapper>();
services.AddScoped<ITaskMapper, TaskMapper>();

// Add services
services.AddScoped<IUserService, UserService>();
services.AddScoped<ITaskService, TaskService>();
services.AddScoped<ITagService, TagService>();
services.AddScoped<IAuthService, AuthService>();

services.AddSwaggerGen();

services.AddControllers();

// Add FluentValidation
services.AddFluentValidation()
    .AddValidatorsFromAssembly(typeof(UserCreateDtoValidator).Assembly);


var app = builder.Build();

app.UseRouting();

app.UseCors(policyName);

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DtoPractice API v1");
        c.RoutePrefix = string.Empty;
    });
}

Log.Information("Starting up the app");
app.Run();

