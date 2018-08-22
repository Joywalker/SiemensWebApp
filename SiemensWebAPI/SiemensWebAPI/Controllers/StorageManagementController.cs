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
                    var entryPoint = (from warehouse in dbctx.WarehouseStorages
                                      join fs in dbctx.Feedstocks on warehouse.ID_feedstock equals fs.ID
                                      join rs in dbctx.Ressuplies on new { warehouse.ID_warehouse, warehouse.ID_compartment } equals new { rs.ID_warehouse, rs.ID_compartment }
                                      select new
                                      {
                                          warehouse.ID_warehouse,
                                          warehouse.ID_compartment,
                                          warehouse.Quantity_Held,
                                          fs.Name,
                                          rs.DateOfRessuply,
                                          rs.Quantity_Bought
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
