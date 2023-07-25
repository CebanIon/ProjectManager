using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class PriorityConfiguration : IEntityTypeConfiguration<Priority>
{
    public void Configure(EntityTypeBuilder<Priority> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(20);

        builder.HasData(ProjectManagerDbContextSeed.lowPriority, ProjectManagerDbContextSeed.mediumPriority, ProjectManagerDbContextSeed.highPriority, ProjectManagerDbContextSeed.urgentPriority);
    }
}

