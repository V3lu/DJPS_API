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
        public async IAsyncEnumerable<string> TestComm([FromBody] int jobLength)
        {
            for (int i = 0; i < jobLength; i++)
            {
                await Task.Delay(500);
                yield return $"Step {i} completed";
            }
        }
    }
}
