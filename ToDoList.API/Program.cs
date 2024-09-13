using ToDoList.Application.Extensions;
using ToDoList.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


services.AddContext(builder.Configuration);
services.AddRepositories();
services.AddServices();
services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();