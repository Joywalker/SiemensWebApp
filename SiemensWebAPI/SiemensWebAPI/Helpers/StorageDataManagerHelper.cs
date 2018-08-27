using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiemensWebAPI.Models;
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
            } catch(InvalidCastException e)
            {
                Console.WriteLine("Exception at StorageDataHelper", e.ToString());
                return null; // Optimize
            }
        }
    }
}