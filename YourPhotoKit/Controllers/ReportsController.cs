﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YourPhotoKit.Data;
using YourPhotoKit.Models;
using IronPdf;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace YourPhotoKit.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private IWebHostEnvironment _env;


        public ReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _env = env;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Reports
        public async Task<IActionResult> Index(string SearchString)
        {
            var user = await GetCurrentUserAsync();
            var searchTerm = SearchString;
            ViewBag.value = searchTerm;

            if (SearchString == null)
            {
                var applicationDbContext = _context.GearItems.OrderByDescending(g => g.DatePurchased).Include(g => g.User).Where(g => g.ApplicationUserId == user.Id);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var searchTerms = SearchString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                IQueryable<GearItem> searchMatch = _context.GearItems.OrderByDescending(g => g.DatePurchased).Include(g => g.User).Where(g => g.ApplicationUserId == user.Id);

                foreach (var term in searchTerms)
                {
                    searchMatch = searchMatch.Where(x => x.Title.Contains(term) || x.gearType.Label.Contains(term) ||x.DatePurchased.Year.ToString().Contains(term));
                }

                return View(searchMatch.ToList());

            }
        }


        //Insurance List
        public async Task<IActionResult> AllGearReport()
        {
            var user = await GetCurrentUserAsync();
            ViewData["UserId"] = user.Id;
          

            var allGearSum = _context.GearItems
                .Include(gi => gi.User)
                .Select(col => col.Cost).Sum();

            return View();
            

        }


        // GET: Reports/Details/5
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

        // GET: Reports/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GearItemId,Title,Description,GearTypeId,Cost,DatePurchased,SerialNumber,PhotoUrl,ApplicationUserId")] GearItem gearItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gearItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", gearItem.ApplicationUserId);
            return View(gearItem);
        }

        // GET: Reports/Edit/5
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

        // POST: Reports/Edit/5
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

        // GET: Reports/Delete/5
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

        // POST: Reports/Delete/5
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
