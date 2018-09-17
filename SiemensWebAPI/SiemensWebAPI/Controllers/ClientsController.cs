using SiemensWebAPI.Models.DataAccesLayer;
using SiemensWebAPI.Models.DomainViewModels;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace SiemensWebAPI.Controllers
{
    public class ClientsController : BaseController
    {
        [Route("api/client/selectClient")]
        [HttpPost]
        public IHttpActionResult SelectClient(ClientViewModel cln)
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var client = dbctx.Clients.Where(c => c.Name.Equals(cln.Name))
                                              .FirstOrDefault();
                    return Ok(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client doesn't exist.", ex.ToString());
                return NotFound();
            }
        }

        [Route("api/client/addClient")]
        [HttpPost]
        public IHttpActionResult AddClient(HttpRequestMessage name)
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    Client client = new Client { Name = name.Content.ReadAsStringAsync().Result };
                    dbctx.Clients.Add(client);
                    dbctx.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in addClient", ex.ToString());
                return NotFound();
            }
        }
    }
}