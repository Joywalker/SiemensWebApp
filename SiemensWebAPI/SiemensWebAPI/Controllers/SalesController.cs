using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models.DomainViewModels;
using System;
using System.Linq;
using System.Web.Http;
using SiemensWebAPI.Helpers;

namespace SiemensWebAPI.Controllers
{
    public class SalesController : BaseController
    {
        [Route("api/stock/updateStock")]
        [HttpPost]
        public IHttpActionResult UpdateStock(SalesViewModel prod)
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var productToSale = dbctx.ProductStocks.Where(p => p.Recipe.Equals(prod.Recipe))
                                                           .FirstOrDefault();
                    if (prod.Amount > productToSale.Number.Value)
                    {
                        int productToOrder = prod.Amount - productToSale.Number.Value;
                        string str = "Insufficient stock. " + productToOrder.ToString() + " bags need to be ordered.";
                        LoggerHelper.FailedSales(str);
                        return Ok(str);
                    }
                    else
                    {
                        dbctx.ProductStocks.Where(p => p.Recipe.Equals(prod.Recipe)).FirstOrDefault().Number -= prod.Amount;
                        dbctx.SaveChanges();
                        LoggerHelper.SucceededSales(prod);
                        return Ok("Succeeded");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception at UpdateStock", ex.ToString());
                return NotFound();
            }
        }
    }
}