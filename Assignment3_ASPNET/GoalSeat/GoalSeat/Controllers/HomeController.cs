using GoalSeat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GoalSeat.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string visitorName = HttpContext.Session.GetString("UserName") ?? "Guest";
            string browserInfo = Request.Headers.UserAgent.ToString();

            CookieOptions options = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(15),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("GoalSeatVisitor", visitorName, options);
            Response.Cookies.Append("GoalSeatBrowser", browserInfo, options);

            ViewBag.CookieUser = Request.Cookies["GoalSeatVisitor"] ?? visitorName;

            return View();
        }
        public IActionResult About()
        {
            return View();
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
