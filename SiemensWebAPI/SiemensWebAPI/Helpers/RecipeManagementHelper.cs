using SiemensWebAPI.Models.DomainViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SiemensWebAPI.Helpers
{
    public class RecipeManagementHelper
    {
        private static String FILEEXTENSION = ".txt";
        private static String PROJECT_BASE_PATH = AppDomain.CurrentDomain.BaseDirectory;
        private static String RECIPES_FOLDER_NAME = "/Recipes/";
        private static String RECIPES_FOLDER_PATH = PROJECT_BASE_PATH + RECIPES_FOLDER_NAME;

        public static string ParseObjectToStringForMSMQ(RecipeViewModel recipe)
        {
            try
            {
                StringBuilder ingredientsSB = new StringBuilder("");
                StringBuilder actionsSB = new StringBuilder("$");

                foreach (Ingredient ingr in recipe.Ingredients)
                {
                    ingredientsSB.Append(ingr.IngredientName + "|" + ingr.Quantity + "|" + ingr.MeasurementUnit + "_");
                }

                string finalIngredients = ingredientsSB.ToString(0, ingredientsSB.Length - 1);

                foreach (RecipeAction action in recipe.Actions)
                {
                    actionsSB.Append(action.ActionName + "|" + action.Duration + "|" + action.TimeMeasurementUnit + "_");
                }

                string finalActions = actionsSB.ToString(0, actionsSB.Length - 1);

                var finalString = finalIngredients + finalActions;
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
            String jsonString = JsonConvert.SerializeObject(recipe, Newtonsoft.Json.Formatting.Indented);
            return jsonString;

        }
        public static bool WasAbleToSaveRecipeAsJsonToFile(RecipeViewModel recipe)
        {
            try
            {
                var recipeAsJson = TransformObjectToJsonString(recipe);
                var whereToWrite = RECIPES_FOLDER_PATH + recipe.RecipeName + FILEEXTENSION;
                File.WriteAllText(whereToWrite, recipeAsJson);
                return true;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Exception at RecipeManagementHelper ", ex.ToString());
                return false;
            }
        }
        private static RecipeViewModel TransformToRecipeFromFile(String fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string json = sr.ReadToEnd();
                RecipeViewModel recipe = JsonConvert.DeserializeObject<RecipeViewModel>(json);
                return recipe;
            }
        }
        public static List<RecipeViewModel> GetAllRecipes()
        {
            List<RecipeViewModel> recipesList = new List<RecipeViewModel>();
            string[] fileEntries = Directory.GetFiles(RECIPES_FOLDER_PATH);
            foreach (string fileName in fileEntries)
            {
                var recipe = TransformToRecipeFromFile(fileName);
                recipesList.Add(recipe);
            }
           
            return recipesList;
        }

        public static bool WasAbleToDeleteRecipeWithName(String recipeName)
        {
            try
            {
                string[] fileEntries = Directory.GetFiles(RECIPES_FOLDER_PATH);
                foreach (string filePath in fileEntries)
                {
                    var filename = Path.GetFileNameWithoutExtension(filePath);
                    if (filename.Equals(recipeName))
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            return true;
                        }
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Exception at RecipeManagementHelper", e.ToString());
            }
            return false;
        }
    }
}