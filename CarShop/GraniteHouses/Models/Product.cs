using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price{ get; set; }
        public bool Available { get; set; }
        public string Image { get; set; }
        public string ShadeColor { get; set; }
        [Display(Name = "Product Type")]
        public int  ProductTypeID { get; set; }

        [ForeignKey("ProductTypeID")]
        public virtual ProductTypes ProductTypes { get; set; }

        [Display(Name = "Spectial Tag")]
        public int SpectialTagID { get; set; }

        [ForeignKey("SpectialTagID")]
        public virtual SpectialTag SpectialTag { get; set; }
    }
}
