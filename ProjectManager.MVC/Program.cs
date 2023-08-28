using ProjectManager.Infrastructure;
using ProjectManager.Application;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.WebUI.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Infrastructure.Persistence;
using System.Security.Claims;
using FluentValidation.AspNetCore;
using FluentValidation;
using ProjectManager.Application.Projects.Commands.CreateProject;
using ProjectManager.Application.Projects.Commands.ModifyProject;
using ProjectManager.Application.ProjectTasks.Commands.CreateTasks;
using ProjectManager.Application.ProjectTasks.Commands.ModifyTask;
using ProjectManager.Application.Users.Commands.CreateUser;
using ProjectManager.Application.Users.Commands.UpdateUser;
using ProjectManager.Application.DTO_s.Projects;
using ProjectManager.Application.Validators.Projects;
using ProjectManager.Application.Validators.ProjectTasks;
using ProjectManager.Application.DTO_s.ProjectTasks;
using ProjectManager.Application.Validators.Users;
using ProjectManager.Application.DTO_s.Users;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001, configure => configure.UseHttps());
    options.ListenAnyIP(5000);
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { options.LoginPath = "/Account/Login";
                            options.Cookie.Name = "AuthCookieTaskManager";
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("administrator", policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
    options.AddPolicy("user", policy => policy.RequireClaim(ClaimTypes.Role, "User", "Administrator"));
});

builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddScoped<IValidator<CreateProjectDTO>, CreateProjectDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateProjectDTO>, UpdateProjectDTOValidator>();
builder.Services.AddScoped<IValidator<CreateTaskDTO>, CreateTaskDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateTaskDTO>, UpdateTaskDTOValidator>();
builder.Services.AddScoped<IValidator<CreateUserDTO>, CreateUserDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateUserDTO>, UpdateUserDTOValidator>();

builder.Services.AddMvc()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IProjectManagerDbContext>());

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ProjectManagerDbContext>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1800);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
