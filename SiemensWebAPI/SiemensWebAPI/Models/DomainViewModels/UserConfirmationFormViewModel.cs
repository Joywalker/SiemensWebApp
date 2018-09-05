using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Models.DomainViewModels
{
    public class UserConfirmationFormViewModel
    {
        public String CNP { get; set; }
        public String EmployeeID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
    }
}