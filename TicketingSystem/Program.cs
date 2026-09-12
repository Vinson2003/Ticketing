using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data.Context;
using TicketingSystem.Data.Entities;
using TicketingSystem.Helper;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ActionFilter>();
});

ConfigureService(builder.Services, builder.Configuration);

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Authentication HARUS sebelum Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();


// Configure services and dependency injection
void ConfigureService(IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });

    // Services
    services.AddScoped<ITicketService, TicketService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IPermissionService, PermissionService>();
    services.AddScoped<ITicketHistoryService, TicketHistoryService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<ITicketAccessService, TicketAccessService>();
    services.AddScoped<IDashboardService, DashboardService>();

    // Add memory cache
    services.AddMemoryCache();

    services.AddScoped<ActionFilter>();
}