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
                            return Ok("Comanda efectuata cu succes");
                        }
                        else return Ok(OrdersManagementHelper.OrderValidation(Ingredients, order.Amount));
                    }
                    else return Ok("Nu exista reteta");
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Exception in OrdersManagementControllere/api/Order", e.ToString());
                return NotFound();
            }
        }
    }
}