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
    public class DiscountsController : Controller
    {
        private readonly HotelContext _context;

        public DiscountsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Discounts
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Discounts.Include(d => d.Client);
            return View(await hotelContext.ToListAsync());
        }

        // GET: Discounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .Include(d => d.Client)
                .FirstOrDefaultAsync(m => m.DiscountId == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // GET: Discounts/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId");
            return View();
        }

        // POST: Discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,Name,DiscountValue,StartDate,EndDate,ReasonText,IsActive")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(discount);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить скидку. Проверьте клиента и данные.");
                }
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", discount.ClientId);
            return View(discount);
        }

        // GET: Discounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", discount.ClientId);
            return View(discount);
        }

        // POST: Discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscountId,ClientId,Name,DiscountValue,StartDate,EndDate,ReasonText,IsActive")] Discount discount)
        {
            if (id != discount.DiscountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountExists(discount.DiscountId))
                        return NotFound();
                    throw;
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить скидку. Проверьте клиента и значение скидки (≥ 0).");
                }
                if (!ModelState.IsValid)
                {
                    ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", discount.ClientId);
                    return View(discount);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", discount.ClientId);
            return View(discount);
        }

        // GET: Discounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .Include(d => d.Client)
                .FirstOrDefaultAsync(m => m.DiscountId == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(int id) =>
            DbGuard.RunDeleteAsync(this, async () =>
            {
                var discount = await _context.Discounts.FindAsync(id);
                if (discount != null)
                    _context.Discounts.Remove(discount);
                await _context.SaveChangesAsync();
            });

        private bool DiscountExists(int id)
        {
            return _context.Discounts.Any(e => e.DiscountId == id);
        }
    }
}
