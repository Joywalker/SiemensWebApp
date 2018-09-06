using System;
using System.Linq;
using System.Web.Http;
using SiemensWebAPI.Models.DomainViewModels;
using SiemensWebAPI.Helpers;
using SiemensWebAPI.Models.DataAccesLayer;
using System.Net.Http;

namespace SiemensWebAPI.Controllers
{
    public class AccountController : BaseController
    {
        [Route("api/user/login")]
        [HttpPost]
        public IHttpActionResult Login(Models.UserViewModel usr)
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    String userRole = dbctx.UserAccounts.Where(usn => usn.Username.Equals(usr.Username))
                                                        .Where(usp => usp.Password.Equals(usr.Password))
                                                        .Select(column => column.UserRole).First();

                    if (userRole != null || userRole != String.Empty)
                    {
                        var permissions = UserManagementHelper.GetPermissionsDictionaryFor(userRole);
                        return Ok(permissions);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in AccountController/api/User/Login", ex.ToString());
            }
            return Ok();
        }

        [HttpPut]
        [Route("api/user/recover")]
        public IHttpActionResult VerifyIfUserExistsInDB(UserConfirmationFormViewModel userConfirmationFormView)
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var user = dbctx.UserAccounts.Where(usr => usr.CNP.Equals(userConfirmationFormView.CNP))
                                                 .Where(usr => usr.Employee_ID.Equals(userConfirmationFormView.EmployeeID))
                                                 .Where(usr => usr.FirstName.Equals(userConfirmationFormView.FirstName))
                                                 .Where(usr => usr.LastName.Equals(userConfirmationFormView.LastName))
                                                 .FirstOrDefault();
                    if (user != null)
                    {
                        return Ok(user.CNP);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in AccountController/GetAll", ex.ToString());
            }
            return NotFound();
        }

        [HttpPost]
        [Route("api/user/updatePassword")]
        public IHttpActionResult UpdatePasswordForUser(HttpRequestMessage message)
        {
            try
            {
                var values = (message.Content.ReadAsStringAsync().Result).Split('|');
                String cnp = values[0];
                String password = values[1];
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    dbctx.UserAccounts.Where(usr => usr.CNP.Equals(cnp)).FirstOrDefault().Password = password;
                    dbctx.SaveChanges();
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in AccountController/GetAll", ex.ToString());
            }
            return NotFound();
        }
    }
}