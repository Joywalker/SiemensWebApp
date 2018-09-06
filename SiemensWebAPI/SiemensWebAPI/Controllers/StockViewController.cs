using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models.DomainViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
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
                    var allCurrentProducts = dbctx.ProductStocks.ToList();
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
        [HttpGet]
        public IHttpActionResult LastMonthEvolution(DateTime currentDate)
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    return Ok();
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