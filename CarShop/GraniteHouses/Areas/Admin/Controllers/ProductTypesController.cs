using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarShop.Data;
using CarShop.Models;
using Microsoft.AspNetCore.Authorization;
using CarShop.Utilyti;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Areas.Admin.Controllers
{
   [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
      
        private readonly ApplicationDbContext _db;
        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var brand = _db.ProductTypes.Include(d => d.ProductBrand);
            return View(await brand.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["ProductBrandId"] = new SelectList(_db.ProductBrands, "Id", "Name");
            return View();
        }
        //post create action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid) {
                _db.Add(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductBrandId"] = new SelectList(_db.ProductBrands, "Id", "Name",productTypes.ProductBrandId);
            return View(productTypes);
        }

        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = await _db.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            ViewData["ProductBrandId"] = new SelectList(_db.ProductBrands, "Id", "Name");
            return View(productType);
        }

        //POST Edit action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductTypes productTypes)
        {
            if (id != productTypes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductBrandId"] = new SelectList(_db.ProductBrands, "Id", "Name", productTypes.ProductBrandId);
            return View(productTypes);
        }


        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = await _db.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            ViewData["ProductBrandId"] = new SelectList(_db.ProductBrands, "Id", "Name");
            return View(productType);
        }


        //GET Delete Action Method
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = await _db.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            ViewData["ProductBrandId"] = new SelectList(_db.ProductBrands, "Id", "Name");
            return View(productType);
        }

        //POST Delete action Method
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var productType = await _db.ProductTypes.FindAsync(id);
            _db.ProductTypes.Remove(productType);
           
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
       
        }

    }
}