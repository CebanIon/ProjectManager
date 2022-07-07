using ProjectManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.Property(x => x.UserName)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.UserName)
            .IsClustered(true)
            .IsUnique();

        //SHA-256 size
        builder.Property(x => x.Password)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.IsEnabled)
            .HasDefaultValue(true);

        builder.HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData(ProjectManagerDbContextSeed.Admin);
    }
}
