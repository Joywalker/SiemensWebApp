using SiemensWebAPI.Models.DomainViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace SiemensWebAPI.Helpers
{
    public class RecipeManagementHelper
    {
        private static int RecipeIndex = 1;
        private static StringBuilder RecipeSB = new StringBuilder(RecipeIndex.ToString());
        private static String StoredRecipesFilePath = "C:/Users/alexandru.razvant/Desktop/SiemensWebApp/SiemensWebAPI/SiemensWebAPI/RecipeList.rtf";
        public static StringBuilder ParseObjectToStringForMSMQ(RecipeViewModel recipe)
        {
            try
            {
                StringBuilder ingredientsSB = new StringBuilder("|");
                StringBuilder actionsSB = new StringBuilder("$");

                foreach (Ingredient ingr in recipe.Ingredients)
                {
                    ingredientsSB.Append(ingr.IngredientName + "|" + ingr.Quantity + "|" + ingr.MeasurementUnit + "|");
                }
                foreach (RecipeAction action in recipe.Actions)
                {
                    actionsSB.Append(action.ActionName + "|" + action.Duration + "|" + action.TimeMeasurementUnit + "|");
                }
                var finalString = ingredientsSB.Append(actionsSB);
                return finalString;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Exception at parseobjecttoString" + ex.ToString());
                return null;
            }
        }
        private static String TransformObjectToJsonString(RecipeViewModel recipe)
        {
            String jsonString = JsonConvert.SerializeObject(recipe, Formatting.Indented);
            String recipeWithIDString = "{" + "\"" + RecipeIndex++.ToString() + "\" " + ":";
            return recipeWithIDString + jsonString;

        }
        public static void SaveRecipeAsJsonToFile(RecipeViewModel recipe)
        {
            try
            {
                var recipeAsJson = RecipeManagementHelper.TransformObjectToJsonString(recipe);
                File.AppendAllText(StoredRecipesFilePath, recipeAsJson);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Exception at RecipeManagementHelper ", ex.ToString());
            }
        }
    }
}