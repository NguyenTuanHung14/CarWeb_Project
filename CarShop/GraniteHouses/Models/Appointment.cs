﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        [Display(Name = ("Sales Person"))]
        public string SalesPersonId { get; set; }
        [ForeignKey("SalesPersonId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public DateTime AppointmentDate { get; set; }
        [NotMapped] //ko ánh xạ(không kết nối)
        public DateTime AppointmentTime { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public bool isConfirmed { get; set; }
    }
}
