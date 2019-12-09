using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarShop.Utilyti;
using CarShop.Models.ViewModel;
using System.Security.Claims;
using CarShop.Data;
using Microsoft.EntityFrameworkCore;
using CarShop.Models;
using System.Text;

namespace CarShop.Areas.Customer
{
    [Authorize(Roles =SD.AdminEndUser+","+SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private int PageSize = 3;
        public AppointmentsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int productPage =1,string searchName =null,string searchEmail = null,string searchPhone = null,string searchDate = null)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User; //tạo cookie nắm giữ thông tin người dùng
            var claimsIdenty = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdenty.FindFirst(ClaimTypes.NameIdentifier);

            AppointmentViewModel appointmentVM = new AppointmentViewModel()
            {
                Appointments = new List<Models.Appointment>()
            };
            StringBuilder param = new StringBuilder();
            param.Append("/Admin/Appointments?productPage=:");
            param.Append("&searchName");
            if (searchName != null)
            {
                param.Append(searchName);
            }
            param.Append("&searchPhone");
            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }
            param.Append("&searchEmail");
            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }
            param.Append("&searchDate");
            if (searchDate != null)
            {
                param.Append(searchDate);
            }
           
            appointmentVM.Appointments = _db.Appointments.Include(a => a.ApplicationUser).ToList();
            if (User.IsInRole(SD.AdminEndUser))
            {
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.SalesPersonId == claim.Value).ToList();
            }
            if (searchName!=null)
            {
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.CustomerName.ToLower().Contains(searchName.ToLower())).ToList();
            }
            if(searchPhone!=null)
            { 
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.CustomerPhoneNumber.ToLower().Contains(searchPhone.ToLower())).ToList();
            }
            if (searchEmail != null)
            {
                appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.CustomerEmail.ToLower().Contains(searchEmail.ToLower())).ToList();
            }
            if (searchDate != null)
            { 
                try {

                    DateTime appDate = Convert.ToDateTime(searchDate);
                    appointmentVM.Appointments = appointmentVM.Appointments.Where(a => a.AppointmentDate.ToShortDateString().Equals(appDate.ToShortDateString())).ToList();
                        }
                catch (Exception ex)
                {
                    
                }
            }
            var count = appointmentVM.Appointments.Count();
            appointmentVM.Appointments = appointmentVM.Appointments.OrderBy(p => p.AppointmentDate)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();

            appointmentVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };

            return View(appointmentVM);
        }
        //Get: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var productList = (IEnumerable<Product>)(from p in _db.Products
                                                     join a in _db.ProductsSelectedForAppointments
                                                     on p.Id equals a.ProductId
                                                      where a.AppointmentId == id
                                                     select p).Include("ProductTypes");
            ApointmentDetailsViewModel objAppointmentVM = new ApointmentDetailsViewModel
            {
                Appointment = _db.Appointments.Include(a => a.ApplicationUser).Where(a => a.Id == id).FirstOrDefault(),
                ApplicationUsers = _db.ApplicationUsers.ToList(),
                Products = productList.ToList()
     
            };
            return View(objAppointmentVM);
        }
        //Post:edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(int id,ApointmentDetailsViewModel objAppointment)
        {
            if (ModelState.IsValid)
            {
                objAppointment.Appointment.AppointmentDate = objAppointment.Appointment.AppointmentDate
                    .AddHours(objAppointment.Appointment.AppointmentTime.Hour)
                    .AddMinutes(objAppointment.Appointment.AppointmentTime.Minute);
                var appointmentFromDB = _db.Appointments.Where(a => a.Id == id).FirstOrDefault();
                appointmentFromDB.CustomerName = objAppointment.Appointment.CustomerName;
                appointmentFromDB.CustomerEmail = objAppointment.Appointment.CustomerEmail;
                appointmentFromDB.CustomerPhoneNumber = objAppointment.Appointment.CustomerPhoneNumber;
                appointmentFromDB.AppointmentDate = objAppointment.Appointment.AppointmentDate;
                appointmentFromDB.isConfirmed = objAppointment.Appointment.isConfirmed;
                if (User.IsInRole(SD.SuperAdminEndUser))
                {
                    appointmentFromDB.SalesPersonId = objAppointment.Appointment.SalesPersonId;
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(objAppointment);
        }
        //Get:Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var productList = (IEnumerable<Product>)(from p in _db.Products
                                                     join a in _db.ProductsSelectedForAppointments
                                                     on p.Id equals a.ProductId
                                                     where a.AppointmentId == id
                                                     select p).Include("ProductTypes");
            ApointmentDetailsViewModel objAppointmentVM = new ApointmentDetailsViewModel
            {
                Appointment = _db.Appointments.Include(a => a.ApplicationUser).Where(a => a.Id == id).FirstOrDefault(),
                ApplicationUsers = _db.ApplicationUsers.ToList(),
                Products = productList.ToList()

            };
            return View(objAppointmentVM);
        }
    }
}   