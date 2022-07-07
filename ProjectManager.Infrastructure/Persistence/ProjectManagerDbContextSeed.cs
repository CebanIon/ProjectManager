using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence
{
    public static class ProjectManagerDbContextSeed
    {
        #region Roles
        public static readonly Role AdminRole = new Role { Id = 1, Name = "Administrator", Description = "User that can add, modify and disable other users." };
        public static readonly Role UserRole = new Role { Id = 2, Name = "User", Description = "User that can access all section except managing other users." };
        #endregion

        #region Users
        public static readonly User Admin = new User
        {
            Id = 1,
            UserName = "Admin",
            Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
            FirstName = "Admin",
            LastName = "Admin",
            Email = "Admin@mail.com",
            IsEnabled = true,
            RoleId = AdminRole.Id,
        };
        #endregion

        #region ProjectTaskStates
        public static readonly ProjectTaskState ProjectTaskStateDone = new ProjectTaskState { Id = 1, Name = "Done" };
        public static readonly ProjectTaskState ProjectTaskStatePending = new ProjectTaskState { Id = 2, Name = "Pending" };
        public static readonly ProjectTaskState ProjectTaskStateRejected = new ProjectTaskState { Id = 3, Name = "Rejected" };
        public static readonly ProjectTaskState ProjectTaskStateConfirmed = new ProjectTaskState { Id = 4, Name = "Confirmed" };
        #endregion

        #region ProjectTaskTypes
        public static readonly ProjectTaskType projectTaskTypeBug = new ProjectTaskType { Id = 1, Name = "Bug" };
        public static readonly ProjectTaskType projectTaskTypeFeature = new ProjectTaskType { Id = 2, Name = "Feature" };
        public static readonly ProjectTaskType projectTaskTypeModify = new ProjectTaskType { Id = 3, Name = "Modify" };
        #endregion

        #region ProjectStates
        public static readonly ProjectState projectStateDevelopment = new ProjectState { Id = 1, Name = "Development" };
        public static readonly ProjectState projectStateMaintenance = new ProjectState { Id = 2, Name = "Maintenance" };
        public static readonly ProjectState projectStateFrozen = new ProjectState { Id = 3, Name = "Frozen" };
        #endregion

    }
}
