using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models.DomainViewModels
{
    public class OrderViewModel
    {
        public int Quantity { get; set; } // cate pungi de chipsuri se vor crea
        public int BagQuantity { get; set; }  // cate g are punga de chipsuri
    }
}