using SiemensWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SiemensWebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class BaseController : ApiController
    {
       //METHODS AND OBJECTS NEEDED THROUGHOUT THE ENTIRE PROJECT
    }
}
