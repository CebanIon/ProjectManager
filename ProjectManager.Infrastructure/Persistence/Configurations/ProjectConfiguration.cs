using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
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

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);
        
        builder.HasOne(x => x.ProjectState)
          .WithMany(x => x.Projects)
          .HasForeignKey(x => x.ProjectStateId)
          .OnDelete(DeleteBehavior.NoAction);
    }
}
