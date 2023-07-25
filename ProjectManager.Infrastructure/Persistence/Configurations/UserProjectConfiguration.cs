using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Persistence.Configurations
{
    public class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
    {
        public void Configure(EntityTypeBuilder<UserProject> builder)
        {
            builder.HasKey(x => new { x.ProjectId, x.UserId});

            builder.HasOne(x => x.Project)
                .WithMany(x => x.UserProjects)
                .HasForeignKey(x => x.ProjectId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserProjects)
                .HasForeignKey(x => x.UserId);
        }
    }
}
