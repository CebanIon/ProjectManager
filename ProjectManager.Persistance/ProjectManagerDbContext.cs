using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Common;
using ProjectManager.Domain.Entities;
using System.Reflection;

namespace ProjectManager.Persistance
{
    public class ProjectManagerDbContext : DbContext, IProjectManagerDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ProjectManagerDbContext(DbContextOptions<ProjectManagerDbContext> options,
            IDateTime dateTime,
            ICurrentUserService currentUserService)
                : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectState> ProjectStates => Set<ProjectState>();
        public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();
        public DbSet<ProjectUserRole> ProjectUserRoles => Set<ProjectUserRole>();
        public DbSet<ProjectToUser> ProjectToUsers => Set<ProjectToUser>();
        public DbSet<ProjectTaskState> ProjectTaskStates => Set<ProjectTaskState>();
        public DbSet<ProjectTaskType> ProjectTaskTypes => Set<ProjectTaskType>();
        public DbSet<User> Users => Set<User>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
