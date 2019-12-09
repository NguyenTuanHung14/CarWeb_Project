using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
namespace CarShop.Models.ViewModel
{
    public class ApointmentDetailsViewModel
    {
        public Appointment Appointment { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public List<Product> Products { get; set; }
    }
}
