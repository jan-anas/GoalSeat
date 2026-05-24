using GoalSeat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GoalSeat.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            bool cookiesAccepted = Request.Cookies["GoalSeatConsent"] == "Accepted";

            if (cookiesAccepted)
            {
                string visitorName = HttpContext.Session.GetString("UserName") ?? "Guest";
                string browserInfo = Request.Headers.UserAgent.ToString();

                Response.Cookies.Append("GoalSeatVisitor", visitorName, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(15),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                Response.Cookies.Append("GoalSeatBrowser", browserInfo, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(15),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AcceptCookies(string? returnUrl)
        {
            Response.Cookies.Append("GoalSeatConsent", "Accepted", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                IsEssential = true
            });

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
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
