using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models.DomainViewModels;
using communicationModule;
using System.Threading;
namespace SiemensWebAPI.Helpers
{
    public class OrdersManagementHelper
    {
        public static String globalRecipe = "";
        public static List<OrderViewModel> Orders = new List<OrderViewModel>();
        public static bool CmdInExecution = false; // nu avem comanda in executie      
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
            int TotalQuantity = 0;
            using (DatabaseContext dbctx = new DatabaseContext())
            {
                foreach (var ingredient in Ingredients)
                {
                    var quantity = (from warehouse in dbctx.Warehouses
                                    join feedstock in dbctx.Feedstocks on warehouse.ID_feedstock equals feedstock.ID
                                    where (feedstock.Name == ingredient.IngredientName)
                                    select new { Quantity = warehouse }).ToList();
                    double qtty = MeasurementUnit(ingredient.MeasurementUnit, ingredient.Quantity) * Amount;
                    if (qtty < 1)
                    {
                        TotalQuantity = 1;
                    }
                    else
                    {
                        TotalQuantity = (int)Math.Ceiling(qtty);
                    }
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
                    if (qtty < 1)
                        ingredient.Quantity = 1;
                    else
                        ingredient.Quantity = (int)Math.Ceiling(qtty);
                    ingredient.MeasurementUnit = "kg";
                    dbctx.SaveChanges();
                }
            }
            return Ingredients;
        }
        public static int ParseStringToInt(string s)
        {
            int Amount;
            string[] ord = s.Split('|');
            string ss = ord.ElementAt(ord.Length - 1);
            Amount = Int32.Parse(ss);
            return Amount;
        }
        public static void OrderStatus(OrderViewModel order)
        {
            using (DatabaseContext dbctx = new DatabaseContext())
            {
                var idOrder = dbctx.Orders.Select(column => column.ID_order)
                                          .ToList();
                var id = idOrder.LastOrDefault();
                int initialAmount = order.Amount;
                LoggerHelper.Order(" a fost trimisa cu succes.", " Va avea id-ul " + (id + 1).ToString() + ".");
                Thread.Sleep(3000);
                MethodsClass mc = new MethodsClass();
                string response = mc.ReceiveMessage();
                if (response.StartsWith("0"))
                {
                    OrdersManagementHelper.AddOrder(order);
                    LoggerHelper.Order(" finalizata cu succes cu id-ul ", (id + 1).ToString() + "." + initialAmount + " produse cu reteta <<" + order.Recipe + ">> au fost create cu succes.");
                    ProductStockHelper.UpdateStock(order);
                    OrdersManagementHelper.CmdInExecution = false;
                }
                else if (response.StartsWith("-1"))
                {
                    order.Amount = OrdersManagementHelper.ParseStringToInt(response);
                    OrdersManagementHelper.AddOrder(order);
                    LoggerHelper.Order(" finalizata cu eroare cu id-ul ", (idOrder.LastOrDefault() + 1).ToString() + "." + OrdersManagementHelper.ParseStringToInt(response) + " produse cu reteta <<" + order.Recipe + ">> au fost create din " + initialAmount + ".");
                    ProductStockHelper.UpdateStock(order);
                    OrdersManagementHelper.CmdInExecution = false;
                }
                else
                {
                    OrdersManagementHelper.CmdInExecution = true;
                    SendRecipe(response);
                }
            }
        }
        public static void SendRecipe(String recipe)
        {
            MethodsClass mc = new MethodsClass();
            mc.SendMessage(recipe);
        }
        public static void CheckQueue(List<OrderViewModel> ords)
        {
            while (true)
            {
                if (ords.Count >= 1)
                {
                    if (OrdersManagementHelper.CmdInExecution == false)
                    {
                        if (globalRecipe != null && globalRecipe != "")
                        {
                            SendRecipe(globalRecipe);                          
                            OrderStatus(ords.ElementAt(0));
                            ords.RemoveAt(0);
                        }
                    }
                }
            }
        }
    }
}