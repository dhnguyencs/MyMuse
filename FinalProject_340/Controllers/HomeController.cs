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
        [Route("/")]
        [Route("/{Controller=Home}/{sort?}")]
        [Route("/{Action=Index}/{sort?}")]
        [Route("/{Controller=Home}/{Action=Index}/{sort?}")]
        [Route("/{sort?}")]
        public IActionResult Index(int sort = 0)
        {
            if (sort != 0)
            {
                CookieServices.SetCookie("sort", sort.ToString(), 9999, Response);
                ViewData["sort"] = sort;
            }
            if (sort == 0 && !Request.Cookies["sort"].IsNullOrEmpty())
            {
                int.TryParse(Request.Cookies["sort"], out sort);
                ViewData["sort"] = sort;
            }
            else if(sort == 0) ViewData["sort"] = 1;

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