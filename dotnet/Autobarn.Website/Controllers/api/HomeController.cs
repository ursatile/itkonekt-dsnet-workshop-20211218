using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase {

        [HttpGet]
        [Produces("application/hal+json")]
        public IActionResult Get() {
            var response = new {
                message = "Welcome to the Autobarn API!",
                version = "v0.0.1",
                _links = new {
                    vehicles = new {
                        href = "/api/vehicles"
                    },
                    models = new {
                        href = "https://myotherserver.mycompany.com/api/models"
                    }
                }
            };
            return Ok(response);
        }
    }
}
