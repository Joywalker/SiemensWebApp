using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models.DomainViewModels
{
    public class OrderViewModel
    {
        public string Recipe { get; set; } // reteta dorita 
        public int Amount { get; set; }
        
    }
}