using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models.DomainViewModels
{
    public class OrderViewModel
    {
        public string Recipe { get; set; } // reteta dorita 
        public int BagSize { get; set; } // cate pungi de chipsuri se vor crea
        public int Amount { get; set; }
        
    }
}