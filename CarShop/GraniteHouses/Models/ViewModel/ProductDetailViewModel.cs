using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models.ViewModel
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public Specification Specification { get; set; }
        public BasicInformation BasicInformation { get; set; }
    }
}
