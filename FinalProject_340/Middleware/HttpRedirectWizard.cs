using FinalProject_340.Models;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject_340.Middleware
{
    public class HttpRedirectWizard
    {
        private readonly RequestDelegate _next;
        public HttpRedirectWizard(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            //first grab the sessionID cookie from the request
            string? cookieValueFromReq = context.Request.Cookies["SessionID"];

            //get originating path of the request
            String Paths = context.Request.Path.ToString().ToLower();

            //1st check: if cookie is null or empty, we don't do anything but redirect them to the login page
            // To avoid circular redirect, we also check if the path is either the login page or registration page
            if (cookieValueFromReq.IsNullOrEmpty()
                && (Paths == "/login/index" || Paths == "/login/createaccount"))
            {
                await this._next(context);
                return;
            }
            Users? user = Users_Service.getUser(cookieValueFromReq);
            //3rd check: if user is null after attempting to grab their information using the provided cookie from request, we redirect them to the login page
            if (user == null && Paths == "/login/index")
            {
                await this._next(context);
                return;
            }
            if ((user == null && Paths != "/login/login")
                || (user != null && Paths == "/login/createaccount"))
            {
                context.Response.Redirect("/login/index");
                return;
            }
            //if the user model passes the above check, we add the user retrieved to the http context.
            await this._next(context);
        }
    }
}
