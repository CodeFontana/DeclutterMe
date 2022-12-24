using DataAccessLibrary.Data;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DeclutterMeDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

WebApplication app = builder.Build();
await ApplyDbMigrations(app);

if (app.Environment.IsDevelopment() == false)
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.Run();

static async Task ApplyDbMigrations(WebApplication app)
{
    using IServiceScope scope = app.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        DeclutterMeDbContext context = services.GetRequiredService<DeclutterMeDbContext>();
        ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration.");
        Console.ReadKey();
    }
}