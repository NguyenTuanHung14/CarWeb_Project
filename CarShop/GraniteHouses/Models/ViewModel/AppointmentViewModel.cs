using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
namespace CarShop.Models.ViewModel
{
    public class AppointmentViewModel
    {
        public List<Appointment> Appointments { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
