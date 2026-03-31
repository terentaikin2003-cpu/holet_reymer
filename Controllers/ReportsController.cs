using HotelReymer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReymer.Controllers
{
    [Authorize(Roles = "Администратор,Менеджер по бронированию,Бухгалтер,Маркетолог")]
    public class ReportsController : Controller
    {
        private readonly HotelContext _context;

        public ReportsController(HotelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Отчёты";

            var totalRooms = await _context.Rooms.CountAsync();
            var occupiedRooms = await _context.Rooms.CountAsync(r => r.Status == "Занят");
            ViewBag.TotalRooms = totalRooms;
            ViewBag.OccupiedRooms = occupiedRooms;
            ViewBag.OccupancyPercent = totalRooms > 0
                ? Math.Round((double)occupiedRooms / totalRooms * 100, 1)
                : 0;

            var monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var monthEnd = monthStart.AddMonths(1);

            ViewBag.MonthRevenue = await _context.Payments
                .Where(p => p.PaymentAt >= monthStart && p.PaymentAt < monthEnd && p.PaymentStatus == "Успешно")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            ViewBag.MonthBookings = await _context.Bookings
                .Where(b => b.CreatedAt >= monthStart && b.CreatedAt < monthEnd)
                .CountAsync();

            ViewBag.ConfirmedBookings = await _context.Bookings
                .Where(b => b.CreatedAt >= monthStart && b.CreatedAt < monthEnd && b.Status == "Подтверждена")
                .CountAsync();

            ViewBag.CancelledBookings = await _context.Bookings
                .Where(b => b.CreatedAt >= monthStart && b.CreatedAt < monthEnd && b.Status == "Отменена")
                .CountAsync();

            var payments = await _context.Payments
                .Where(p => p.PaymentStatus == "Успешно")
                .ToListAsync();
            ViewBag.AvgCheck = payments.Count > 0
                ? Math.Round(payments.Average(p => p.Amount), 2)
                : 0m;

            ViewBag.ActiveStays = await _context.Stays.CountAsync(s => s.StayStatus == "Активна");
            ViewBag.PendingCleanings = await _context.HousekeepingTasks.CountAsync(h => h.TaskStatus != "Выполнена");
            ViewBag.TotalClients = await _context.Clients.CountAsync();
            ViewBag.ActiveCampaigns = await _context.MarketingCampaigns.CountAsync(c => c.IsActive);

            return View();
        }
    }
}
