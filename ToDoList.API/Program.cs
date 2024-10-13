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

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")/*"User ID=postgres;Password=1339;Port=5432;Database=to-do;Host=localhost"*/;
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")/*"SecretKeySecretKeySecretKeySecretKey"*/;


// DbContext configuration
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

const string policyName = "AllowOrigin";
services.AddCors(options =>
{
    options.AddPolicy(policyName, builder =>
    {
        builder.WithOrigins("https://cladkoewka.github.io")
            .AllowAnyMethod()
            .AllowAnyHeader();
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
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

