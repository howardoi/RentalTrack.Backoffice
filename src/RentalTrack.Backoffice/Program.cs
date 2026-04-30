using RentalTrack.Backoffice.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddBackofficeOptions(builder.Configuration)
    .AddBackofficeUtility()
    .AddBackofficeData()
    .AddBackofficeBusiness()
    .AddBackofficeServices()
    .AddBackofficeAuth();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Account/Login");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
