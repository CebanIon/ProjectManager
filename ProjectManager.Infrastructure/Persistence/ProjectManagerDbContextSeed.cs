using ProjectManager.Domain.Entities;
using System.Xml.Linq;

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
        public static readonly ProjectTaskState ProjectTaskStateInProgress = new ProjectTaskState { Id = 5, Name = "In progress" };
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
        public static readonly ProjectState projectStateDone = new ProjectState { Id = 4, Name = "Done" };
        #endregion

        #region Priorities
        public static readonly Priority lowPriority = new Priority { Id = 1, Name="Low", PriorityValue = 1 };
        public static readonly Priority mediumPriority = new Priority { Id = 2, Name = "Medium", PriorityValue = 2 };
        public static readonly Priority highPriority = new Priority { Id = 3, Name = "High", PriorityValue = 3 };
        public static readonly Priority urgentPriority = new Priority { Id = 4, Name = "Urgent", PriorityValue = 4 };
        #endregion

        #region FileType
        public static readonly List<FileType> fileTypes = new List<FileType>
        {
            new FileType
            {
                Type = "Image"
            },
            new FileType
            {
                Type = "Document"
            },
            new FileType
            {
                Type = "Archive"
            },
            new FileType
            {
                Type = "Folder"
            },
            new FileType
            {
                Type = "Other"
            }
        };
        #endregion
    }
}
