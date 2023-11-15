using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;
using File = ProjectManager.Domain.Entities.File;

namespace ProjectManager.Infrastructure.Persistence.Configurations
{
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.HasKey(x => x.Id)
                .IsClustered(false);

            builder.Property(x => x.FileName)
                .IsRequired(false);

            builder.Property(x => x.FileData)
                .IsRequired(false);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(f => f.ProjectTask)
              .WithMany(pt => pt.Files)
              .HasForeignKey(f => f.ProjectTaskId);

            builder.HasOne(e => e.FileType)
                .WithMany(e => e.FilesByFileType)
                .HasForeignKey(e => e.FileTypeId);
        }
    }
}
