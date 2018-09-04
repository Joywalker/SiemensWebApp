using System;
using System.Net.Http;
using System.Web.Http;
using SiemensWebAPI.Models.DomainViewModels;
using SiemensWebAPI.Helpers;

namespace SiemensWebAPI.Controllers
{
    public class RecipeManagementController : BaseController
    {
        [HttpPost]
        [Route("api/recipe/add")]
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
            }
            catch (Exception e)
            {
                return Ok(e);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/recipe/get")]
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
        [HttpPut]
        [Route("api/recipe/delete")]
        public IHttpActionResult DeleteRecipeByID(HttpRequestMessage request)
        {
            var segmentsLength = request.Headers.Referrer.Segments.Length;
            var parameter = request.Headers.Referrer.Segments[segmentsLength - 1];
            bool result = RecipeManagementHelper.WasAbleToDeleteRecipeWithName(parameter);
            return (result) ? Ok(true) : Ok(false);
        }
    }
}