using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Elasticsearch;
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
    .WriteTo.Seq("http://localhost:5341")
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"logs-myapp-{DateTime.UtcNow:yyyy.MM}",
    }));


var services = builder.Services;
var configuration = builder.Configuration;


// DbContext configuration
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

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

services.AddSwaggerGen();

services.AddControllers();

// Add FluentValidation
services.AddFluentValidation()
    .AddValidatorsFromAssembly(typeof(UserCreateDtoValidator).Assembly);


// Add CORS
const string CorsName = "AllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsName,
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500") // Frontend Port
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseRouting();
app.UseCors(CorsName);
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

try
{
    Log.Information("Starting up the app");
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "App startup failed");
}
finally
{
    Log.CloseAndFlush();
}



