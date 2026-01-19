using DataAccessLibrary.Data;
using DeclutterMeBlazorUI.Features;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
        options.HandshakeTimeout = TimeSpan.FromSeconds(30);
    });
builder.Services.AddResponseCompression();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<DeclutterMeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddJSComponents();
WebApplication app = builder.Build();
await ApplyDbMigrations(app);

app.UseExceptionHandler("/Error", createScopeForErrors: true);

if (app.Environment.IsDevelopment() == false)
{
    app.UseHsts();
    app.UseResponseCompression();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();
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
        logger.LogError(ex, "An error occurred during migration.");
        Console.ReadKey();
    }
}