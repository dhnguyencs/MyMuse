using FinalProject_340.Utilities;
using FinalProject_340.Models;

namespace FinalProject_340.Middleware
{
    public class Users_Service : Users
    {
        public static Users? _user;
        public static void setUser(Users user) { _user = user; }

        public static Users? getUser(String? cookie)
        {
            //if cookie is null, return a null user
            if (String.IsNullOrEmpty(cookie)) return (Users?)null;
            //retrieve session token from cookie
            SessionTokens? newToken = SessionTokens.getToken(cookie);
            //if the token is null or the account UUID associated with token is null, return a null user
            if (newToken == null || String.IsNullOrEmpty(newToken.accountHash)) return (Users?)null;
            //create a new sql db connection targeting the user table
            SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>(FinalProject_340.Properties.Resource.appData);
            //retrieve the user using the account UUID in the token
            Users? user = newConnection.getFirstOrDefault(new Dictionary<string, string>() { { "UUID", newToken.accountHash } });
            //return either the user or null user
            return user != null ? user : (Users?)null;
        }
        private readonly RequestDelegate _next;
        public Users_Service(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            string? cookieValueFromReq = context.Request.Cookies["SessionID"];
            _user = getUser(cookieValueFromReq);
            await this._next(context);
        }
    }
}
