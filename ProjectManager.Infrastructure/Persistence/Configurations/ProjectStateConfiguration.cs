using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class ProjectStateConfiguration : IEntityTypeConfiguration<ProjectState>
{
    public void Configure(EntityTypeBuilder<ProjectState> builder)
    {
        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsClustered(true)
            .IsUnique();

        builder.HasData(ProjectManagerDbContextSeed.projectStateDevelopment,
            ProjectManagerDbContextSeed.projectStateMaintenance,
            ProjectManagerDbContextSeed.projectStateFrozen);
    }
}
