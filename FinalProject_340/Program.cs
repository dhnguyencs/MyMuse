using FinalProject_340.Models;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

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

app.UseAuthorization();

//app.UseMiddleware<Users>();

app.Use(async (context, next) => {
    //first grab the sessionID cookie from the request
    string? cookieValueFromReq = context.Request.Cookies["SessionID"];

    //get originating path of the request
    String Paths = context.Request.Path.ToString().ToLower();

    //1st check: if cookie is null or empty, we don't do anything but redirect them to the login page
    // To avoid circular redirect, we also check if the path is either the login page or registration page
    if (cookieValueFromReq.IsNullOrEmpty()
        && (Paths == "/login/index" || Paths == "/login/createaccount"))
    {
        await next(context);
        return;
    }

    //2nd check: we attempt to retrieve the user from the database using our cookie
    Users? user = Users.getUser(cookieValueFromReq);

    //3rd check: if user is null after attempting to grab their information using the provided cookie from request, we redirect them to the login page
    if ((user == null && Paths != "/login/login")
        || (user != null && Paths == "/login/createaccount"))
    {
        context.Response.Redirect("/login/index");
        return;
    }
    //if the user model passes the above check, we add the user retrieved to the http context.
    context.Items.Add("User", user);
    await next(context);
});

app.MapControllerRoute(
    name        : "default",
    pattern     : "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name        : "login",
    pattern     : "{controller=Login}/{action=Index}/{id?}");

app.Run();
