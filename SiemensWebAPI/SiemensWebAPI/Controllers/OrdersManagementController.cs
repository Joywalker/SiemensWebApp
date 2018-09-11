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
        [Route("api/orders/add")]
        [HttpPost]
        public IHttpActionResult Order(OrderViewModel order)
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var recipList = RecipeManagementHelper.GetAllRecipes();

                    RecipeViewModel recipe = recipList.Find(recip => recip.RecipeName == order.Recipe);

                    if (recipe.RecipeName.Equals(order.Recipe))
                    {
                        var Ingredients = recipe.Ingredients;

                        if (OrdersManagementHelper.OrderValidation(Ingredients, order.Amount).ElementAt(0).Key.Equals("true"))
                        {
                            var NewIngredients = OrdersManagementHelper.ExtractIngredients(Ingredients, order.Amount);
                            RecipeViewModel NewRecipe = new RecipeViewModel(recipe.RecipeName, NewIngredients, recipe.Actions);
                            OrdersManagementHelper.AddOrder(order);
                            return Ok("SUCCESS");
                        }
                        else return Ok(OrdersManagementHelper.OrderValidation(Ingredients, order.Amount));
                    }
                    else return NotFound();
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Exception in OrdersManagementControllere/api/Order", e.ToString());
                return NotFound();
            }
        }
        [Route("api/orders/get")]
        [HttpGet]
        public IHttpActionResult GetAllOrders()
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var orders = dbctx.Orders.Select(order => order).ToList();
                    if(orders != null)
                    {
                        return Ok(orders);
                    } else
                    {
                        return NotFound();
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Exception in OrdersManagementControllere/api/Orders/get", e.ToString());
                return NotFound();
            }
        }
    }
}