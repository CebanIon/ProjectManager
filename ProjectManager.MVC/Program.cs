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
using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

var people = new List<Person>
{
    new Person("tom@gmail.com", "12345"),
    new Person("bob@gmail.com", "55555")
};

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     //.AddCookie(options => options.LoginPath = "/Account/Login");
.AddCookie(options =>
{
    options.LoginPath = "/Loginn";
    options.Events.OnRedirectToLogin = (context) =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

//.AddCookie(options => options.LoginPath = "/Account/_Login");
builder.Services.AddAuthorization();
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Administrator", policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
//    options.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "User", "Administrator"));
//});

builder.Services.AddMvc()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IProjectManagerDbContext>());

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ProjectManagerDbContext>();

builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromHours(1);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/loginn", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    string loginForm = @"<!DOCTYPE html>
    <html>
    <head>
        <meta charset='utf-8' />
        <title>METANIT.COM</title>
    </head>
    <body>
        <h2>Login Form</h2>
        <form method='post'>
            <p>
                <label>Email</label><br />
                <input name='email' />
            </p>
            <p>
                <label>Password</label><br />
                <input type='password' name='password' />
            </p>
            <input type='submit' value='Login' />
        </form>
    </body>
    </html>";
    await context.Response.WriteAsync(loginForm);
});

app.MapPost("/loginn", async (string? returnUrl, HttpContext context) =>
{
    var form = context.Request.Form;
    if (!form.ContainsKey("email") || !form.ContainsKey("password"))
        return Results.BadRequest("Email");

    string email = form["email"];
    string password = form["password"];

    Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
    if (person is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    //return RedirectToAction("Index", "Home");
    return Results.Redirect(returnUrl ?? "/Home/Index");

});
//app.UseSession();

app.MapRazorPages();
app.MapDefaultControllerRoute();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

record class Person(string Email, string Password);