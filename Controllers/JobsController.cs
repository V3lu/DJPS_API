using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AJPS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        public JobsController() { }

        [HttpPost]
        [Route("testcomm")]
        public async Task<IActionResult> TestComm([FromBody] int jobLength)
        {
            int tryme = jobLength;
            return Ok(tryme);
        }
    }
}
