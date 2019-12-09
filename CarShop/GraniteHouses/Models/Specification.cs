using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
namespace CarShop.Models
{
    public class Specification
    {
        public int Id { get; set; }
        public string Weight { get; set; }
        public string Engine { get; set; }
        public string CylinderCapacity { get; set; }
        public  string Brake { get; set; }
        public string  Wheels  { get; set; }
        [Display(Name = ("Product Name"))]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
