using Microsoft.Extensions.DependencyInjection;
using ToDoList.Application.Services.Implementations;
using ToDoList.Application.Services.Interfaces;

namespace ToDoList.Application.Extensions;

public static class ApplicationExtensions
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserService, UserService>();
    }
}