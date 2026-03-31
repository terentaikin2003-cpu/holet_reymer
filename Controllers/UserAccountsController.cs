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
    public class UserAccountsController : Controller
    {
        private readonly HotelContext _context;

        public UserAccountsController(HotelContext context)
        {
            _context = context;
        }

        // GET: UserAccounts
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.UserAccounts.Include(u => u.Role);
            return View(await hotelContext.ToListAsync());
        }

        // GET: UserAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // GET: UserAccounts/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View();
        }

        // POST: UserAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,Login,Hash,FullName,Phone,Email")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(userAccount);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить пользователя. Проверьте роль и уникальность логина.");
                }
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", userAccount.RoleId);
            return View(userAccount);
        }

        // GET: UserAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts.FindAsync(id);
            if (userAccount == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", userAccount.RoleId);
            return View(userAccount);
        }

        // POST: UserAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,RoleId,Login,Hash,FullName,Phone,Email")] UserAccount userAccount)
        {
            if (id != userAccount.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccountExists(userAccount.UserId))
                        return NotFound();
                    throw;
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить пользователя. Проверьте роль и уникальность логина.");
                }
                if (!ModelState.IsValid)
                {
                    ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", userAccount.RoleId);
                    return View(userAccount);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", userAccount.RoleId);
            return View(userAccount);
        }

        // GET: UserAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(int id) =>
            DbGuard.RunDeleteAsync(this, async () =>
            {
                var userAccount = await _context.UserAccounts.FindAsync(id);
                if (userAccount != null)
                    _context.UserAccounts.Remove(userAccount);
                await _context.SaveChangesAsync();
            });

        private bool UserAccountExists(int id)
        {
            return _context.UserAccounts.Any(e => e.UserId == id);
        }
    }
}
