using FinalProject_340.Models;
using Microsoft.AspNetCore.Mvc;
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
            //SqlDBConnection<Users> newConnection = new SqlDBConnection<Users>("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=userDB_340;Connect Timeout=100;");
            //Users testUser = new Users("324324", "wardoc22@yahoo.commm", "David", "Nguyen");
            //newConnection.insertIntoTable(testUser);
            string? cookieValueFromReq = Request.Cookies["sessionID"];
            if (cookieValueFromReq == null) return RedirectToAction("Index", "Login"); 
            return View(new Users("123"));
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