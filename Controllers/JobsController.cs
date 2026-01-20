using Microsoft.AspNetCore.Mvc;

namespace AJPS_API.Controllers
{
    public record Employee(int Id, string Name, int DepartmentId, decimal Salary);
    public record Department(int Id, string DepartmentName);
    public record DeptSalaryResult(int DeptId, decimal AvgSalary);
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

        public string? Ret1(string? text)
        {
            return text?.ToUpper() ?? "Lack of data";
        }
            
        public List<IGrouping<string?, Employee>> Ret2(List<Employee> employees)
        {
            return employees.GroupBy(eb => eb.Department).ToList();
        }

        public List<DeptSalaryResult> Ret3(List<Employee> employees)
        {
            return employees.GroupBy(eb => eb.DepartmentId).Select(eb => new DeptSalaryResult{ DeptId = eb.Key, AvgSalary = eb.Average(b => b.Salary)}).ToList();
        }

        public List<Employee> Ret4()
    }
}
