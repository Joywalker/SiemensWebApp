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
    public class RecipeManagementController: BaseController
    {
        [HttpPost]
        [Route("recipe/add")]
        public void AddNewRecipe(RecipeViewModel recipe)
        {
            var recipeToStringBuffer = RecipeManagementHelper.ParseObjectToStringForMSMQ(recipe);
            RecipeManagementHelper.SaveRecipeAsJsonToFile(recipe);
        }

    }
}