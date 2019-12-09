﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YourPhotoKit.Data;
using YourPhotoKit.Models;
using YourPhotoKit.Models.GearItems;

namespace YourPhotoKit.Controllers
{
    public class GearItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GearItemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: GearItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GearItems.Include(g => g.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GearItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gearItem = await _context.GearItems
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GearItemId == id);
            if (gearItem == null)
            {
                return NotFound();
            }

            return View(gearItem);
        }

        // GET: GearItems/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new GearItemCreateViewModel()
            {
                GearTypes = await _context.GearType.ToListAsync()
            };
            return View(viewModel);
        }

        // POST: GearItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GearItemCreateViewModel viewModel)
        {
            ModelState.Remove("GearItem.UserId");
            ModelState.Remove("GearItem.User");
            
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                viewModel.GearItem.ApplicationUserId = user.Id;
                _context.Add(viewModel.GearItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", viewModel.GearItem.ApplicationUserId);
            return View(viewModel.GearItem);
        }

        // GET: GearItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gearItem = await _context.GearItems.FindAsync(id);
            if (gearItem == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", gearItem.ApplicationUserId);
            return View(gearItem);
        }

        // POST: GearItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GearItemId,Title,Description,GearTypeId,Cost,DatePurchased,SerialNumber,PhotoUrl,ApplicationUserId")] GearItem gearItem)
        {
            if (id != gearItem.GearItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gearItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GearItemExists(gearItem.GearItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", gearItem.ApplicationUserId);
            return View(gearItem);
        }

        // GET: GearItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gearItem = await _context.GearItems
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GearItemId == id);
            if (gearItem == null)
            {
                return NotFound();
            }

            return View(gearItem);
        }

        // POST: GearItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gearItem = await _context.GearItems.FindAsync(id);
            _context.GearItems.Remove(gearItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GearItemExists(int id)
        {
            return _context.GearItems.Any(e => e.GearItemId == id);
        }
    }
}
