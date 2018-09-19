using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models.DomainViewModels
{
    public class SalesViewModel
    {
        public string ClientName { get; set; }
        public string Recipe { get; set; }
        public int Amount { get; set; }
    }
}