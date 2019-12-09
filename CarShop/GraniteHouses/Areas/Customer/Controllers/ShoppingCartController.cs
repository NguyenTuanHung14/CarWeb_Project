using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Data;
using Microsoft.AspNetCore.Mvc;
using CarShop.Models.ViewModel;
using CarShop.Extensions;
using CarShop.Models;
using Microsoft.EntityFrameworkCore;
using CarShop.Utilyti;
namespace CarShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Products = new List<Models.Product>()
            };
        }
        //Get Index Shopping Cart
        public async Task<IActionResult> Index()
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if (lstShoppingCart == null)
            {
                SD.flag = false;
                return View(nameof(Index));
            }
            if (lstShoppingCart.Count > 0)
            {
                SD.flag = true;
                foreach (int cartItem in lstShoppingCart)
                {
                    Product prod = _db.Products.Include(p => p.SpectialTag).Include(p => p.ProductTypes).Where(p => p.Id == cartItem).FirstOrDefault();
                    ShoppingCartVM.Products.Add(prod);
                }
            }
          
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>> ("ssShoppingCart");
            ShoppingCartVM.Appointment.AppointmentDate = ShoppingCartVM.Appointment.AppointmentDate
                                                                     .AddHours(ShoppingCartVM.Appointment.AppointmentTime.Hour)
                                                                     .AddMinutes(ShoppingCartVM.Appointment.AppointmentTime.Minute);
            Appointment appointment = ShoppingCartVM.Appointment;
            _db.Appointments.Add(appointment);
            _db.SaveChanges();
            int appointmentId = appointment.Id;
            foreach (int productId in lstCartItems)
            {
                ProductsSelectedForAppointment productsSelectedForAppointment = new ProductsSelectedForAppointment()
                {
                    AppointmentId = appointmentId,
                    ProductId = productId
                };
                _db.ProductsSelectedForAppointments.Add(productsSelectedForAppointment);
            }
            _db.SaveChanges();
            lstCartItems = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);
            return RedirectToAction("AppointmentConfirmation","ShoppingCart", new { Id = appointmentId});
        }

        //Remove

        public IActionResult Remove(int id)
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if (lstCartItems.Count > 0)
            {
                if (lstCartItems.Contains(id))
                {
                    lstCartItems.Remove(id);
                }
            }
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AppointmentConfirmation(int id)
        {
            ShoppingCartVM.Appointment = _db.Appointments.Where(a => a.Id == id).FirstOrDefault();
            List<ProductsSelectedForAppointment> objProdList = _db.ProductsSelectedForAppointments.Where(p => p.AppointmentId == id).ToList();

            foreach (var prodApObj in objProdList)
            {
                ShoppingCartVM.Products.Add(_db.Products.Include(p => p.SpectialTag).Where(p => p.Id == prodApObj.ProductId).FirstOrDefault());
            }
            return View(ShoppingCartVM);
        }
    }


}