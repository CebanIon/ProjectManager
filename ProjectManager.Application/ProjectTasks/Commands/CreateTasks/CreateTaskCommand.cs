using MediatR;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.DTO_s.ProjectTasks;
using ProjectManager.Application.Enums;
using ProjectManager.Domain.Entities;
using System.Net.Http.Headers;
using File = ProjectManager.Domain.Entities.File;

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

            ProjectTask projectTask = new ProjectTask();

            projectTask.Name = request.DTO.Name;
            projectTask.Description = request.DTO.Description;
            projectTask.TaskState = _context.ProjectTaskStates.FirstOrDefault(x => x.Name == "Pending");
            projectTask.TaskStateId = projectTask.TaskState.Id;
            projectTask.TaskTypeId = request.DTO.TaskTypeId;
            projectTask.TaskType = _context.ProjectTaskTypes.FirstOrDefault(x => x.Id == request.DTO.TaskTypeId);
            projectTask.TaskStartDate = request.DTO.TaskStartDate;
            projectTask.TaskEndDate = request.DTO.TaskEndDate;
            projectTask.CreatedBy = request.DTO.CreatorId;
            projectTask.PriorityId = request.DTO.PriorityId;
            projectTask.Priority = _context.Priority.FirstOrDefault(x => x.Id == request.DTO.PriorityId);
            projectTask.LastModified = DateTime.UtcNow;
            projectTask.Created = DateTime.UtcNow;
            projectTask.LastModifiedBy = request.DTO.CreatorId;
            projectTask.ProjectId = request.DTO.ProjectId;
            projectTask.Project = _context.Projects.FirstOrDefault(x => x.Id == request.DTO.ProjectId);


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
                        uploadedFile.Type = GetFileType(extension).ToString();
                    }
                    if(uploadedFile != null)
                        projectTask.Files.Add(uploadedFile);
                }
            }

            await _context.ProjectTasks.AddAsync(projectTask, cancellationToken);
            
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public static FileType GetFileType(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                return FileType.Folder;
            }

            var extension = Path.GetExtension(filePath).ToLower();

            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".svg" };
            if (Array.Exists(imageExtensions, ext => ext == extension))
            {
                return FileType.Image;
            }

            var documentExtensions = new[] { ".doc", ".docx", ".pdf", ".txt", ".xls", ".xlsx", ".ppt", ".pptx" };
            if (Array.Exists(documentExtensions, ext => ext == extension))
            {
                return FileType.Document;
            }

            var archiveExtensions = new[] { ".zip", ".rar", ".tar", ".gz", ".7z" };
            if (Array.Exists(archiveExtensions, ext => ext == extension))
            {
                return FileType.Archive;
            }

            return FileType.Other;
        }
    }
}
