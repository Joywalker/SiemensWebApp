using SiemensWebAPI.Models.DomainViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace SiemensWebAPI.Helpers
{
    public class RecipeManagementHelper
    {
        public static String ParseObjectToStringForMSMQ(RecipeViewModel recipe)
        {
            try
            {
                return "";
            }
            catch(ArgumentNullException ex)
            {

            }
            return "";
        }
    }
}