﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectManager.Infrastructure.Persistence;

#nullable disable

namespace ProjectManager.Infrastructure.Migrations
{
    [DbContext(typeof(ProjectManagerDbContext))]
    partial class ProjectManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProjectManager.Domain.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LastModifiedBy")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("ProjectEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProjectStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProjectStateId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Name")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Name"));

                    b.HasIndex("ProjectStateId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.ProjectState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Name")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Name"));

                    b.ToTable("ProjectStates");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Development"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Maintenance"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Frozen"
                        });
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.ProjectTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LastModifiedBy")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TaskEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TaskStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TaskStateId")
                        .HasColumnType("int");

                    b.Property<int>("TaskTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Name")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Name"));

                    b.HasIndex("ProjectId");

                    b.HasIndex("TaskStateId");

                    b.HasIndex("TaskTypeId");

                    b.ToTable("ProjectTasks");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.ProjectTaskState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Name")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Name"));

                    b.ToTable("ProjectTaskStates");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Done"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Pending"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Confirmed"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Rejected"
                        });
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.ProjectTaskType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Name")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Name"));

                    b.ToTable("ProjectTaskTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Bug"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Feature"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Modify"
                        });
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Name")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Name"));

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "User that can add, modify and disable other users.",
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = 2,
                            Description = "User that can access all section except managing other users.",
                            Name = "User"
                        });
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LastModifiedBy")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("UserName"));

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedBy = 0,
                            Email = "Admin@mail.com",
                            FirstName = "Admin",
                            IsEnabled = true,
                            LastName = "Admin",
                            Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                            RoleId = 1,
                            UserName = "Admin"
                        });
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.UserProject", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ProjectId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserProject");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.UserProjectTask", b =>
                {
                    b.Property<int>("ProjectTaskId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ProjectTaskId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserProjectTask");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.Project", b =>
                {
                    b.HasOne("ProjectManager.Domain.Entities.ProjectState", "ProjectState")
                        .WithMany("Projects")
                        .HasForeignKey("ProjectStateId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ProjectState");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.ProjectTask", b =>
                {
                    b.HasOne("ProjectManager.Domain.Entities.Project", "Project")
                        .WithMany("ProjectTasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ProjectManager.Domain.Entities.ProjectTaskState", "TaskState")
                        .WithMany("ProjectTasks")
                        .HasForeignKey("TaskStateId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ProjectManager.Domain.Entities.ProjectTaskType", "TaskType")
                        .WithMany("ProjectTasks")
                        .HasForeignKey("TaskTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("TaskState");

                    b.Navigation("TaskType");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.User", b =>
                {
                    b.HasOne("ProjectManager.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.UserProject", b =>
                {
                    b.HasOne("ProjectManager.Domain.Entities.Project", "Project")
                        .WithMany("UserProjects")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Domain.Entities.User", "User")
                        .WithMany("UserProjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.UserProjectTask", b =>
                {
                    b.HasOne("ProjectManager.Domain.Entities.ProjectTask", "ProjectTask")
                        .WithMany("UserProjectTasks")
                        .HasForeignKey("ProjectTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Domain.Entities.User", "User")
                        .WithMany("UserProjectTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProjectTask");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.Project", b =>
                {
                    b.Navigation("ProjectTasks");

                    b.Navigation("UserProjects");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.ProjectState", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.ProjectTask", b =>
                {
                    b.Navigation("UserProjectTasks");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.ProjectTaskState", b =>
                {
                    b.Navigation("ProjectTasks");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.ProjectTaskType", b =>
                {
                    b.Navigation("ProjectTasks");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("ProjectManager.Domain.Entities.User", b =>
                {
                    b.Navigation("UserProjectTasks");

                    b.Navigation("UserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}
