using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models
{
    public class SupplyModel
    {
        public int IDRawMaterial { get; set; }
        public DateTime DateOfResupply { get; set; }
        public int Quantity { get; set;}
        
    }
}