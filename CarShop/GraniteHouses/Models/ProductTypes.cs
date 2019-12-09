using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models
{
    public class ProductTypes
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Display(Name="Product Brand")]
        public int ProductBrandId { get; set; }
        public virtual ProductBrand ProductBrand { get; set; }

    }
}
