using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiemensWebAPI.Models.DomainViewModels;
using SiemensWebAPI.Models.DataAccesLayer;
namespace SiemensWebAPI.Helpers
{
    public class ProductStockHelper
    {
        public static void UpdateStock(OrderViewModel order)
        {
            using (DatabaseContext dbctx = new DatabaseContext())
            {
                var amount = (from productstock in dbctx.ProductStocks
                              join ord in dbctx.Orders on productstock.Recipe equals ord.Recipe
                              where (productstock.Recipe.Equals(order.Recipe))
                              select new { Amount = productstock }).ToList();
                amount.ElementAt(0).Amount.Number += order.Amount;
                dbctx.SaveChanges();
                LoggerHelper.Products(order.Recipe + ">> a crescut cu " + order.Amount);
            }
        }
    }
}