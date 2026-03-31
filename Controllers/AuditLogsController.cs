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
    [Authorize(Roles = "Системный администратор")]
    public class AuditLogsController : Controller
    {
        private readonly HotelContext _context;

        public AuditLogsController(HotelContext context)
        {
            _context = context;
        }

        // GET: AuditLogs
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.AuditLogs.Include(a => a.User);
            return View(await hotelContext.ToListAsync());
        }

        // GET: AuditLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditLog = await _context.AuditLogs
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.LogId == id);
            if (auditLog == null)
            {
                return NotFound();
            }

            return View(auditLog);
        }

        // GET: AuditLogs/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId");
            return View();
        }

        // POST: AuditLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Action,Entity,EntityId,Status")] AuditLog auditLog)
        {
            if (ModelState.IsValid)
            {
                auditLog.Date = DateTime.Now;
                try
                {
                    _context.Add(auditLog);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить запись журнала. Проверьте пользователя и длину полей.");
                }
            }
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", auditLog.UserId);
            return View(auditLog);
        }

        // GET: AuditLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditLog = await _context.AuditLogs.FindAsync(id);
            if (auditLog == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", auditLog.UserId);
            return View(auditLog);
        }

        // POST: AuditLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LogId,UserId,Date,Action,Entity,EntityId,Status")] AuditLog auditLog)
        {
            if (id != auditLog.LogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auditLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuditLogExists(auditLog.LogId))
                        return NotFound();
                    throw;
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить запись журнала. Проверьте пользователя и длину полей.");
                }
                if (!ModelState.IsValid)
                {
                    ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", auditLog.UserId);
                    return View(auditLog);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.UserAccounts, "UserId", "UserId", auditLog.UserId);
            return View(auditLog);
        }

        // GET: AuditLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditLog = await _context.AuditLogs
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.LogId == id);
            if (auditLog == null)
            {
                return NotFound();
            }

            return View(auditLog);
        }

        // POST: AuditLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(int id) =>
            DbGuard.RunDeleteAsync(this, async () =>
            {
                var auditLog = await _context.AuditLogs.FindAsync(id);
                if (auditLog != null)
                    _context.AuditLogs.Remove(auditLog);
                await _context.SaveChangesAsync();
            });

        private bool AuditLogExists(int id)
        {
            return _context.AuditLogs.Any(e => e.LogId == id);
        }
    }
}
