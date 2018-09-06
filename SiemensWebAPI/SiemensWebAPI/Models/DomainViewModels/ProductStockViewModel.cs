using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models.DomainViewModels
{
    public class ProductStockViewModel
    {
        public string Name { get; set; }
        public int NumberOfBags { get; set; }
        public string Recipe { get; set; }
        public DateTime? ManufactureDate { get; set; }
    }
}