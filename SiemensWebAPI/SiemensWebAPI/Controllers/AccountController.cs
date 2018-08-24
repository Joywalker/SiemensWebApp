using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Cors;
using System.Web.Http;
using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models;

namespace SiemensWebAPI.Controllers
{
    public class AccountController : BaseController
    {
        [Route("api/User/Login")]
        [HttpPost]
        public IHttpActionResult Login(UserViewModel usr)
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    String userRole = dbctx.UserAccounts.Where(usn => usn.Username.Equals(usr.Username))
                                                        .Where(usp => usp.Password.Equals(usr.Password))
                                                        .Select(column => column.UserRole).First();
                    if (userRole == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return Ok(userRole);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in AccountController/api/User/Login", ex.ToString());
            }
            return Ok();
        }

        [Route("api/User/GetAll")]
        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var allUsers = dbctx.UserAccounts.ToList();
                    return Ok(allUsers);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in AccountController/GetAll", ex.ToString());
            }
            return Ok();
        }
    }
}