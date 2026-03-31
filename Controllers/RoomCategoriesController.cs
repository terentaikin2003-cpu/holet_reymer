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
    public class RoomCategoriesController : Controller
    {
        private readonly HotelContext _context;

        public RoomCategoriesController(HotelContext context)
        {
            _context = context;
        }

        // GET: RoomCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.RoomCategories.ToListAsync());
        }

        // GET: RoomCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomCategory = await _context.RoomCategories
                .FirstOrDefaultAsync(m => m.RoomCategoryId == id);
            if (roomCategory == null)
            {
                return NotFound();
            }

            return View(roomCategory);
        }

        // GET: RoomCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Capacity,ComfortLevel,BasePricePerDay")] RoomCategory roomCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(roomCategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить категорию номеров.");
                }
            }
            return View(roomCategory);
        }

        // GET: RoomCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomCategory = await _context.RoomCategories.FindAsync(id);
            if (roomCategory == null)
            {
                return NotFound();
            }
            return View(roomCategory);
        }

        // POST: RoomCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomCategoryId,Name,Capacity,ComfortLevel,BasePricePerDay")] RoomCategory roomCategory)
        {
            if (id != roomCategory.RoomCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomCategoryExists(roomCategory.RoomCategoryId))
                        return NotFound();
                    throw;
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить категорию. Вместимость ≥ 1, цена ≥ 0.");
                }
                if (!ModelState.IsValid)
                    return View(roomCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(roomCategory);
        }

        // GET: RoomCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomCategory = await _context.RoomCategories
                .FirstOrDefaultAsync(m => m.RoomCategoryId == id);
            if (roomCategory == null)
            {
                return NotFound();
            }

            return View(roomCategory);
        }

        // POST: RoomCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(int id) =>
            DbGuard.RunDeleteAsync(this, async () =>
            {
                var roomCategory = await _context.RoomCategories.FindAsync(id);
                if (roomCategory != null)
                    _context.RoomCategories.Remove(roomCategory);
                await _context.SaveChangesAsync();
            });

        private bool RoomCategoryExists(int id)
        {
            return _context.RoomCategories.Any(e => e.RoomCategoryId == id);
        }
    }
}
