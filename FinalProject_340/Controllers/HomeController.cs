using FinalProject_340.Middleware;
using FinalProject_340.Models;
using FinalProject_340.Tests;
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
        [Route("")]
        [Route("/{Controller=Home}/{sort?}")]
        [Route("/{Action=Index}/{sort?}")]
        [Route("/{Controller=Home}/{Action=Index}/{sort?}")]
        [Route("/{sort?}")]
        public IActionResult Index(int sort)
        {
            if (!Request.Cookies.ContainsKey("sort"))
                ViewData["sort"] = 1;
            if(sort != 0)
                ViewData["sort"] = sort;
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