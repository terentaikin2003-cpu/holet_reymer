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
    [Authorize(Roles = "Администратор,Бухгалтер")]
    public class PaymentsController : Controller
    {
        private readonly HotelContext _context;

        public PaymentsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Payments.Include(p => p.Stay).Include(p => p.User);
            return View(await hotelContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Stay)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["StayId"] = new SelectList(_context.Stays, "StayId", "StayId");
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StayId,UserId,Amount,PaymentMethod,PaymentStatus,ExternalId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.PaymentAt = DateTime.Now;
                try
                {
                    _context.Add(payment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить платёж. Проверьте проживание, пользователя и длину текстовых полей.");
                }
            }
            ViewData["StayId"] = new SelectList(_context.Stays, "StayId", "StayId", payment.StayId);
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", payment.UserId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["StayId"] = new SelectList(_context.Stays, "StayId", "StayId", payment.StayId);
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", payment.UserId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,StayId,UserId,PaymentAt,Amount,PaymentMethod,PaymentStatus,ExternalId")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить платёж. Сумма должна быть больше нуля; проверьте проживание и поля.");
                }
                if (!ModelState.IsValid)
                {
                    ViewData["StayId"] = new SelectList(_context.Stays, "StayId", "StayId", payment.StayId);
                    ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", payment.UserId);
                    return View(payment);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StayId"] = new SelectList(_context.Stays, "StayId", "StayId", payment.StayId);
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", payment.UserId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Stay)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(int id) =>
            DbGuard.RunDeleteAsync(this, async () =>
            {
                var payment = await _context.Payments.FindAsync(id);
                if (payment != null)
                    _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            });

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
