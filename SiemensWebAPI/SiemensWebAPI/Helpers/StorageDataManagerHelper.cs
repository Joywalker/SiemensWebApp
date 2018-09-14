using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiemensWebAPI.Models;
using System.Text.RegularExpressions;
using SiemensWebAPI.Models.DataAccesLayer;

namespace SiemensWebAPI.Helpers
{
    public class StorageDataManagerHelper
    {
        static int UNDEFINED_ID = -1;
        public static Dictionary<int, StorageViewModel[]> transformFromListToDictionary(List<StorageViewModel> list)
        {
            try
            {
                Dictionary<int, StorageViewModel[]> myTempDictionary = new Dictionary<int, StorageViewModel[]>();
                int ID_warehouse = UNDEFINED_ID;
                foreach (StorageViewModel stViewModel in list)
                {
                    if (stViewModel.ID_warehouse == ID_warehouse)
                        continue;
                    ID_warehouse = stViewModel.ID_warehouse;
                    StorageViewModel[] storageViewModels = list.Where(x => x.ID_warehouse == ID_warehouse).ToArray();
                    myTempDictionary.Add(ID_warehouse, storageViewModels);
                }
                return myTempDictionary;
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine("Exception at StorageDataHelper", e.ToString());
                return null; // Optimize
            }
        }
        public static int[] SplitString(String s)
        {
            int[] numbers = new int[100];
            numbers = s.Split('|').Select(Int32.Parse).ToArray();

            return numbers;
        }
        public static bool CompartmentValidation(int compartment)
        {
            bool result = false;
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var compartments = dbctx.Warehouses.Select(column => column.ID_compartment).Distinct().ToList();
                    foreach (var comp in compartments)
                    {
                        if (compartment == comp) return true;
                    }
                }

                return result;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Exception in StorageManagementControllere/api/Ioana", e.ToString());
                return false;
            }
        }
        public static bool WarehouseValidation(int warehouse)
        {
            bool result = false;
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var warehouses = dbctx.Warehouses.Select(column => column.ID_warehouse).Distinct().ToList();
                    foreach (var ware in warehouses)
                    {
                        if (warehouse == ware) return true;
                    }
                }
                return result;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Exception in StorageManagementControllere/api/Ioana", e.ToString());
                return false;
            }
        }
        public static bool MaterialValidation(String materialName)
        {
            bool result = false;
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var ID_materials = dbctx.Feedstocks.Select(column => column.Name).Distinct().ToList();
                    foreach (var nameID in ID_materials)
                    {
                        if (materialName == nameID) return true;
                    }
                }

                return result;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Exception in StorageManagementControllere/api/Ioana", e.ToString());
                return false;
            }
        }
    }
}