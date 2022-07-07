using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsClustered(true)
            .IsUnique();

        builder.Property(x => x.Description)
            .IsRequired();

        builder.HasOne(x => x.TaskType)
          .WithMany(x => x.ProjectTasks)
          .HasForeignKey(x => x.TaskTypeId)
          .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.TaskState)
          .WithMany(x => x.ProjectTasks)
          .HasForeignKey(x => x.TaskStateId)
          .OnDelete(DeleteBehavior.NoAction);
    }
}
