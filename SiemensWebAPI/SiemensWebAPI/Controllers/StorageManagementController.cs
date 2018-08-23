using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiemensWebAPI.Models;

namespace SiemensWebAPI.Controllers
{
    public class StorageManagementController : ApiController
    {
        [Route("api/Storage")]
        [HttpGet]
        public IHttpActionResult getStorageStatus()
        {
            using(DatabaseContext dbctx = new DatabaseContext())
            {
                try
                {
                    var entryPoint = (from warehouse in dbctx.Warehouses
                                      join supply in dbctx.Supplies on warehouse.ID_compartment equals supply.ID_compartment
                                      join feedstock in dbctx.Feedstocks on warehouse.ID_feedstock equals feedstock.ID
                                      select new
                                      {
                                          ID_wh = warehouse.ID_warehouse,
                                          ID_cmp = warehouse.ID_compartment,
                                          FS_name = feedstock.Name,
                                          SupplyString = supply.ID_supply
                                      }).ToList();
                    return Ok(entryPoint);
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.ToString());
                    return NotFound();
                }
            }
        }
    }
}
