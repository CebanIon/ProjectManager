﻿using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Common.Interfaces
{
    public interface IProjectManagerDbContext
    {
        DbSet<Role> Roles { get; }
        DbSet<Project> Projects { get; }
        DbSet<ProjectState> ProjectStates { get; }
        DbSet<ProjectTask> ProjectTasks { get; }
        DbSet<ProjectTaskState> ProjectTaskStates { get; }
        DbSet<ProjectTaskType> ProjectTaskTypes { get; }
        DbSet<User> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
