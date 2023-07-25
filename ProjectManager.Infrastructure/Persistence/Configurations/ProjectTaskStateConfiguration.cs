using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class ProjectTaskStateConfiguration : IEntityTypeConfiguration<ProjectTaskState>
{
    public void Configure(EntityTypeBuilder<ProjectTaskState> builder)
    {
        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsClustered(true)
            .IsUnique();

        builder.HasData(ProjectManagerDbContextSeed.ProjectTaskStateDone,
            ProjectManagerDbContextSeed.ProjectTaskStatePending,
            ProjectManagerDbContextSeed.ProjectTaskStateConfirmed,
            ProjectManagerDbContextSeed.ProjectTaskStateRejected,
            ProjectManagerDbContextSeed.ProjectTaskStateInProgress);
    }
}
