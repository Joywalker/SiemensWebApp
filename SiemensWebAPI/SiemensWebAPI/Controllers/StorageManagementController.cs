using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models;
using SiemensWebAPI.Helpers;
using System.Data.Entity;

namespace SiemensWebAPI.Controllers
{
    public class StorageManagementController : BaseController
    {

        [Route("api/Storage/Ressuply")]
        [HttpPost]
        public IHttpActionResult oRessuply(Supply newSupply)
        {
            try
            {
                using(DatabaseContext dbctx = new DatabaseContext())
                {
                    // Verify if the newSupply object doesn't have null fields, thus it can be inserted into the database.
                    if (!ClassValidatorHelper.IsAnyNullOrEmpty(newSupply))
                    {
                        dbctx.Supplies.Add(newSupply);
                        return Ok(newSupply);
                    }
                }


            } catch(InvalidOperationException ex)
            {
                Console.WriteLine("Exception in StorageManagementController/api/Storage/Ressuply", ex.ToString());
            }
            return Ok(); 
        }


        [Route("api/Storage")]
        [HttpGet]
        public IHttpActionResult getStorageStatus()
        {
            using(DatabaseContext dbctx = new DatabaseContext())
            {
                try
                {
                    //Get records as a list of view model objects.
                    List<StorageViewModel> storageEntriesList = (from warehouse in dbctx.Warehouses
                                                                 join supply in dbctx.Supplies on warehouse.ID_compartment equals supply.ID_compartment
                                                                 join feedstock in dbctx.Feedstocks on warehouse.ID_feedstock equals feedstock.ID
                                                                 select new StorageViewModel
                                                                 {
                                                                     ID_warehouse = warehouse.ID_warehouse,
                                                                     ID_compartment = warehouse.ID_compartment,
                                                                     FeedstockName = feedstock.Name,
                                                                     QuantityStored = warehouse.Quantity_Held,
                                                                     NewestDateOfSupply = DbFunctions.TruncateTime(dbctx.Supplies.Where(sp => sp.ID_feedstock == warehouse.ID_feedstock)
                                                                                                        .Where(sp => sp.ID_compartment == warehouse.ID_compartment)
                                                                                                        .OrderByDescending(date => date.DateOfRessuply)
                                                                                                        .Select(date => date.DateOfRessuply)
                                                                                                        .FirstOrDefault()),
                                                                     OldestDateOfSupply = DbFunctions.TruncateTime(dbctx.Supplies.Where(sp => sp.ID_feedstock == warehouse.ID_feedstock)
                                                                                                        .Where(sp => sp.ID_compartment == warehouse.ID_compartment)
                                                                                                        .OrderBy(date => date.DateOfRessuply)
                                                                                                        .Select(date => date.DateOfRessuply)
                                                                                                        .FirstOrDefault()),
                                                                     QuantityFromTheOldestDate = dbctx.Supplies.Where(sp => sp.ID_feedstock == warehouse.ID_feedstock)
                                                                                                        .Where(sp => sp.ID_compartment == warehouse.ID_compartment)
                                                                                                        .OrderBy(date => date.DateOfRessuply)
                                                                                                        .Select(qtt => qtt.Quantity)
                                                                                                        .FirstOrDefault(),

                                                                 }).Distinct().ToList();

                    // transform from list to dictionary with the deposit_ID as key
                    var tempDictionary = StorageDataManagerHelper.transformFromListToDictionary(storageEntriesList);
                    return Ok(tempDictionary);
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("Exception in StorageManagementControllere/api/Storage", e.ToString());
                    return NotFound();
                }
            }
        }
    }
}
