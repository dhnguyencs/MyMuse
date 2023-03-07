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
            //if either cookie or user is null, redirect to the login page instead
            if (cookieValueFromReq == null || user == null) return RedirectToAction("Index", "Login");

            Song newSong = new Song()
            {
                title = "k545",
                plays = 0,
                artist = "Mozart",
                songHash = "10000000",
                USR_UUID = user.UUID,
                songLength = 349,
                fav = 1,

            };
            Song newSong_1 = new Song()
            {
                title = "This Is It",
                plays = 0,
                artist = "The Strokes",
                songHash = "10000001",
                USR_UUID = user.UUID,
                songLength = 145,
                fav = 0,
            };
            Song newSong_2 = new Song()
            {
                title = "Barely Legal",
                plays = 0,
                artist = "The Strokes",
                songHash = "10000002",
                USR_UUID = user.UUID,
                songLength = 123,
                fav = 1,
            };
            Song newSong_3 = new Song()
            {
                title = "2 Arabesques No 1",
                plays = 0,
                artist = "Claude Debussy",
                songHash = "10000003",
                USR_UUID = user.UUID,
                songLength = 424,
                fav = 1,
            };
            Song newSong_4 = new Song()
            {
                title = "Prelude",
                plays = 0,
                artist = "William Alwyn",
                songHash = "10000004",
                USR_UUID = user.UUID,
                songLength = 84,
                fav = 0,
            };
            Song newSong_5 = new Song()
            {
                title = "Un Sospiro",
                plays = 0,
                artist = "Franz Liszt",
                songHash = "10000005",
                USR_UUID = user.UUID,
                songLength = 345,
                fav = 1,
            };
            System.Console.WriteLine(user.AddSong(newSong));
            System.Console.WriteLine(user.AddSong(newSong_1));
            System.Console.WriteLine(user.AddSong(newSong_2));
            System.Console.WriteLine(user.AddSong(newSong_3));
            System.Console.WriteLine(user.AddSong(newSong_4));
            System.Console.WriteLine(user.AddSong(newSong_5));


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