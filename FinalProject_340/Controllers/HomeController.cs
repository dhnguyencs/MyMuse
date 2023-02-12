using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace FinalProject_340.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //tEsT test = new tEsT()
            //{
            //    c1 = 2,
            //    c2 = 3.3f,
            //    c3 = 'f',
            //    c4 = "Hello World",
            //    c5 = DateTime.Now
            //};

            //SqlDBConnection<tEsT> sqlDBConnection = new SqlDBConnection<tEsT>(FinalProject_340.Properties.Resource.appData);
            //sqlDBConnection.insertIntoTable(test);
            //sqlDBConnection.getFirst(new Dictionary<string, string>()
            //                        {
            //                            { "c4", "Hello World" }
            //                        }); 

            SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>(FinalProject_340.Properties.Resource.appData);
            string? cookieValueFromReq = Request.Cookies["sessionID"];
            //Request.Cookies are where all the cookies for
            //for any given user (connection/browser) is stored.

            if (cookieValueFromReq == null) return RedirectToAction("Index", "Login");
            //if there is no cookie, user is not logged in. Redirect to the login page (Index page, Controller)

            return View(getUser(cookieValueFromReq));
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
        public Users getUser(String cookie)
        {
            SqlDBConnection<SessionTokens> newSessionTokenConnection = new SqlDBConnection<SessionTokens>(FinalProject_340.Properties.Resource.appData);
            SessionTokens newToken = newSessionTokenConnection.getFirst(new Dictionary<string, string>()
            {
                {"SessionID", cookie }
            });
            SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>(FinalProject_340.Properties.Resource.appData);
            return newConnection.getFirst(new Dictionary<string, string>()
            {
                {"UUID", newToken.accountHash }
            });
        }
    }
}