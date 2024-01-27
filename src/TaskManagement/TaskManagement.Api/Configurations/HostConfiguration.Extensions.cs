using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagement.Application.Common.TaskManagement.Services;
using TaskManagement.Domain.Common.Caching;
using TaskManagement.Infrastructure.Common.Caching.Brokers;
using TaskManagement.Infrastructure.Common.TaskManagement.Services;
using TaskManagement.Persistence.Caching.Brokers;
using TaskManagement.Persistence.DbContexts;
using TaskManagement.Persistence.Interceptors;
using TaskManagement.Persistence.Repositories;
using TaskManagement.Persistence.Repositories.Interfaces;

namespace TaskManagement.Api.Configurations;

public static partial class HostConfiguration
{
    private static WebApplicationBuilder AddDevTools(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return builder;
    }

    private static WebApplicationBuilder AddCaching(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(nameof(CacheSettings)));

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
            options.InstanceName = "TaskManagementSystem";
        });

        builder.Services.AddSingleton<ICacheBroker, RedisDistributedCacheBroker>();
        return builder;
    }

    private static WebApplicationBuilder AddValidators(this WebApplicationBuilder builder)
    {
        var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
        assemblies.Add(Assembly.GetExecutingAssembly());

        builder.Services.AddValidatorsFromAssemblies(assemblies);
        return builder;
    }

    private static WebApplicationBuilder AddMappers(this WebApplicationBuilder builder)
    {
        var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
        assemblies.Add(Assembly.GetExecutingAssembly());

        builder.Services.AddAutoMapper(assemblies);
        return builder;
    }

    private static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<UpdateAuditableInterceptor>();

        builder.Services.AddDbContext<AppDbContext>((provider, options) =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"));

            options.AddInterceptors(provider.GetRequiredService<UpdateAuditableInterceptor>());
        });

        return builder;
    }

    private static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<ITaskRepository, TaskRepository>()
            .AddScoped<ITaskService, TaskService>();

        return builder;
    }

    private static WebApplicationBuilder AddExposers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        return builder;
    }

    private static WebApplication UseExposers(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }

    private static WebApplication UseDevTools(this WebApplication app)
    {
        app
            .UseSwagger()
            .UseSwaggerUI();

        return app;
    }
}
