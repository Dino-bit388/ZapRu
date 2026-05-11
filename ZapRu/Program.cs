using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using ZapRu.Data;
using ZapRu.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ZapRuDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.LoginPath = "/Home/Authorization";
    });
builder.Services.AddHttpClient<CarImportService>();
builder.Services.AddScoped<CatalogImportService>();

builder.Services.AddControllersWithViews().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataBase = scope.ServiceProvider.GetRequiredService<ZapRuDbContext>();
    dataBase.Database.Migrate();



    var carImporter = scope.ServiceProvider.GetRequiredService<CarImportService>();
    await carImporter.ImportPopularCarsAsync();

    var importer = scope.ServiceProvider.GetRequiredService<CatalogImportService>();
    var partsPath = Path.Combine(AppContext.BaseDirectory, "parts.json");
    await importer.ImportPartsAsync(partsPath);
}
app.UseDefaultFiles();
app.UseStaticFiles();




if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
