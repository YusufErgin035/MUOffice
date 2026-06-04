using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProgProjc.Data;
using WebProgProjc.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed Roles and Rooms
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        if (!context.Rooms.Any())
        {
            context.Rooms.AddRange(
                new Room { Name = "Toplantı Odası A", Description = "10 kişilik küçük toplantı odası", Capacity = 10 },
                new Room { Name = "Konferans Salonu", Description = "50 kişilik büyük konferans salonu", Capacity = 50 },
                new Room { Name = "Çalışma Odası 1", Description = "Sessiz çalışma alanı", Capacity = 4 },
                new Room { Name = "Çalışma Odası 2", Description = "Grup çalışma alanı", Capacity = 6 }
            );
            await context.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı başlatılırken hata oluştu.");
    }
}

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

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "";
    // Avoid infinite redirect if going to Register page or API endpoints
    if (!path.StartsWith("/account/register") && !path.StartsWith("/_framework"))
    {
        var userManager = context.RequestServices.GetService<UserManager<ApplicationUser>>();
        if (userManager != null && !userManager.Users.Any())
        {
            context.Response.Redirect("/Account/Register");
            return;
        }
    }
    await next.Invoke();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
