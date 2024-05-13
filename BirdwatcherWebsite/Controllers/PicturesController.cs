using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BirdwatcherWebsite.Data;
using BirdwatcherWebsite.Models;
using Microsoft.AspNetCore.Http;

namespace BirdwatcherWebsite.Controllers
{
    public class PicturesController : Controller
    {
        private readonly BirdwatcherWebsiteContext _context;

        IWebHostEnvironment _env;

        public PicturesController(BirdwatcherWebsiteContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Pictures
        public async Task<IActionResult> Index()
        {
            return View(await _context.Picture.ToListAsync());
        }

        // GET: Pictures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture
                .FirstOrDefaultAsync(m => m.Id == id);
            if (picture == null)
            {
                return NotFound();
            }

            return View(picture);
        }

        // GET: Pictures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,DateTimeImgTaken,BirdType")] Picture picture, IFormFile formFile)
        {
            string file;

                if (formFile != null && formFile.Length > 0)
                {
                    // Make a unique image name
                    string name = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-") + formFile.FileName;

                     file = _env.ContentRootPath + "\\wwwroot\\Photos\\" + name;

                    picture.ImagePath = file;

                using (FileStream fileStream = new FileStream(file, FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }


            }

            

            if (ModelState.IsValid)
            {
                _context.Add(picture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(picture);
        }

        // GET: Pictures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }
            return View(picture);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DateTimeImgTaken,BirdType,ImagePath,Description")] Picture picture)
        {
            if (id != picture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(picture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PictureExists(picture.Id))
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
            return View(picture);
        }

        // GET: Pictures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture
                .FirstOrDefaultAsync(m => m.Id == id);
            if (picture == null)
            {
                return NotFound();
            }

            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var picture = await _context.Picture.FindAsync(id);
            if (picture != null)
            {
                // Try to delete the picture file from the filesystem
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", Path.GetFileName(picture.ImagePath));
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        // Log the error or handle it accordingly
                        // Consider whether to notify the user or handle silently
                        Console.WriteLine("An error occurred while attempting to delete the file: " + ex.Message);
                    }
                }


                _context.Picture.Remove(picture);
            }

            // delete picture from wwwroot/Photos


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PictureExists(int id)
        {
            return _context.Picture.Any(e => e.Id == id);
        }
    }
}
