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
            //get cookie from request if any
            string? cookieValueFromReq = Request.Cookies["SessionID"];
            //get user with cookie
            Users ? user = Users.getUser(cookieValueFromReq);
            //Song newSong = new Song()
            //{
            //    title = "k545",
            //    plays = 0,
            //    artist = "Mozart",
            //    songHash = "10000000",
            //    USR_UUID = user.UUID,
            //};
            //user.AddSong(newSong);

            //if either cookie or user is null, redirect to the login page instead
            if (cookieValueFromReq == null || user == null) return RedirectToAction("Index", "Login");
            //return the main page, passing in the model of the user
            return View(user);
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