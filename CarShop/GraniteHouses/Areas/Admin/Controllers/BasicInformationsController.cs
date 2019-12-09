using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShop.Data;
using CarShop.Models;
using CarShop.Models.ViewModel;
namespace CarShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BasicInformationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public BasicInForViewModel BasicInForVM { get; set; }
        public BasicInformationsController(ApplicationDbContext context)
        {
            _context = context;
            BasicInForVM = new BasicInForViewModel()
            {
                BasicInformation = new Models.BasicInformation(),
                Products = _context.Products.ToList()
            };
        }

    
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BasicInformations.Include(b => b.Product);
            return View(await applicationDbContext.ToListAsync());
        }

      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicInformation = await _context.BasicInformations
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basicInformation == null)
            {
                return NotFound();
            }

            return View(basicInformation);
        }

        public IActionResult Create()
        {
           
            return View(BasicInForVM);
        }

        
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            if (ModelState.IsValid)
            {
                _context.BasicInformations.Add(BasicInForVM.BasicInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(BasicInForVM);
        }

     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BasicInForVM.BasicInformation = await _context.BasicInformations.Include(m => m.Product).SingleOrDefaultAsync(m => m.Id == id);
            if (BasicInForVM == null)
            {
                return NotFound();
            }
          
            return View(BasicInForVM);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != BasicInForVM.BasicInformation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var BasicInfoFormDB = await _context.BasicInformations.Where(m => m.Id == BasicInForVM.BasicInformation.Id).SingleOrDefaultAsync();
                BasicInfoFormDB.InteriorColor = BasicInForVM.BasicInformation.InteriorColor;
                BasicInfoFormDB.NumberOfDoors = BasicInForVM.BasicInformation.NumberOfDoors;
                BasicInfoFormDB.SeatNumber = BasicInForVM.BasicInformation.SeatNumber;
                BasicInfoFormDB.Source = BasicInForVM.BasicInformation.Source;
                BasicInfoFormDB.Status = BasicInForVM.BasicInformation.Status;
                BasicInfoFormDB.ProductId = BasicInForVM.BasicInformation.ProductId;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(BasicInForVM);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicInformation = await _context.BasicInformations
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basicInformation == null)
            {
                return NotFound();
            }

            return View(basicInformation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var basicInformation = await _context.BasicInformations.FindAsync(id);
            _context.BasicInformations.Remove(basicInformation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BasicInformationExists(int id)
        {
            return _context.BasicInformations.Any(e => e.Id == id);
        }
    }
}
