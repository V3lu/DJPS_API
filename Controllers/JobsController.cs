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
                await Task.Delay(100); //Simulate job
                yield return $"Step {i} completed";
            }
        }

        [HttpPost]
        [Route("intStream")]
        public async IAsyncEnumerable<int> IntStream([FromBody] int jobLength)
        {
            for (int i = 0; i < jobLength; i++)
            {
                await Task.Delay(500); //Simulate job
                yield return i;
            }
        }
    }
}
