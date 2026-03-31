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
    [Authorize(Roles = "Маркетолог")]
    public class MarketingCampaignsController : Controller
    {
        private readonly HotelContext _context;

        public MarketingCampaignsController(HotelContext context)
        {
            _context = context;
        }

        // GET: MarketingCampaigns
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.MarketingCampaigns.Include(m => m.RoomCategory);
            return View(await hotelContext.ToListAsync());
        }

        // GET: MarketingCampaigns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketingCampaign = await _context.MarketingCampaigns
                .Include(m => m.RoomCategory)
                .FirstOrDefaultAsync(m => m.CampaignId == id);
            if (marketingCampaign == null)
            {
                return NotFound();
            }

            return View(marketingCampaign);
        }

        // GET: MarketingCampaigns/Create
        public IActionResult Create()
        {
            ViewData["RoomCategoryId"] = new SelectList(_context.RoomCategories, "RoomCategoryId", "RoomCategoryId");
            return View();
        }

        // POST: MarketingCampaigns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomCategoryId,Name,Description,AdjustmentValue,StartDate,EndDate,IsActive")] MarketingCampaign marketingCampaign)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(marketingCampaign);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить акцию. Проверьте категорию номеров и даты.");
                }
            }
            ViewData["RoomCategoryId"] = new SelectList(_context.RoomCategories, "RoomCategoryId", "RoomCategoryId", marketingCampaign.RoomCategoryId);
            return View(marketingCampaign);
        }

        // GET: MarketingCampaigns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketingCampaign = await _context.MarketingCampaigns.FindAsync(id);
            if (marketingCampaign == null)
            {
                return NotFound();
            }
            ViewData["RoomCategoryId"] = new SelectList(_context.RoomCategories, "RoomCategoryId", "RoomCategoryId", marketingCampaign.RoomCategoryId);
            return View(marketingCampaign);
        }

        // POST: MarketingCampaigns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CampaignId,RoomCategoryId,Name,Description,AdjustmentValue,StartDate,EndDate,IsActive")] MarketingCampaign marketingCampaign)
        {
            if (id != marketingCampaign.CampaignId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marketingCampaign);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarketingCampaignExists(marketingCampaign.CampaignId))
                        return NotFound();
                    throw;
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "Не удалось сохранить акцию. Проверьте даты и категорию номеров.");
                }
                if (!ModelState.IsValid)
                {
                    ViewData["RoomCategoryId"] = new SelectList(_context.RoomCategories, "RoomCategoryId", "RoomCategoryId", marketingCampaign.RoomCategoryId);
                    return View(marketingCampaign);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomCategoryId"] = new SelectList(_context.RoomCategories, "RoomCategoryId", "RoomCategoryId", marketingCampaign.RoomCategoryId);
            return View(marketingCampaign);
        }

        // GET: MarketingCampaigns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marketingCampaign = await _context.MarketingCampaigns
                .Include(m => m.RoomCategory)
                .FirstOrDefaultAsync(m => m.CampaignId == id);
            if (marketingCampaign == null)
            {
                return NotFound();
            }

            return View(marketingCampaign);
        }

        // POST: MarketingCampaigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(int id) =>
            DbGuard.RunDeleteAsync(this, async () =>
            {
                var marketingCampaign = await _context.MarketingCampaigns.FindAsync(id);
                if (marketingCampaign != null)
                    _context.MarketingCampaigns.Remove(marketingCampaign);
                await _context.SaveChangesAsync();
            });

        private bool MarketingCampaignExists(int id)
        {
            return _context.MarketingCampaigns.Any(e => e.CampaignId == id);
        }
    }
}
