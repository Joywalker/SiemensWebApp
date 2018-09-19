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
        [Route("api/sales/getAllClients")]
        [HttpGet]
        public IHttpActionResult GetClients(ClientViewModel cln)
        {
            try
            {
                using (DatabaseContext dbctx = new DatabaseContext())
                {
                    var clients = dbctx.Clients.Select(cl => cl.Name).ToList();
                    return Ok(clients);
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