using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Data;
using CarShop.Models;
using CarShop.Utilyti;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Areas.Admin.Controllers
{
   [Authorize(Roles =SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminUsersController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.ApplicationUsers.ToList());
        }

        //Get:Edit
        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null || Id.Trim().Length == 0)
            {
                return NotFound();
            }
            var userFormDb = await _db.ApplicationUsers.FindAsync(Id);
            if (userFormDb == null)
                return NotFound();

            return View(userFormDb);
         }

        //Post: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string Id, ApplicationUser applicationUser)
        { 
            if(Id!=applicationUser.Id)
            {

                return NotFound();
            }
            if (ModelState.IsValid)
            {
               ApplicationUser userFormDB = _db.ApplicationUsers.Where(u => u.Id == Id).FirstOrDefault();

                userFormDB.Name = applicationUser.Name;
                userFormDB.PhoneNumber = applicationUser.PhoneNumber;

                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        //Get: Detele

        public async Task<IActionResult> Delete(string Id)
        {
            if (Id == null || Id.Trim().Length == 0)
            {
                return NotFound();
            }
            var userFormDb = await _db.ApplicationUsers.FindAsync(Id);
            if (userFormDb == null)
                return NotFound();

            return View(userFormDb);
        }

        //Post: Edit
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(string Id)
        {
            ApplicationUser userFormDB = _db.ApplicationUsers.Where(u => u.Id == Id).FirstOrDefault();
            userFormDB.LockoutEnd = DateTime.Now.AddYears(1000);    
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}