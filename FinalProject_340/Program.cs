using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FinalProject_340.Data;
using FinalProject_340.Areas.Identity.Data;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FinalProject_340ContextConnection") ?? throw new InvalidOperationException("Connection string 'FinalProject_340ContextConnection' not found.");

builder.Services.AddDbContext<FinalProject_340Context>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User_CORE_ID>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<FinalProject_340Context>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name        : "default",
    pattern     : "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name        : "login",
    pattern     : "{controller=Login}/{action=Index}/{id?}");
app.UseEndpoints(endPoints =>
    {
        endPoints.MapRazorPages();
    }
   );
app.Run();
