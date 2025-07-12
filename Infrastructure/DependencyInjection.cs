using Application.Interfaces;
using Application.Interfaces.Users;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString)); // Ensure the Microsoft.EntityFrameworkCore.SqlServer package is installed  

        // Fix: Correct the service registration to match the interface and implementation relationship  
        services.AddScoped<IUserService, UserService>();

        return services;
    }
} 
