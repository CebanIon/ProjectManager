using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.DTO_s.ProjectTasks;
using ProjectManager.Application.Projects.Queries.GetAllProjectsByUserId;
using ProjectManager.Application.TaskPriority.Queries.GetAllTaskPriorities;
using ProjectManager.Application.TaskState.Queries;
using ProjectManager.Application.TaskType.Queries.GetAllTaskTypes;
using ProjectManager.Domain.Entities;
using File = ProjectManager.Domain.Entities.File;
using FileType = ProjectManager.Application.Enums.FileType;

namespace ProjectManager.Application.ProjectTasks.Commands.CreateTasks
{
    public class CreateTaskCommand : IRequest<int>
    {
        public CreateTaskDTO DTO { get; set; }
    }

    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly IProjectManagerDbContext _context;
        public CreateTaskHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.DTO == null)
            {
                return 0;
            }

            ProjectTask projectTask = new ProjectTask
            {
                Name = request.DTO.Name,
                Description = request.DTO.Description,
                TaskState = _context.ProjectTaskStates.FirstOrDefault(x => x.Name == "Pending"),
                TaskStateId = _context.ProjectTaskStates.FirstOrDefault(x => x.Name == "Pending").Id,
                TaskTypeId = request.DTO.TaskTypeId,
                TaskType = _context.ProjectTaskTypes.FirstOrDefault(x => x.Id == request.DTO.TaskTypeId),
                TaskStartDate = request.DTO.TaskStartDate,
                TaskEndDate = request.DTO.TaskEndDate,
                CreatedBy = request.DTO.CreatorId,
                PriorityId = request.DTO.PriorityId,
                Priority = _context.Priority.FirstOrDefault(x => x.Id == request.DTO.PriorityId),
                LastModified = DateTime.UtcNow,
                Created = DateTime.UtcNow,
                LastModifiedBy = request.DTO.CreatorId,
                ProjectId = request.DTO.ProjectId,
                Project = _context.Projects.FirstOrDefault(x => x.Id == request.DTO.ProjectId)
            };

            ///<summary>
            /// Inserting files for project
            /// </summary>
            projectTask.Files = new List<File>();
            if (request.DTO.Files != null && request.DTO.Files.Count > 0)
            {
                foreach(var file in request.DTO.Files)
                {
                    var uploadedFile = new File();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        uploadedFile.FileData = ms.ToArray();
                        uploadedFile.FileName = file.FileName;
                        uploadedFile.CreatedBy = request.DTO.CreatorId;
                        uploadedFile.Created = DateTime.UtcNow;
                        uploadedFile.LastModifiedBy = request.DTO.CreatorId;
                        uploadedFile.LastModified = DateTime.UtcNow;

                        var extension = Path.GetExtension(file.FileName);
                        //uploadedFile.FileTypeId = await GetFileType(extension, cancellationToken);
                    }
                    if(uploadedFile != null)
                        projectTask.Files.Add(uploadedFile);
                }
            }

            await _context.ProjectTasks.AddAsync(projectTask, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetFileType(string filePath, CancellationToken cancellationToken)
        {
            var fileType = "";

            if (Directory.Exists(filePath))
                fileType = "Folder";

            var extension = Path.GetExtension(filePath).ToLower();

            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".svg" };
            if (Array.Exists(imageExtensions, ext => ext == extension))
                filePath = "Image";

            var documentExtensions = new[] { ".doc", ".docx", ".pdf", ".txt", ".xls", ".xlsx", ".ppt", ".pptx" };
            if (Array.Exists(documentExtensions, ext => ext == extension))
                filePath = "Document";

            var archiveExtensions = new[] { ".zip", ".rar", ".tar", ".gz", ".7z" };
            if (Array.Exists(archiveExtensions, ext => ext == extension))
                filePath = "Archive";

            int result = (await _context.FileTypes
                .FirstOrDefaultAsync(x => x.Type == fileType, cancellationToken)).Id;

            return result;
        }
    }
}
