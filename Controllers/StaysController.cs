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
    [Authorize(Roles = "Администратор")]
    public class StaysController : Controller
    {
        private readonly HotelContext _context;

        public StaysController(HotelContext context)
        {
            _context = context;
        }

        // GET: Stays
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Stays.Include(s => s.BookingRoom);
            return View(await hotelContext.ToListAsync());
        }

        // GET: Stays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stay = await _context.Stays
                .Include(s => s.BookingRoom)
                .FirstOrDefaultAsync(m => m.StayId == id);
            if (stay == null)
            {
                return NotFound();
            }

            return View(stay);
        }

        // GET: Stays/Create
        public IActionResult Create()
        {
            ViewData["BookingRoomId"] = new SelectList(_context.BookingRooms, "BookingRoomId", "BookingRoomId");
            return View();
        }

        // POST: Stays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingRoomId,CheckinAt,CheckoutDatePlan,CheckoutAt,StayStatus,AmountBeforeDiscount,DiscountAmount,TotalAmount")] Stay stay)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(stay);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить запись. Проверьте, что выбран существующий «Бронь-Номер», длина статуса не больше 20 символов, и нет конфликта с ограничениями в базе данных.");
                }
            }
            ViewData["BookingRoomId"] = new SelectList(_context.BookingRooms, "BookingRoomId", "BookingRoomId", stay.BookingRoomId);
            return View(stay);
        }

        // GET: Stays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stay = await _context.Stays.FindAsync(id);
            if (stay == null)
            {
                return NotFound();
            }
            ViewData["BookingRoomId"] = new SelectList(_context.BookingRooms, "BookingRoomId", "BookingRoomId", stay.BookingRoomId);
            return View(stay);
        }

        // POST: Stays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StayId,BookingRoomId,CheckinAt,CheckoutDatePlan,CheckoutAt,StayStatus,AmountBeforeDiscount,DiscountAmount,TotalAmount")] Stay stay)
        {
            if (id != stay.StayId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StayExists(stay.StayId))
                        return NotFound();
                    throw;
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить проживание. Проверьте «Бронь-Номер», статус и суммы.");
                    ViewData["BookingRoomId"] = new SelectList(_context.BookingRooms, "BookingRoomId", "BookingRoomId", stay.BookingRoomId);
                    return View(stay);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingRoomId"] = new SelectList(_context.BookingRooms, "BookingRoomId", "BookingRoomId", stay.BookingRoomId);
            return View(stay);
        }

        // GET: Stays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stay = await _context.Stays
                .Include(s => s.BookingRoom)
                .FirstOrDefaultAsync(m => m.StayId == id);
            if (stay == null)
            {
                return NotFound();
            }

            return View(stay);
        }

        // POST: Stays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(int id) =>
            DbGuard.RunDeleteAsync(this, async () =>
            {
                var stay = await _context.Stays.FindAsync(id);
                if (stay != null)
                    _context.Stays.Remove(stay);
                await _context.SaveChangesAsync();
            });

        private bool StayExists(int id)
        {
            return _context.Stays.Any(e => e.StayId == id);
        }
    }
}
