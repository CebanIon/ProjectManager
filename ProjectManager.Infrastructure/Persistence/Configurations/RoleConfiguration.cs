using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.Name)
            .HasMaxLength(40)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsClustered(true)
            .IsUnique();

        builder.HasData(ProjectManagerDbContextSeed.AdminRole, ProjectManagerDbContextSeed.UserRole);
    }
}
