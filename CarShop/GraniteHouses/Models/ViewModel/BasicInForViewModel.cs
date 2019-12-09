using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models.ViewModel
{
    public class BasicInForViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public BasicInformation BasicInformation { get; set; }
    }
}
