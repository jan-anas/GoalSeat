using Microsoft.AspNetCore.Mvc;

namespace GoalSeat.Controllers
{
    public class MatchesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}