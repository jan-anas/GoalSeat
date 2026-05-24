using GoalSeat.Data;
using GoalSeat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoalSeat.Controllers
{
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchTerm)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Message"] = "Please login to view your bookings.";
                return RedirectToAction("Login", "Account");
            }

            IQueryable<Booking> query = _context.Bookings
                .Where(booking => booking.UserId == userId.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.Trim();

                query = query.Where(booking =>
                    booking.CustomerName.Contains(term) ||
                    booking.CustomerEmail.Contains(term) ||
                    booking.MatchName.Contains(term));
            }

            List<Booking> bookings = await query
                .OrderByDescending(booking => booking.Id)
                .ToListAsync();

            ViewBag.SearchTerm = searchTerm;

            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string matchName,
            string ticketType,
            int ticketCount)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Message"] = "Please login before booking a ticket.";
                return RedirectToAction("Login", "Account");
            }

            User? user = await _context.Users.FindAsync(userId.Value);

            if (user == null)
            {
                HttpContext.Session.Clear();
                TempData["Message"] = "Please login again.";
                return RedirectToAction("Login", "Account");
            }

            decimal basePrice = GetBasePrice(matchName);
            decimal multiplier = GetMultiplier(ticketType);

            if (basePrice == 0m || multiplier == 0m || ticketCount < 1 || ticketCount > 10)
            {
                TempData["BookingError"] = "Please fill in all booking fields correctly.";
                return RedirectToAction("Index", "Home", null, "bookTicket");
            }

            Booking booking = new Booking
            {
                UserId = user.Id,
                CustomerName = user.FullName,
                CustomerEmail = user.Email,
                MatchName = matchName,
                TicketType = ticketType,
                TicketCount = ticketCount,
                TotalPrice = basePrice * multiplier * ticketCount
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["BookingMessage"] =
                $"Booking confirmed for {booking.CustomerName}. Total price: {booking.TotalPrice} SAR.";

            return RedirectToAction("Index", "Home", null, "bookTicket");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Message"] = "Please login first.";
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            Booking? booking = await _context.Bookings
                .FirstOrDefaultAsync(booking =>
                    booking.Id == id &&
                    booking.UserId == userId.Value);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            string matchName,
            string ticketType,
            int ticketCount)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Message"] = "Please login first.";
                return RedirectToAction("Login", "Account");
            }

            Booking? booking = await _context.Bookings
                .FirstOrDefaultAsync(booking =>
                    booking.Id == id &&
                    booking.UserId == userId.Value);

            if (booking == null)
            {
                return NotFound();
            }

            decimal basePrice = GetBasePrice(matchName);
            decimal multiplier = GetMultiplier(ticketType);

            if (basePrice == 0m || multiplier == 0m || ticketCount < 1 || ticketCount > 10)
            {
                ViewBag.Error = "Please choose valid booking information.";
                return View(booking);
            }

            booking.MatchName = matchName;
            booking.TicketType = ticketType;
            booking.TicketCount = ticketCount;
            booking.TotalPrice = basePrice * multiplier * ticketCount;

            await _context.SaveChangesAsync();

            TempData["BookingMessage"] = "Booking updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Message"] = "Please login first.";
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            Booking? booking = await _context.Bookings
                .FirstOrDefaultAsync(booking =>
                    booking.Id == id &&
                    booking.UserId == userId.Value);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Message"] = "Please login first.";
                return RedirectToAction("Login", "Account");
            }

            Booking? booking = await _context.Bookings
                .FirstOrDefaultAsync(booking =>
                    booking.Id == id &&
                    booking.UserId == userId.Value);

            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            TempData["BookingMessage"] = "Booking deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private static decimal GetBasePrice(string matchName)
        {
            return matchName switch
            {
                "Arsenal vs Manchester City" => 250m,
                "Liverpool vs Chelsea" => 220m,
                "Manchester United vs Tottenham" => 200m,
                "Newcastle United vs Aston Villa" => 170m,
                "Arsenal vs Liverpool" => 230m,
                "Chelsea vs Tottenham" => 210m,
                "Manchester City vs Newcastle" => 240m,
                _ => 0m
            };
        }

        private static decimal GetMultiplier(string ticketType)
        {
            return ticketType switch
            {
                "VIP" => 2m,
                "Standard" => 1.5m,
                "Economy" => 1m,
                _ => 0m
            };
        }
    }
}