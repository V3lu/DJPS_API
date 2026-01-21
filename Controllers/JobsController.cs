using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace AJPS_API.Controllers
{
    public record Employee(int Id, string Name, int DepartmentId, decimal Salary);
    public record Department(int Id, string DepartmentName);
    public record DeptSalaryResult(int DeptId, decimal AvgSalary);
    public enum Season { Spring, Summer, Autumn, Winter }
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public Book(string title, string author)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                throw new ArgumentException("Bad argument");
            }
            this.Title = title;
            this.Author = author;
        }
    }



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

        public List<string> Ret4(List<Employee> employees, List<Department> departments)
        {
            return employees.Join(departments,
                emp => emp.DepartmentId,
                dept => dept.Id,
                (emp, dept) => $"Pracownik {emp.Name} pracuje w dziale {dept.DepartmentName}").ToList();
        }

        public int SafeParse(string input)
        {
            if (int.TryParse(input, out int result))
            {
                return result;
            }
            return 0;
        }

        public Dictionary<string, int> CountWords(string longText)
        {
            var stats = new Dictionary<string, int>();
            string[] words = longText.ToLower().Split(new[] { ' ', '.', ',', '!' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (stats.ContainsKey(word))
                {
                    stats[word]++;
                }
                else
                {
                    stats[word] = 1;
                }
            }

            return stats;
        }

        public string Ret5(decimal price, int discount)
        {
            return $"Cena: {price:C2} (Obniżka: {discount}%)";
        }

        public List<int> Ret6(List<int> list)
        {
            List<int> ret = list.Where(eb => eb % 2 != 0).Reverse().ToList();
            return ret;
            
        }

        public bool CanEnterClub(int age, bool hasReservation, bool isVip)
        {
            return age > 18 && (hasReservation || isVip);
        }

        public string Combine(params string[] items)
        {
            return string.Join(", ", items);
        }

        public void CheckGrades(List<int> grades)
        {
            // 1. Sprawdź czy jest jakakolwiek 1
            bool is1 = grades.Any(eb => eb == 1);
            // 2. Sprawdź czy wszystkie są > 2
            bool isAll2 = grades.All(eb => eb == 2);
        }

        public string GetStatusMessage(Season season) => season switch
        {
            Season.Spring => "Spring",
            Season.Summer => "summer",
            _ => "Default"
        };

        public void ExtractPathInfo(string path)
        {
            Console.WriteLine(Path.GetFileName(path));
            Console.WriteLine(Path.GetFileNameWithoutExtension(path));
            Console.WriteLine(Path.GetExtension(path));
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        public class Sale
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public void AnalyzeSales()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Kawa", Price = 20 },
                new Product { Id = 2, Name = "Herbata", Price = 15 }
            };

            var sales = new List<Sale>
            {
                new Sale { ProductId = 1, Quantity = 2 }, // 40 zł
                new Sale { ProductId = 1, Quantity = 3 }, // 60 zł -> Razem Kawa: 100 zł
                new Sale { ProductId = 2, Quantity = 1 }  // 15 zł -> Razem Herbata: 15 zł
            };

            // TWOJE ZADANIE: Napisz zapytanie LINQ
            var result = products.Join(sales, prod => prod.Id, sale => sale.ProductId, (prod, sale) => new { prod, sale }).GroupBy(eb => eb.prod.Name).Select(
                group => new
                {
                    ProductName = group.Key,
                    TotalRevenue = group.Sum(eb => eb.sale.Quantity * eb.prod.Price)
                }).ToList();
        }
    }
}
