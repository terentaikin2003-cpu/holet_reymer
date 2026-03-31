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
    [Authorize(Roles = "Администратор,Горничная")]
    public class HousekeepingTasksController : Controller
    {
        private readonly HotelContext _context;

        public HousekeepingTasksController(HotelContext context)
        {
            _context = context;
        }

        // GET: HousekeepingTasks
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.HousekeepingTasks.Include(h => h.Room).Include(h => h.User);
            return View(await hotelContext.ToListAsync());
        }

        // GET: HousekeepingTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var housekeepingTask = await _context.HousekeepingTasks
                .Include(h => h.Room)
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.HousekeepingId == id);
            if (housekeepingTask == null)
            {
                return NotFound();
            }

            return View(housekeepingTask);
        }

        // GET: HousekeepingTasks/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId");
            return View();
        }

        // POST: HousekeepingTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,UserId,DueAt,CompletedAt,TaskStatus,Comment")] HousekeepingTask housekeepingTask)
        {
            if (ModelState.IsValid)
            {
                housekeepingTask.CreatedAt = DateTime.Now;
                if (housekeepingTask.PriorityNo < 1)
                    housekeepingTask.PriorityNo = 5;
                try
                {
                    _context.Add(housekeepingTask);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить задачу. Проверьте номер и пользователя, длину статуса (до 20 символов).");
                }
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", housekeepingTask.RoomId);
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", housekeepingTask.UserId);
            return View(housekeepingTask);
        }

        // GET: HousekeepingTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var housekeepingTask = await _context.HousekeepingTasks.FindAsync(id);
            if (housekeepingTask == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", housekeepingTask.RoomId);
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", housekeepingTask.UserId);
            return View(housekeepingTask);
        }

        // POST: HousekeepingTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HousekeepingId,RoomId,UserId,CreatedAt,DueAt,CompletedAt,TaskStatus,PriorityNo,Comment")] HousekeepingTask housekeepingTask)
        {
            if (id != housekeepingTask.HousekeepingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(housekeepingTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HousekeepingTaskExists(housekeepingTask.HousekeepingId))
                        return NotFound();
                    throw;
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить задачу. Приоритет 1–10; проверьте номер и пользователя.");
                }
                if (!ModelState.IsValid)
                {
                    ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", housekeepingTask.RoomId);
                    ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", housekeepingTask.UserId);
                    return View(housekeepingTask);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", housekeepingTask.RoomId);
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", housekeepingTask.UserId);
            return View(housekeepingTask);
        }

        // GET: HousekeepingTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var housekeepingTask = await _context.HousekeepingTasks
                .Include(h => h.Room)
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.HousekeepingId == id);
            if (housekeepingTask == null)
            {
                return NotFound();
            }

            return View(housekeepingTask);
        }

        // POST: HousekeepingTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(int id) =>
            DbGuard.RunDeleteAsync(this, async () =>
            {
                var housekeepingTask = await _context.HousekeepingTasks.FindAsync(id);
                if (housekeepingTask != null)
                    _context.HousekeepingTasks.Remove(housekeepingTask);
                await _context.SaveChangesAsync();
            });

        private bool HousekeepingTaskExists(int id)
        {
            return _context.HousekeepingTasks.Any(e => e.HousekeepingId == id);
        }
    }
}
