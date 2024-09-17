using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Services.Implementations;
using ToDoList.Application.Services.Interfaces;
using ToDoList.Application.Services.Mapping;
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

var app = builder.Build();

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