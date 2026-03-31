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
    public class BookingRoomsController : Controller
    {
        private readonly HotelContext _context;

        public BookingRoomsController(HotelContext context)
        {
            _context = context;
        }

        // GET: BookingRooms
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.BookingRooms.Include(b => b.Booking).Include(b => b.Room);
            return View(await hotelContext.ToListAsync());
        }

        // GET: BookingRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingRoom = await _context.BookingRooms
                .Include(b => b.Booking)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BookingRoomId == id);
            if (bookingRoom == null)
            {
                return NotFound();
            }

            return View(bookingRoom);
        }

        // GET: BookingRooms/Create
        public IActionResult Create()
        {
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            return View();
        }

        // POST: BookingRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,RoomId,PricePerDay,Capacity")] BookingRoom bookingRoom)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(bookingRoom);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить «Бронь-Номер». Проверьте бронирование, номер и цену.");
                }
            }
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingRoom.BookingId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", bookingRoom.RoomId);
            return View(bookingRoom);
        }

        // GET: BookingRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingRoom = await _context.BookingRooms.FindAsync(id);
            if (bookingRoom == null)
            {
                return NotFound();
            }
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingRoom.BookingId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", bookingRoom.RoomId);
            return View(bookingRoom);
        }

        // POST: BookingRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingRoomId,BookingId,RoomId,PricePerDay,Capacity")] BookingRoom bookingRoom)
        {
            if (id != bookingRoom.BookingRoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingRoomExists(bookingRoom.BookingRoomId))
                        return NotFound();
                    throw;
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить «Бронь-Номер». Проверьте цену (≥ 0) и вместимость (≥ 1).");
                }
                if (!ModelState.IsValid)
                {
                    ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingRoom.BookingId);
                    ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", bookingRoom.RoomId);
                    return View(bookingRoom);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingRoom.BookingId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", bookingRoom.RoomId);
            return View(bookingRoom);
        }

        // GET: BookingRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingRoom = await _context.BookingRooms
                .Include(b => b.Booking)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BookingRoomId == id);
            if (bookingRoom == null)
            {
                return NotFound();
            }

            return View(bookingRoom);
        }

        // POST: BookingRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(int id) =>
            DbGuard.RunDeleteAsync(this, async () =>
            {
                var bookingRoom = await _context.BookingRooms.FindAsync(id);
                if (bookingRoom != null)
                    _context.BookingRooms.Remove(bookingRoom);
                await _context.SaveChangesAsync();
            });

        private bool BookingRoomExists(int id)
        {
            return _context.BookingRooms.Any(e => e.BookingRoomId == id);
        }
    }
}
