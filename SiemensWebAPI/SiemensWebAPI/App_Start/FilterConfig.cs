using System.Web;
using System.Web.Mvc;
using System.Threading;
using SiemensWebAPI.Helpers;
using SiemensWebAPI.Models.DomainViewModels;
using System.Collections.Generic;

namespace SiemensWebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {                                   
            filters.Add(new HandleErrorAttribute());
        }
    }
}
