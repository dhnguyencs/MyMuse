using FinalProject_340.Middleware;
using FinalProject_340.Models;
using FinalProject_340.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace FinalProject_340.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private int cookieTrigger(HttpRequest request, int newVal, string cookieName)
        {
            if (newVal == 0 && !Request.Cookies[cookieName].IsNullOrEmpty())
            {
                int.TryParse(Request.Cookies[cookieName], out newVal);
                return newVal;
            }
            if (newVal != 0)
            {
                CookieServices.SetCookie(cookieName, newVal.ToString(), 9999, Response);
                return newVal;
            }        
            return 0;
        }

        [Route("/")]
        [Route("/{Controller=Home}/{Action=Index}/{sort?}/{uploadMultiple?}")]
        [Route("/{Action=Index}")]
        [Route("{Action=Index}/{sort?}")]
        [Route("/{Action=Index}/{sort?}/{uploadMultiple?}")]
        public IActionResult Index(int sort = 0, int uploadMultiple = 0)
        {
            ViewData["sort"] = cookieTrigger(Request, sort, "usr_pref_sort");
            ViewData["upload_as_list"] = cookieTrigger(Request, uploadMultiple, "usr_pref_asList");
            return View(Users_Service._user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}