using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models
{
    public class BasicInformation
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public string InteriorColor { get; set; }
        public string NumberOfDoors{ get; set; }
        public string SeatNumber { get; set; }
        [Display(Name="Product Name")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
