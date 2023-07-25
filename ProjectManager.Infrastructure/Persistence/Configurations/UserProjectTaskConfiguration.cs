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
    public class UserProjectTaskConfiguration : IEntityTypeConfiguration<UserProjectTask>
    {
        public void Configure(EntityTypeBuilder<UserProjectTask> builder)
        {
            builder.HasKey(x => new { x.ProjectTaskId, x.UserId });

            builder.HasOne(x => x.ProjectTask)
                .WithMany(x => x.UserProjectTasks)
                .HasForeignKey(x => x.ProjectTaskId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserProjectTasks)
                .HasForeignKey(x => x.UserId);
        }
    }
}
