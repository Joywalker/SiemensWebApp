using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models.DomainViewModels
{
    public class Ingredient
    {
        public String IngredientName { get; set; }
        public int Quantity { get; set; }
        public String MeasurementUnit { get; set; }
        override  public String ToString()
        {
            return IngredientName + "|" + Quantity + "|" + MeasurementUnit + "_";
        }
    }

    public class RecipeAction
    {
        public String ActionName { get; set; }
        public int Duration { get; set; }
        public String TimeMeasurementUnit { get; set; }
    }

    public class RecipeViewModel
    {
        public String RecipeName { get; set; }
        public Ingredient[] Ingredients;
        public RecipeAction[] Actions;
        

        public RecipeViewModel(String name, Ingredient[] ingr, RecipeAction[] act)
        {
            
            this.RecipeName = name;
            this.Ingredients = ingr;
            this.Actions = act;
        }
    }

    
}