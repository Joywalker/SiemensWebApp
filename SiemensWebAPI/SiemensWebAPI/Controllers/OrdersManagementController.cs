using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SiemensWebAPI.Models.DomainViewModels;
using SiemensWebAPI.Helpers;
using SiemensWebAPI.Models.DataAccesLayer;

namespace SiemensWebAPI.Controllers
{
    public class OrdersManagementController : BaseController
    {
        [Route("api/Order")]
        [HttpPost]
        public IHttpActionResult Order(OrderViewModel order)
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var ord = new Order();
                    ord.Recipe = order.Recipe;
                    ord.BagSize = order.BagSize;
                    ord.Amount = order.Amount;
                    dbctx.Orders.Add(ord);
                    dbctx.SaveChanges();
                }
            }
            catch
            {

            }
            return Ok("Ok");
        }

    }
}