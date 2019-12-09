using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarShop.Data;
using CarShop.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CarShop.Utilyti;
using CarShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace CarShop.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnviroment;
        public ProductsController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnviroment = hostingEnvironment;
            ProductsVM = new ProductViewModel()
            {
                ProductTypes = _db.ProductTypes.ToList(),
                SpectialTags = _db.SpectialTags.ToList(),
                Product = new Models.Product()
            };
        }
        [BindProperty]
        public ProductViewModel ProductsVM { get; set; }       
        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(m => m.ProductTypes).Include(m => m.SpectialTag).Include(m=>m.ProductTypes.ProductBrand);
            return View(await products.ToListAsync());
        }


        //Get: product create
        public IActionResult Create()
        {
            return View(ProductsVM);    
        }

        //Post:Product Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
            {
                return View(ProductsVM);
            }

            _db.Products.Add(ProductsVM.Product);
            await _db.SaveChangesAsync();

            //Image being saved

            string webRootPath = _hostingEnviroment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var productsFromDb = _db.Products.Find(ProductsVM.Product.Id);

            if (files.Count != 0)
            {
                //Image has been uploaded
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Product.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Product.Id + extension;
            }
            else
            {
                //when user does not upload image
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Product.Id + ".png");
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Product.Id + ".png";
            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));



        }
        //GET: EDIT
        public async Task<IActionResult> Edit(int? id)
        { 
            if(id==null)
            {
                return NotFound();
             }
            ProductsVM.Product = await _db.Products.Include(m => m.SpectialTag).Include(m => m.ProductTypes).SingleOrDefaultAsync(m=>m.Id==id);
            if (ProductsVM.Product == null)
            {
                return NotFound();
            }
            return View(ProductsVM);
        }


        //Post: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnviroment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var productFromDb = _db.Products.Where(m => m.Id == ProductsVM.Product.Id).FirstOrDefault();

                if (files.Count > 0 && files[0] != null)
                {
                    //if user uploads a new image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(productFromDb.Image);

                    if (System.IO.File.Exists(Path.Combine(uploads, ProductsVM.Product.Id + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, ProductsVM.Product.Id + extension_old));
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Product.Id + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    ProductsVM.Product.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Product.Id + extension_new;
                }

                if (ProductsVM.Product.Image != null)
                {
                    productFromDb.Image = ProductsVM.Product.Image;
                }

                productFromDb.Name = ProductsVM.Product.Name;
                productFromDb.Price = ProductsVM.Product.Price;
                productFromDb.Available = ProductsVM.Product.Available;
                productFromDb.ProductTypeID = ProductsVM.Product.ProductTypeID;
                productFromDb.SpectialTagID = ProductsVM.Product.SpectialTagID;
                productFromDb.ShadeColor = ProductsVM.Product.ShadeColor;
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ProductsVM);
        }
        //GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ProductsVM.Product = await _db.Products.Include(m => m.SpectialTag).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductsVM.Product == null)
            {
                return NotFound();
            }
            return View(ProductsVM);
        }
        //GET:Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ProductsVM.Product = await _db.Products.Include(m => m.SpectialTag).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (ProductsVM.Product == null)
            {
                return NotFound();
            }
            return View(ProductsVM);
        }
        //POST: Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnviroment.WebRootPath;
            Product product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            else
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(product.Image);
                if (System.IO.File.Exists(Path.Combine(uploads, product.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, product.Id + extension));
                }
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}