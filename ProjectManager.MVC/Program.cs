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
using ProjectManager.Application.ProjectTasks.Queries.ModifyTask;
using ProjectManager.Application.ProjectTasks.Queries.ModifyTask.Validator;
using ProjectManager.Application.ProjectTasks.Queries.CreateTasks;
using ProjectManager.Application.ProjectTasks.Queries.CreateTasks.Validator;
using ProjectManager.Application.Users.Queries.CreateUser;
using ProjectManager.Application.Users.Queries.CreateUser.Validator;
using ProjectManager.Application.Users.Queries.UpdateUser;
using ProjectManager.Application.Users.Queries.UpdateUser.Validator;
using ProjectManager.Application.Projects.Queries.CreateProject;
using ProjectManager.Application.Projects.Queries.CreateProject.Validator;
using ProjectManager.Application.Projects.Queries.ModifyProject;
using ProjectManager.Application.Projects.Queries.ModifyProject.Validator;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001); // to listen for incoming http connection on port 5001
    options.ListenAnyIP(7001, configure => configure.UseHttps()); // to listen for incoming https connection on port 7001
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

builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddScoped<IValidator<CreateProjectQuery>, CreateProjectQueryValidator>();
builder.Services.AddScoped<IValidator<ModifyProjectQuery>, ModifyProjectQueryValidator>();
builder.Services.AddScoped<IValidator<CreateTaskQuery>, CreateTaskQueryValidator>();
builder.Services.AddScoped<IValidator<ModifyTaskQuery>, ModifyTaskQueryValidator>();
builder.Services.AddScoped<IValidator<CreateUserQuery>, CreateUserQueryValidator>();
builder.Services.AddScoped<IValidator<UpdateUserQuery>, UpdateUserQueryValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
