using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models.DomainViewModels;

namespace SiemensWebAPI.Helpers
{
    public class OrdersManagementHelper
    {
        public static List<KeyValuePair<string, int>> OrderValidation(Ingredient[] Ingredients, int Amount)
        {
            int disponibil = 0;
            List<KeyValuePair<string, int>> kvpList = new List<KeyValuePair<string, int>>();

            using (DatabaseContext dbctx = new DatabaseContext())
            {
                foreach (var ingredient in Ingredients)
                {
                    // array cu cantitatile din fiecare depozit pentru fiecare ingredient
                    var quantity = (from warehouse in dbctx.Warehouses
                                    join feedstock in dbctx.Feedstocks on warehouse.ID_feedstock equals feedstock.ID
                                    where (feedstock.Name == ingredient.IngredientName)
                                    select new { Quantity = warehouse }).ToList();

                    // cantitatea totala a unui ingredient disponibila in toate depozitele
                    var sum = quantity.Select(q => q.Quantity.Quantity_Held).Sum();

                    // cantitatea de ingredient necesare retetei
                    var IngQuantity = OrdersManagementHelper.MeasurementUnit(ingredient.MeasurementUnit, (double)ingredient.Quantity);

                    // se verifica daca e cantitate disponibila in depozite
                    if (IngQuantity * Amount < sum) disponibil++;
                    else kvpList.Add(new KeyValuePair<string, int>(ingredient.IngredientName, sum.Value));
                }

                if (disponibil == Ingredients.Length) kvpList.Add(new KeyValuePair<string, int>("true", 0));
            }

            return kvpList;
        }

        public static double MeasurementUnit(String measurement, double quantity)
        {
            if (measurement.Equals("g")) quantity = quantity * 0.001;
            return quantity;
        }

        public static void AddOrder(OrderViewModel order)
        {
            using (DatabaseContext dbctx = new DatabaseContext())
            {
                var ord = new Order();
                ord.Recipe = order.Recipe;
                ord.Amount = order.Amount;
                dbctx.Orders.Add(ord);
                dbctx.SaveChanges();
            }
        }

        public static Ingredient[] ExtractIngredients(Ingredient[] Ingredients, int Amount)
        {

            using (DatabaseContext dbctx = new DatabaseContext())
            {
                foreach (var ingredient in Ingredients)
                {
                    var quantity = (from warehouse in dbctx.Warehouses
                                    join feedstock in dbctx.Feedstocks on warehouse.ID_feedstock equals feedstock.ID
                                    where (feedstock.Name == ingredient.IngredientName)
                                    select new { Quantity = warehouse }).ToList();

                    int TotalQuantity = Convert.ToInt32(OrdersManagementHelper.MeasurementUnit(ingredient.MeasurementUnit, ingredient.Quantity) * Amount);

                    for (int i = 0; i < quantity.Count(); i++)
                    {
                        if (quantity.ElementAt(i).Quantity.Quantity_Held <= TotalQuantity)
                        {
                            var q = quantity.ElementAt(i).Quantity.Quantity_Held;
                            quantity.ElementAt(i).Quantity.Quantity_Held = 0;
                            quantity.ElementAt(i + 1).Quantity.Quantity_Held = quantity.ElementAt(i + 1).Quantity.Quantity_Held + q - TotalQuantity;
                        }
                        else quantity.ElementAt(i).Quantity.Quantity_Held -= TotalQuantity;

                        TotalQuantity = 0;
                    }

                    ingredient.Quantity = Convert.ToInt32(OrdersManagementHelper.MeasurementUnit(ingredient.MeasurementUnit, ingredient.Quantity) * Amount);

                    dbctx.SaveChanges();
                }
            }

            return Ingredients;
        }

    }
}