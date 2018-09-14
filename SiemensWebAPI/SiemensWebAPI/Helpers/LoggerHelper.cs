using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Helpers
{
    public class LoggerHelper
    {
        private static String FILEEXTENSION = "Logger.txt";
        private static String PROJECT_BASE_PATH = AppDomain.CurrentDomain.BaseDirectory;
        private static String LOGGER_FOLDER_NAME = "/Logger/";
        private static String LOGGER_FOLDER_PATH = PROJECT_BASE_PATH + LOGGER_FOLDER_NAME + FILEEXTENSION;

        public static void UserAction(String username, String action)
        {
            String today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(LOGGER_FOLDER_PATH, true))
            {
                file.WriteLine(action + " pentru utilizatorul " + username + " (" + today + ")");
            }
        }

        public static void UpdateWarehouse(string id_material, int id_swarehouse, int id_scompartment, int id_dwarehouse, int id__dcompartment)
        {
            String today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(LOGGER_FOLDER_PATH, true))
            {
                file.WriteLine("S-a mutat materia prima cu id-ul " + id_material.ToString() + " din depozitul " + id_swarehouse.ToString() + ", compartimentul " + id_scompartment.ToString() + " in depozitul " + id_dwarehouse.ToString() + ", compartimentul " + id__dcompartment.ToString() + " (" + today + ")");
            }
        }

       public static void Order(String action, String id_order)
        {
            String today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(LOGGER_FOLDER_PATH, true))
            {
                file.WriteLine("Comanda " +  action + id_order + " (" + today + ")");
            }
        }

        public static void Products(int products)
        {
            String today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(LOGGER_FOLDER_PATH, true))
            {
                file.WriteLine("Stocul de produse a crescut cu " + products + " produse." + " (" + today + ")");
            }
        }


    }
}