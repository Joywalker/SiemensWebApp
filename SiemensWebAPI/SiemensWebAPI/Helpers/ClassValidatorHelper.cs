using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models;

namespace SiemensWebAPI.Helpers
{
    public class ClassValidatorHelper
    {
        public static bool IsAnyNullOrEmpty(object classToVerify)
        {
            foreach (var property in classToVerify.GetType().GetProperties())
            {
                string value = (string)property.GetValue(classToVerify);
                if (String.IsNullOrEmpty(value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}