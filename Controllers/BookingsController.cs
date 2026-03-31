using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelReymer.Infrastructure;
using HotelReymer.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelReymer.Controllers
{
    [Authorize(Roles = "Администратор,Менеджер по бронированию")]
    public class BookingsController : Controller
    {
        private readonly HotelContext _context;

        public BookingsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Bookings.Include(b => b.Client).Include(b => b.User);
            return View(await hotelContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Client)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId");
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,UserId,CheckinDatePlan,CheckoutDatePlan,Status,Comment")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.CreatedAt = DateTime.Now;
                try
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить бронирование. Проверьте клиента и пользователя, длину статуса (до 20 символов) и корректность дат.");
                }
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", booking.ClientId);
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", booking.UserId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", booking.ClientId);
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", booking.UserId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,ClientId,UserId,CreatedAt,CheckinDatePlan,CheckoutDatePlan,Status,Comment")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить бронирование. Проверьте даты (выезд строго позже заезда), клиента и пользователя.");
                }
                if (!ModelState.IsValid)
                {
                    ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", booking.ClientId);
                    ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", booking.UserId);
                    return View(booking);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", booking.ClientId);
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", booking.UserId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Client)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(int id) =>
            DbGuard.RunDeleteAsync(this, async () =>
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking != null)
                    _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            });

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
