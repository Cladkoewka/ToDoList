using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Services.Implementations;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Application.Services.Mapping;
using ToDoList.Application.Validation.Task;
using ToDoList.Application.Validation.User;
using ToDoList.Domain.Interfaces;
using ToDoList.Infrastructure.DbContext;
using ToDoList.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<ITagRepository, TagRepository>();
services.AddScoped<ITaskRepository, TaskRepository>();

services.AddScoped<IUserMapper, UserMapper>();
services.AddScoped<ITagMapper, TagMapper>();
services.AddScoped<ITaskMapper, TaskMapper>();

services.AddScoped<IUserService, UserService>();
services.AddScoped<ITaskService, TaskService>();
services.AddScoped<ITagService, TagService>();

services.AddSwaggerGen();

services.AddControllers();
services.AddFluentValidation()
    .AddValidatorsFromAssembly(typeof(UserCreateDtoValidator).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500") // Порт вашего фронтенда
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DtoPractice API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapGet("/", () => "Hello World!");

app.Run();