using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using YourPhotoKit.Data;
using YourPhotoKit.Models;
using YourPhotoKit.Models.TripGearViews;
using YourPhotoKit.Models.TripModels;

namespace YourPhotoKit.Controllers
{
    [Authorize]
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TripsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var applicationDbContext = _context.Trips.Include(t => t.User).Where(t => t.ApplicationUserId == user.Id).OrderBy(t => t.StartDate);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: List of Gear for a Trip
        public async Task<IActionResult> TripGearIndex(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var pickedItems = await _context.TripGear.Include(tg => tg.GearItem).Where(tg => tg.TripId == id).ToListAsync();
            //var gearItems = await _context.GearItems.Where(g => g.User == user).ToListAsync();
            var gearItems = await _context.GearItems.Include(gi => gi.TripGear).Where(gi => gi.User == user && !gi.TripGear.Any(tg => tg.TripId == id)).ToListAsync();

            var trip = await _context.Trips
                 .Include(t => t.User)
                 .Include(t => t.GearItems)
                 .FirstOrDefaultAsync(m => m.TripId == id);

            var viewModel = new AddGearToTripViewModel()
            {
                
                Trip = trip,
                GearItems = gearItems,
                PickedItems = pickedItems
                 
        };

            return View(viewModel);
        }


        // GET: Trips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));

            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var pickedItems = await _context.TripGear.Include(tg => tg.GearItem).Where(tg => tg.TripId == id).ToListAsync();
            //var gearItems = await _context.GearItems.Where(g => g.User == user).ToListAsync();
            var gearItems = await _context.GearItems.Include(gi => gi.TripGear).Where(gi => gi.User == user && !gi.TripGear.Any()).ToListAsync();
            
            var trip = await _context.Trips
                .Include(t => t.User)
                .Include(t => t.TripGear)
                .FirstOrDefaultAsync(m => m.TripId == id);

            var viewModel = new AddTripGearViewModel()
            {

                Trip = trip,
                GearItems = gearItems,
                PickedItems = pickedItems

            };

            

            if (trip == null)
            {
                return RedirectToAction(nameof(Index));

            }

            return View(viewModel);
        }

        // GET: Trips/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateandEditTripViewModel()
            {
                GearItems = await _context.GearItems.ToListAsync()
                
                
            };
            return View(viewModel);
        }

        // POST: Trips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateandEditTripViewModel viewModel)
        {
            ModelState.Remove("Trip.UserId");
            ModelState.Remove("Trip.User");
            
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
                    viewModel.Trip.PhotoUrl = uniqueFileName;
                }
            
                var user = await GetCurrentUserAsync();
                viewModel.Trip.User = user;
                viewModel.Trip.ApplicationUserId = user.Id;

                _context.Add(viewModel.Trip);
                 await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", viewModel.Trip.ApplicationUserId);
            _context.Add(viewModel.TripGear);
            await _context.SaveChangesAsync();

            return View(viewModel.Trip);
        }

        //Add gear to a tripgear join table
       public async Task<IActionResult> AddTripGear(AddGearToTripViewModel viewModel)
        {

            var tripId = viewModel.Trip.TripId;
            var gearItemId = viewModel.GearItemId;
            var existingTripGear = await _context.TripGear.FirstOrDefaultAsync(tg => tg.TripId == tripId && tg.GearItemId == gearItemId && tg.IsPacked);
            if (existingTripGear == null)
            {
                var tripGear = new TripGear
                {
                    GearItemId = gearItemId,
                    TripId = tripId,
                    IsPacked = true
                };
                _context.TripGear.Add(tripGear);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(TripGearIndex), new { id = tripId });
            }
            else
            {
                var successMsg = TempData["notice"] as string;
                TempData["notice"] = $"You already added this gear.";
                return RedirectToAction(nameof(TripGearIndex), new { id = tripId });
            }
        }

        //Remove gear to a tripgear join table
        public async Task<IActionResult> RemoveTripGear(AddGearToTripViewModel viewModel)
        {

            var tripId = viewModel.Trip.TripId;
            var gearItemId = viewModel.GearItemId;

            var tripGear = await _context.TripGear.FirstOrDefaultAsync(tg => tg.TripId == tripId && tg.GearItemId == gearItemId && tg.IsPacked);

            _context.TripGear.Remove(tripGear);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(TripGearIndex), new { id = tripId });

        }


        // GET: Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @trip = await _context.Trips.FindAsync(id);
        

            var viewModel = new CreateandEditTripViewModel()
            {
                Trip = @trip,
                
            };

          


            if (@trip == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateandEditTripViewModel viewModel)
        {

            if (id != viewModel.Trip.TripId)
            {
                return NotFound();
            }

            ModelState.Remove("Trip.UserId");
            ModelState.Remove("Trip.User");

            if (ModelState.IsValid)
            {
                try
                {
                    var currentFileName = viewModel.Trip.PhotoUrl;
                    var user = await GetCurrentUserAsync();
                    var tripPhoto = await _context.TripPhotos.ToListAsync();
                    if (viewModel.Img != null && viewModel.Img.FileName != currentFileName)
                    {
                        if (currentFileName != null)
                        {
                            
                            viewModel.Trip.User = user;
                            viewModel.Trip.ApplicationUserId = user.Id;
                           
                            var images = Directory.GetFiles("wwwroot/images");
                            var fileToDelete = images.First(i => i.Contains(currentFileName));
                            System.IO.File.Delete(fileToDelete);
                        }
                        var uniqueFileName = GetUniqueFileName(viewModel.Img.FileName);
                        var imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        var filePath = Path.Combine(imageDirectory, uniqueFileName);
                        using (var myFile = new FileStream(filePath, FileMode.Create))
                        {
                            viewModel.Img.CopyTo(myFile);
                        }
                        viewModel.Trip.PhotoUrl = uniqueFileName;
                    }
                   
                    viewModel.Trip.User = user;
                    viewModel.Trip.ApplicationUserId = user.Id;

                    _context.Update(viewModel.Trip);
                    await _context.SaveChangesAsync();

                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(viewModel.Trip.TripId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var tripId = viewModel.Trip.TripId;
                return RedirectToAction("Details", "Trips", new { id = tripId });
            }
            return View(viewModel);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TripId == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //remove TripId from TripGear join table
            var tripGearTrip = await _context.TripGear.Where(tg => tg.TripId == id).ToListAsync();
            _context.TripGear.RemoveRange(tripGearTrip);
            await _context.SaveChangesAsync();

            var trip = await _context.Trips.FindAsync(id);
            var currentFileName = trip.PhotoUrl;
            if (currentFileName != null)
            {
                var images = Directory.GetFiles("wwwroot/images");
                var fileToDelete = images.First(i => i.Contains(currentFileName));
                System.IO.File.Delete(fileToDelete);
            }
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.TripId == id);
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
