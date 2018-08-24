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
                    List<StorageViewModel> storageEntriesList = (from warehouse in dbctx.Warehouses
                                                         join supply in dbctx.Supplies on warehouse.ID_compartment equals supply.ID_compartment
                                                         join feedstock in dbctx.Feedstocks on warehouse.ID_feedstock equals feedstock.ID
                                                         select new StorageViewModel
                                                         {
                                                             ID_warehouse = warehouse.ID_warehouse,
                                                             ID_compartment = warehouse.ID_compartment,
                                                             feedstockName = feedstock.Name,
                                                             quantityStored = warehouse.Quantity_Held,
                                                             newestDateOfSupply = dbctx.Supplies.Where(sp => sp.ID_feedstock == warehouse.ID_feedstock)
                                                                                                .Where(sp => sp.ID_compartment == warehouse.ID_compartment)
                                                                                                .OrderByDescending(date => date.DateOfRessuply)
                                                                                                .Select(date => date.DateOfRessuply)
                                                                                                .FirstOrDefault(),
                                                             oldestDateOfSupply = dbctx.Supplies.Where(sp => sp.ID_feedstock == warehouse.ID_feedstock)
                                                                                                .Where(sp => sp.ID_compartment == warehouse.ID_compartment)
                                                                                                .OrderBy(date => date.DateOfRessuply)
                                                                                                .Select(date => date.DateOfRessuply)
                                                                                                .FirstOrDefault(),
                                                             quantityFromTheOldestDate = dbctx.Supplies.Where(sp => sp.ID_feedstock == warehouse.ID_feedstock)
                                                                                                .Where(sp => sp.ID_compartment == warehouse.ID_compartment)
                                                                                                .OrderBy(date => date.DateOfRessuply)
                                                                                                .Select(qtt => qtt.Quantity)
                                                                                                .FirstOrDefault(),

                                                         }).Distinct().ToList();
                    
                    return Ok(storageEntriesList);
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
