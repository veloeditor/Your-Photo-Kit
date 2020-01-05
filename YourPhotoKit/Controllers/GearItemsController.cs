using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YourPhotoKit.Data;
using YourPhotoKit.Models;
using YourPhotoKit.Models.GearItems;

namespace YourPhotoKit.Controllers
{
    [Authorize]
    public class GearItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GearItemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: GearItems
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var applicationDbContext = _context.GearItems.Include(g => g.gearType).Include(g => g.User).Where(g => g.ApplicationUserId == user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GearItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var gearItem = await _context.GearItems
                .Include(g => g.User)
                .Include(g => g.gearType)
                .Include(g => g.TripGear)
                .Include(g => g.Trip)
                .FirstOrDefaultAsync(m => m.GearItemId == id);

            

            if (gearItem == null)
            {
                return RedirectToAction(nameof(Index));
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
                if (viewModel.Img != null)
                {
                    var uniqueFileName = GetUniqueFileName(viewModel.Img.FileName);
                    var imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    var filePath = Path.Combine(imageDirectory, uniqueFileName);
                    using (var myFile = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Img.CopyTo(myFile);
                    }
                    viewModel.GearItem.PhotoUrl = uniqueFileName;
                }
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

            var viewModel = new GearItemCreateViewModel()
            {
                GearItem = await _context.GearItems.FindAsync(id),
                GearTypes = await _context.GearType.ToListAsync()
            };

            if (viewModel.GearItem == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: GearItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GearItemCreateViewModel viewModel)
        {
            if (id != viewModel.GearItem.GearItemId)
            {
                return NotFound();
            }

            ModelState.Remove("GearItem.UserId");
            ModelState.Remove("GearItem.User");

            if (ModelState.IsValid)
            {
                try
                {
                    var currentFileName = viewModel.GearItem.PhotoUrl;
                    //This if statement will check to see if there is a photo already on the gear item and the photo replacing it is a new file (with unique name).
                    if (viewModel.Img != null && viewModel.Img.FileName != currentFileName && currentFileName != null)
                    {
                        var user = await GetCurrentUserAsync();
                        viewModel.GearItem.User = user;
                        viewModel.GearItem.ApplicationUserId = user.Id;
                        var images = Directory.GetFiles("wwwroot/images");
                        var fileToDelete = images.First(i => i.Contains(currentFileName));
                        System.IO.File.Delete(fileToDelete);
                        var uniqueFileName = GetUniqueFileName(viewModel.Img.FileName);
                        var imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        var filePath = Path.Combine(imageDirectory, uniqueFileName);
                        using (var myFile = new FileStream(filePath, FileMode.Create))
                        {
                            viewModel.Img.CopyTo(myFile);
                        }
                        viewModel.GearItem.PhotoUrl = uniqueFileName;
                        _context.Update(viewModel.GearItem);
                        await _context.SaveChangesAsync();
                    }
                    //This else if statement allows the user to add a new photo and it will replace the temp image
                    else if (viewModel.GearItem.PhotoUrl == null)
                    {
                        var user = await GetCurrentUserAsync();
                        viewModel.GearItem.User = user;
                        viewModel.GearItem.ApplicationUserId = user.Id;
                        var uniqueFileName = GetUniqueFileName(viewModel.Img.FileName);
                        var imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        var filePath = Path.Combine(imageDirectory, uniqueFileName);
                        using (var myFile = new FileStream(filePath, FileMode.Create))
                        {
                            viewModel.Img.CopyTo(myFile);
                        }
                        viewModel.GearItem.PhotoUrl = uniqueFileName;
                        _context.Update(viewModel.GearItem);
                        await _context.SaveChangesAsync();
                    }
                    //The else statement is a basic edit post with no picture consideration
                    else
                    {
                        var user = await GetCurrentUserAsync();
                        viewModel.GearItem.User = user;
                        viewModel.GearItem.ApplicationUserId = user.Id; 
                        _context.Update(viewModel.GearItem);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GearItemExists(viewModel.GearItem.GearItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var gearId = viewModel.GearItem.GearItemId;
                return RedirectToAction("Details", "GearItems", new { id = gearId });
            }
            return View(viewModel);
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
            //remove GearItemId from TripGear join table
            var tripGearItem = await _context.TripGear.Where(tg => tg.GearItemId == id).ToListAsync();
            _context.TripGear.RemoveRange(tripGearItem);
            await _context.SaveChangesAsync();
            
            var gearItem = await _context.GearItems.FindAsync(id);
            var currentFileName = gearItem.PhotoUrl;
            if (currentFileName != null)
            {
                var images = Directory.GetFiles("wwwroot/images");
                var fileToDelete = images.First(i => i.Contains(currentFileName));
                System.IO.File.Delete(fileToDelete);
            }
            _context.GearItems.Remove(gearItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GearItemExists(int id)
        {
            return _context.GearItems.Any(e => e.GearItemId == id);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
