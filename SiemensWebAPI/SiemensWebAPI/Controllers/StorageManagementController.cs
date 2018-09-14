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
using SiemensWebAPI.Models.DomainViewModels;

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
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    // Verify if the newSupply object doesn't have null fields, thus it can be inserted into the database.
                    if (!ClassValidatorHelper.IsAnyNullOrEmpty(newSupply))
                    {
                        dbctx.Supplies.Add(newSupply);
                        return Ok(newSupply);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Exception in StorageManagementController/api/Storage/Ressuply", ex.ToString());
            }
            return Ok();
        }

        [Route("api/Storage")]
        [HttpGet]
        public IHttpActionResult getStorageStatus()
        {
            using (DatabaseContext dbctx = new DatabaseContext())
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
                                                                     QuantityStored = warehouse.Quantity_Held.Value,
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

        [Route("api/Storage/getOptions")]
        [HttpGet]
        public IHttpActionResult getOptionsForStorageEdit()
        {
            using (DatabaseContext dbctx = new DatabaseContext())
            {
                try
                {
                    var optionsList = dbctx.Warehouses.Join(dbctx.Feedstocks,
                        warehouse => warehouse.ID_feedstock,
                        feedstock => feedstock.ID,
                        (warehouse, feedstock) => new { WH = warehouse, FS = feedstock })
                        .Where(warehouseAndFeedstock => warehouseAndFeedstock.FS.ID == warehouseAndFeedstock.WH.ID_feedstock).Select(entry => new
                        {
                            entry.WH.ID_compartment,
                            entry.FS.Name
                        }).Distinct().ToList();

                    return Ok(optionsList);
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("Exception in StorageManagementControllere/api/Storage", e.ToString());
                    return NotFound();
                }
            }
        }

        [Route("api/EditStorage")]
        [HttpPost]
        public IHttpActionResult UpdateWarehouse(UpdateViewModel update)
        {
            try
            {
                if (StorageDataManagerHelper.CompartmentValidation(update.ID_DC) && StorageDataManagerHelper.CompartmentValidation(update.ID_SC) && StorageDataManagerHelper.MaterialValidation(update.MaterialName))
                {
                    using (DatabaseContext dbctx = new DatabaseContext())
                    {
                        //stringul ID_supplyes din Warehouse pentru dep sursa
                        string supplyes = dbctx.Warehouses.Where(comp => comp.ID_compartment.Equals(update.ID_SC))
                                                          .Select(column => column.ID_supply).FirstOrDefault();

                        // array format din stringurile supplyes
                        int[] sup = StorageDataManagerHelper.SplitString(supplyes);

                        // materia prima ce trebuie mutata
                        string id_material = dbctx.Warehouses.Where(comp => comp.ID_compartment.Equals(update.ID_DC))
                                                          .Join(dbctx.Feedstocks, wh => wh.ID_feedstock, fs => fs.ID, (wh,fs)=> new { wh, fs })
                                                          .Where(whAndfs => whAndfs.fs.ID == whAndfs.wh.ID_feedstock)
                                                          .Select(column => column.fs.Name).First(); // materia prima ce trebuie mutata

                        // variabila pentru depozitul destinatie
                        var dataDestination = (from warehouse in dbctx.Warehouses
                                               join supply in dbctx.Supplies on warehouse.ID_compartment equals supply.ID_compartment
                                               where (supply.ID_feedstock == warehouse.ID_feedstock) && (supply.ID_compartment == update.ID_DC)
                                               select new { Destination = supply }
                                );

                        // variabila pentru depozitul sursa
                        var dataSource = (from warehouse in dbctx.Warehouses
                                          join supply in dbctx.Supplies on warehouse.ID_compartment equals supply.ID_compartment
                                          where (supply.ID_feedstock == warehouse.ID_feedstock) && (supply.ID_compartment == update.ID_SC)
                                          orderby supply.DateOfRessuply
                                          select new { Source = supply }
                                         ).ToList();

                        //cantitatea din depozitul sursa
                        int quantitySource = dbctx.Warehouses
                                                        .Where(comp => comp.ID_compartment.Equals(update.ID_SC))
                                                        .Select(column => column.Quantity_Held).First().Value;

                        // cantitatea de la utima aprovizionare a compartimentului din depozitul sursa
                        var LastSupplyQuantity = dataSource.ElementAt(sup.Length - 1).Source.Quantity;
                        if (update.MaterialName == id_material && update.Quantity < quantitySource)

                        {
                            LoggerHelper.UpdateWarehouse(update.MaterialName ,update.ID_SC, update.ID_SC, update.ID_DC, update.ID_DC);
                            dbctx.Warehouses.First(w => w.ID_compartment == update.ID_DC).Quantity_Held += update.Quantity;
                            dbctx.Warehouses.First(w => w.ID_compartment == update.ID_SC).Quantity_Held -= update.Quantity;

                            if (update.Quantity.ToString().Equals(LastSupplyQuantity))
                            {
                                dbctx.Warehouses.First(w => w.ID_compartment == update.ID_SC).ID_supply = dbctx.Warehouses.First(w => w.ID_compartment == update.ID_SC).ID_supply.Remove(supplyes.Length - 2);
                                dbctx.Warehouses.First(w => w.ID_compartment == update.ID_DC).ID_supply += "|" + supplyes.ElementAt(supplyes.Length - 1);
                            }
                            else
                            {
                                dbctx.Warehouses.First(w => w.ID_compartment == update.ID_DC).ID_supply += "|" + supplyes.ElementAt(supplyes.Length - 1);
                            }
                            dbctx.SaveChanges();
                            return Ok("Ok");
                        }

                        else
                        {
                            return Ok("NEQ"); // not enough quantity
                        }
                    }
                }
                return Ok("Invalid data!");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Exception in StorageManagementControllere/api/EditStorage", e.ToString());
                return NotFound();
            }
        }
    }
}

