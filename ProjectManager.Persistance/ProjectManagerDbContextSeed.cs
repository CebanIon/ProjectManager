using ProjectManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ProjectManager.Persistance
{
    public static class ProjectManagerDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ProjectManagerDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.Add(new Role
                {
                    Name = "Administrator",
                    Description = "Application administrator user capable of adding, modifying and disabling other users.",
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
