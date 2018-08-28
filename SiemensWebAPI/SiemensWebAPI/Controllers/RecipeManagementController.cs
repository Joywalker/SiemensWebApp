using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models;
using SiemensWebAPI.Models.DomainViewModels;

namespace SiemensWebAPI.Controllers
{
    public class RecipeManagementController: BaseController
    {
        [HttpPost]
        [Route("recipe/add")]
        public IHttpActionResult AddNewRecipe(RecipeViewModel recipe)
        {

            return Ok(recipe);
        }
    }
}