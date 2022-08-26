WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DeclutterMeDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

WebApplication app = builder.Build();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
