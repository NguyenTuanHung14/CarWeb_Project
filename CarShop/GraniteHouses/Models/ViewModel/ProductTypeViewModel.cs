using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models.ViewModel
{
    public class ProductTypeViewModel
    {
        public ProductTypes ProductTypes { get; set; }
        public IEnumerable<ProductBrand> ProductBrands { get; set; }
    }
}
