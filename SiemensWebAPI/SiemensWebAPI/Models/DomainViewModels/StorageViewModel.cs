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
        public String FeedstockName { get; set; }
        public int QuantityStored { get; set; }
        public DateTime? NewestDateOfSupply { get; set; }
        public DateTime? OldestDateOfSupply { get; set; }
        public String QuantityFromTheOldestDate { get; set; }
    }

    public partial class StorageViewModel
    {

    }
}