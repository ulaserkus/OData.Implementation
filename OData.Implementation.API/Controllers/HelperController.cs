using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace OData.Implementation.API.Controllers
{
    public class HelperController : ODataController
    {
        [ODataRoute("GetKdv")]
        [HttpGet]
        public IActionResult GetKdv()
        {
            return Ok(0.18);
        }
    }
}
