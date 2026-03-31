using HotelReymer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HotelReymer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HotelContext _context;

        public HomeController(HotelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserName = User.Identity?.Name;

            if (User.IsInRole("Администратор") || User.IsInRole("Менеджер по бронированию"))
            {
                ViewBag.ClientCount = await _context.Clients.CountAsync();
                ViewBag.BookingCount = await _context.Bookings.CountAsync();
            }

            if (User.IsInRole("Администратор") || User.IsInRole("Менеджер по бронированию") || User.IsInRole("Горничная"))
            {
                ViewBag.RoomCount = await _context.Rooms.CountAsync();
                ViewBag.FreeRooms = await _context.Rooms.CountAsync(r => r.Status == "Свободен");
            }

            if (User.IsInRole("Администратор"))
            {
                ViewBag.ActiveStays = await _context.Stays.CountAsync(s => s.StayStatus == "Активна");
            }

            if (User.IsInRole("Администратор") || User.IsInRole("Бухгалтер"))
            {
                ViewBag.TodayPayments = await _context.Payments
                    .Where(p => p.PaymentAt.Date == DateTime.Today)
                    .SumAsync(p => (decimal?)p.Amount) ?? 0;
            }

            if (User.IsInRole("Горничная"))
            {
                ViewBag.PendingCleanings = await _context.HousekeepingTasks
                    .CountAsync(h => h.TaskStatus != "Выполнена");
            }

            if (User.IsInRole("Маркетолог"))
            {
                ViewBag.ActiveCampaigns = await _context.MarketingCampaigns.CountAsync(c => c.IsActive);
            }

            if (User.IsInRole("Системный администратор"))
            {
                ViewBag.UserCount = await _context.UserAccounts.CountAsync();
                ViewBag.RecentLogs = await _context.AuditLogs.CountAsync();
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
