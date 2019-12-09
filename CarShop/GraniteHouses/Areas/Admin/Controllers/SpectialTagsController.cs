using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShop.Data;
using CarShop.Models;
using Microsoft.AspNetCore.Authorization;
using CarShop.Utilyti;

namespace CarShop.Areas.Admin.Controllers
{
   [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class SpectialTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpectialTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SpectialTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.SpectialTags.ToListAsync());
        }

        // GET: Admin/SpectialTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spectialTag = await _context.SpectialTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spectialTag == null)
            {
                return NotFound();
            }

            return View(spectialTag);
        }

        // GET: Admin/SpectialTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SpectialTags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpectialTagID,Name")] SpectialTag spectialTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spectialTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spectialTag);
        }

        // GET: Admin/SpectialTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spectialTag = await _context.SpectialTags.FindAsync(id);
            if (spectialTag == null)
            {
                return NotFound();
            }
            return View(spectialTag);
        }

        // POST: Admin/SpectialTags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpectialTagID,Name")] SpectialTag spectialTag)
        {
            if (id != spectialTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spectialTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpectialTagExists(spectialTag.Id))
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
            return View(spectialTag);
        }

        // GET: Admin/SpectialTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spectialTag = await _context.SpectialTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spectialTag == null)
            {
                return NotFound();
            }

            return View(spectialTag);
        }

        // POST: Admin/SpectialTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spectialTag = await _context.SpectialTags.FindAsync(id);
            _context.SpectialTags.Remove(spectialTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpectialTagExists(int id)
        {
            return _context.SpectialTags.Any(e => e.Id == id);
        }
    }
}
