using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Infrastructure.Persistence;
using ProjectManager.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ProjectManagerDbContext>(options =>
                options.UseInMemoryDatabase("ProjectManagerDb"));
        }
        else
        {
            services.AddDbContext<ProjectManagerDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ProjectManagerDbConnection"),
                    b => b.MigrationsAssembly(typeof(ProjectManagerDbContext).Assembly.FullName)));
        }

        services.AddScoped<IProjectManagerDbContext>(provider => provider.GetRequiredService<ProjectManagerDbContext>());

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}
