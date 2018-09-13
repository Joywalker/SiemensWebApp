using SiemensWebAPI.Models.DataAccesLayer;
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
                    List<ProductStock> allCurrentProducts = dbctx.ProductStocks.ToList();
                    return Ok(allCurrentProducts);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception at ViewCurrentStock", ex.ToString());
                return NotFound();
            }
        }

        [Route("api/stock/lastMonthEv")]
        [HttpPost]
        public IHttpActionResult LastMonthEvolution()
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    DateTime currentDate = DateTime.Now;
                    DateTime earliestDate = currentDate.AddDays(-30);
                    List<ProductStock> products = dbctx.ProductStocks.Where(usr => usr.ManufactureDate <= currentDate && usr.ManufactureDate >= earliestDate).ToList();
                    List<KeyValuePair<DateTime?, int?>> graph = new List<KeyValuePair<DateTime?, int?>>();
                    graph = products.GroupBy(p => p.ManufactureDate)
                                    .Select(p => new KeyValuePair<DateTime?, int?>(p.Key, p.Sum(prod => prod.Number))).ToList();
                    return Ok(graph);
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