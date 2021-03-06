﻿using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models.DomainViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SiemensWebAPI.Controllers
{
    public class StockViewController : BaseController
    {
        [Route("api/stock/viewAll")]
        [HttpGet]
        public IHttpActionResult ViewCurrentStock()
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    List<ProductStockViewModel> allCurrentProducts = dbctx.ProductStocks.OrderBy(dt => dt.ManufactureDate).Select(entry => new ProductStockViewModel
                    {
                        Name = entry.Name,
                        NumberOfBags = (int)entry.Number,
                        Recipe = entry.Recipe,
                        ManufactureDate = entry.ManufactureDate
                    }).ToList();
                    return Ok(allCurrentProducts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception at ViewCurrentStock", ex.ToString());
                return NotFound();
            }
        }

        [Route("api/stock/lastMonthEv")]
        [HttpGet]
        public IHttpActionResult LastMonthEvolution()
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    DateTime currentDate = DateTime.Now;
                    DateTime earliestDate = currentDate.AddDays(-30);
                    var entries = dbctx.ProductStocks.Where(usr => (usr.ManufactureDate <= currentDate && usr.ManufactureDate >= earliestDate))
                        .GroupBy(x => x.ManufactureDate).Select(entry => new
                        {
                            date = entry.Key,
                            number = dbctx.ProductStocks.Where(date => date.ManufactureDate == entry.Key).Sum(count => count.Number)
                        })
                        .ToList();
                    return Ok(entries);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception at LastMonthEvolution", ex.ToString());
                return NotFound();
            }
        }

    }
}