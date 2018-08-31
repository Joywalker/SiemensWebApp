using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models;
using SiemensWebAPI.Models.DomainViewModels;
using SiemensWebAPI.Helpers;

namespace SiemensWebAPI.Controllers
{
    public class RecipeManagementController : BaseController
    {
        [HttpPost]
        [Route("recipe/add")]
        public IHttpActionResult AddNewRecipe(RecipeViewModel recipe)
        {
            try
            {
                var recipeToStringBuffer = RecipeManagementHelper.ParseObjectToStringForMSMQ(recipe);
                var response = RecipeManagementHelper.WasAbleToSaveRecipeAsJsonToFile(recipe);
                if (response)
                {
                    return Ok(response);
                }
            } catch (Exception e)
            {
                return Ok(e);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("recipe/get")]
        public IHttpActionResult GetAllRecipes()
       {
            try
            {
                var recipesList = RecipeManagementHelper.GetAllRecipes();
                return Ok(recipesList);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at RecipeManagementController", e.ToString());
                return NotFound();
            }
        }
    }
}