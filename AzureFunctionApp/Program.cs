using Application.Interfaces.Users;
using Application.Queries;
using AzureFunctionApp.Orders;
using Google.Protobuf.WellKnownTypes;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Handlers;
using Infrastructure.Services.User;
using MediatR; // Ensure MediatR namespace is included  
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Instrumentation;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection.Metadata;



var builder = FunctionsApplication.CreateBuilder(args);

// Define CORS policy
/* var corsPolicyName = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});  */


// Fix: Provide a configuration action for MediatRServiceConfiguration instead of passing the assembly directly  
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllOrdersFunction).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllOrdersQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllOrdersQuery).Assembly));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetOrderByIdFunction).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetOrderByIdQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetOrderByIdQuery).Assembly));

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Command.Users.CreateUserCommand).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.DTOs.Users.UserReadDto).Assembly));


// Register DbContext and repositories
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>() // optional
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructure(connectionString!);
builder.Services.AddDistributedMemoryCache();



var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("AzureFunctionApp"))
                    .AddHttpClientInstrumentation()
                    .AddSource("Microsoft.Azure.Functions.Worker")
                    .AddConsoleExporter();
            });

      /*  services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

        services.AddDbContext<AppDbContext>(options =>
        {
            //var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUserService, UserService>(); */
    })
    .Build();



builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
