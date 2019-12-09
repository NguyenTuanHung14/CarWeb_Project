using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarShop.Utilyti;
using CarShop.Models.ViewModel;
using CarShop.Models;
using CarShop.Data;
namespace CarShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductBrandController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductBrandController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.ProductBrands.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductBrand productBrand)
        {
            if (ModelState.IsValid)
            {
                _db.Add(productBrand);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productBrand);
        }

        //Get:Edit
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
                return NotFound();
            var productbrand = await _db.ProductBrands.FindAsync(id);
            if (productbrand == null)
                return NotFound();
            return View(productbrand);
        }
        //Post:Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ProductBrand productBrand)
        {
            if (id != productBrand.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                _db.ProductBrands.Update(productBrand);
                await  _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productBrand);

        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = await _db.ProductBrands.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }


        //GET Delete Action Method
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = await _db.ProductBrands.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        //POST Delete action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var productBrand = await _db.ProductBrands.FindAsync(id);
            _db.ProductBrands.Remove(productBrand);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}