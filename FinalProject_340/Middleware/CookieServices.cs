using Azure;

namespace FinalProject_340.Models
{
    public static class CookieServices
    {
        public static void SetCookie(string key, string value, int? expireTime, HttpResponse httpRes)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue) option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else option.Expires = DateTime.Now.AddMilliseconds(10);

            httpRes.Cookies.Append(key, value, option);
        }
    }
}
