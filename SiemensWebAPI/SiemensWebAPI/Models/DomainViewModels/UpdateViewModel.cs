using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models.DomainViewModels
{
    public class UpdateViewModel
    {
        public int ID_SW { get; set; } // id depozit sursa
        public int ID_SC { get; set; } // id comp sursa
        public int ID_DW { get; set; } //id depozit destinatar
        public int ID_DC { get; set; } // id comp destinatar 
        public int ID_Material { get; set; }
        public int Quantity { get; set; }
       
    }
}