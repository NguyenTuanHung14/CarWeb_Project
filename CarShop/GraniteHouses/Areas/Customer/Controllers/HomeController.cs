using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarShop.Models;
using CarShop.Data;
using Microsoft.EntityFrameworkCore;
using CarShop.Extensions;
using CarShop.Models.ViewModel;
namespace CarShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
    
        private readonly ApplicationDbContext _db;
        [BindProperty]
        private ProductDetailViewModel ProductDetailVM { get; set; }
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
            ProductDetailVM = new ProductDetailViewModel()
            { 
                Product = new Models.Product(),
                Specification = new Models.Specification()
            };
        }
     
        public async Task<IActionResult> Index()
        {
           
            var productList = await _db.Products.Include(m => m.ProductTypes).Include(m => m.SpectialTag).ToListAsync();
            return View(productList);
        }
        //Get: Details
        public async Task<IActionResult> Details(int id)
        {
           ProductDetailVM.Specification = await _db.Specifications.Include(m=>m.Product).Where(m => m.ProductId == id).FirstOrDefaultAsync();
            ProductDetailVM.Product = await _db.Products.Include(m => m.ProductTypes).Include(m => m.SpectialTag).Where(m => m.Id == id).FirstOrDefaultAsync();
            return View(ProductDetailVM);
        }
        //Post:Details
        [HttpPost,ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if (lstShoppingCart == null)
            {
                lstShoppingCart = new List<int>();
            }
            lstShoppingCart.Add(id);
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);

            return RedirectToAction("Index", "Home", new { area = "Customer" });

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Remove(int id)
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(lstShoppingCart.Count>0)
            {
                if (lstShoppingCart.Contains(id))
                {
                    lstShoppingCart.Remove(id);
                }
              
            }
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            return RedirectToAction(nameof(Index));

        }
    }
}
