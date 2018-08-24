using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models
{
    public partial class StorageViewModel
    {
        public int ID_warehouse { get; set;}
        public int ID_compartment { get; set;}
        public String feedstockName { get; set; }
        public String quantityStored { get; set; }
        public DateTime newestDateOfSupply { get; set; }
        public DateTime oldestDateOfSupply { get; set; }
        public String quantityFromTheOldestDate { get; set; }
    }

    public partial class StorageViewModel
    {

    }
}