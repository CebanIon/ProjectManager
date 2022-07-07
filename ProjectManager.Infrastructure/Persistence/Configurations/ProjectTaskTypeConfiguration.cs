using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class ProjectTaskTypeConfiguration : IEntityTypeConfiguration<ProjectTaskType>
{
    public void Configure(EntityTypeBuilder<ProjectTaskType> builder)
    {
        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsClustered(true)
            .IsUnique();

        builder.HasData(ProjectManagerDbContextSeed.projectTaskTypeBug,
            ProjectManagerDbContextSeed.projectTaskTypeFeature,
            ProjectManagerDbContextSeed.projectTaskTypeModify);
    }
}
