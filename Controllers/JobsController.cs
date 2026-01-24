using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AJPS_API.Controllers
{
    public record Department(int Id, string DepartmentName);
    public record DeptSalaryResult(int DeptId, decimal AvgSalary);
    public enum Season { Spring, Summer, Autumn, Winter }
    public record Product(string Name, string Category, decimal Price);
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
            return employees.GroupBy(eb => eb.DepartmentId).Select(eb => new DeptSalaryResult { DeptId = eb.Key, AvgSalary = eb.Average(b => b.Salary) }).ToList();
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

        public class StudentGrade
        {
            private string name;
            public string Name { get; private set; }
            public int? Score { get; private set; }
        }

        public void PrintResult(StudentGrade student)
        {
            string displayScore = student.Score?.ToString() ?? "Lack of mark";
            Console.WriteLine($"Student received {displayScore}");
        }

        public void BirthdayInfo(DateTime birthDate)
        {
            string readText = System.IO.File.ReadAllText("pathToFile");
        }

        public void TryOpenFile(string path)
        {
            try
            {
                string content = System.IO.File.ReadAllText(path);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("End of operation");
            }
        }

        public record UserDTO(string firstName, string lastName);

        public void Ret7()
        {
            UserDTO userDTO = new UserDTO(firstName: "das", lastName: "das");
            userDTO.firstName = "dsa";  //Compilator does not allow it
            var user2 = userDTO with { firstName = "dasfas" };
        }

        public List<string> Ret8(List<string> names)
        {
            return names.Select(eb => eb.Trim()).Select(eb => eb.ToUpper()).OrderBy(eb => eb).ToList();
        }
        Dictionary<string, decimal> prices = new() { { "Chleb", 4.50m }, { "Mleko", 3.20m } }
        public void PrintPrice(string productName)
        {
            if (prices.TryGetValue(productName, out decimal price))
            {
                Console.WriteLine($"Price: {price}");
            }
            else
            {
                Console.WriteLine("No such an item");
            }
        }

        public interface IDiscountStrategy
        {
            decimal ApplyDiscount(decimal price);
        }

        public class PercentageDiscount : IDiscountStrategy
        {
            public decimal ApplyDiscount(decimal price)
            {
                return price * 0.9m;
            }
        }

        public class FixedDiscount : IDiscountStrategy
        {
            public decimal ApplyDiscount(decimal price)
            {
                return price - 5m;
            }
        }

        public void Temperature(double? currentTemp)
        {
            double temp = currentTemp ?? 0.0;

            if (temp > 25.0)
            {
                Console.WriteLine("Turn on air conditioning");
            }
            else
            {
                Console.WriteLine("Temperature at the normal rate");
            }
        }

        public record Car(string Brand, int Year);

        List<Car> cars = new List<Car>
        {
            new Car("Audi", 2010),
            new Car("Toyota", 2020),
            new Car("Fiat", 2005)
        };

        List<Car> oldCars = cars.Where(eb => eb.Year < 2015).Select(eb => eb with { Brand = "Old " + eb.Brand}).ToList();

        public async Task DownloadDataAsync()
        {
            Console.WriteLine("Download started");
            await Task.Delay(3000);
            Console.WriteLine("Data downloaded");
        }

        public async Task<string> SimulateTaskAsync()
        {
            await Task.Delay(1000);
            return "Work completed";
        }

        public async Task<int> GetStockAmountAsync(string productName)
        {
            await Task.Delay(1500);

            if (productName == "Laptop")
            {
                return 5;
            }
            else
            {
                return 0;
            }
        }

        public class InsufficientFundsException : Exception
        {
            public InsufficientFundsException(string message) : base(message)
            {
                
            }
        }

        public void Withdraw(decimal balance, decimal amount)
        {
            try
            {
                if (amount > balance)
                {
                    throw new InsufficientFundsException("Lack of funds");
                }
            }
            catch (InsufficientFundsException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public class DataStorage<T>
        {
            private List<T> _list;

            public void Add(T item)
            {
                _list.Add(item);
            }

            public T? GetLast()
            {
                if (_list.Count != 0)
                {
                    return _list[_list.Count - 1];
                }
                else
                {
                    return default;
                }
            }

            public List<T> FindAll(Predicate<T> match)
            {
                return _list.FindAll(match);
            }
        }

        public void CreateObjects()
        {
            DataStorage<int> ints = new();
            DataStorage<string> strings = new();

            strings.FindAll(eb => eb.Equals(string.Empty)).ToList();
        }

        public void FindCategory()
        {
            List<Product> products = new List<Product> 
            {
                new ("Jabłko", "Owoce", 2.5m),
                new ("Banan", "Owoce", 3.0m),
                new ("Chleb", "Pieczywo", 4.0m),
                new ("Bułka", "Pieczywo", 1.0m)
            };

            Console.WriteLine(products.GroupBy(eb => eb.Category).Select(group => new { Category = group.Key, AvgPrice = group.Average(eb => eb.Price) }).ToList());
        }

        public void SecureExecute(Action action)
        {
            Console.WriteLine("Starting operation");
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
            finally
            {
                Console.WriteLine("Operation finished");
            }
        }

        public event Action OnClick;

        public void Press()
        {
            OnClick?.Invoke();
        }

        public void Code()
        {
            Console.WriteLine("Invoking Action");
            this.OnClick += () => Console.WriteLine("Akcja 1");

            this.Press();
        }

        public class Box<T>
        {
            public event Action<T>? OnItemStored;

            void Store(T item)
            {
                OnItemStored?.Invoke(item);
            }
        }

        public double Calculate(double a, double b, Func<double, double, double> operation)
        {
            return operation(a, b);
        }

        Calculate(5, 6, (a, b) => a * b);

        public static List<TResult> MyMap<TSource, TResult>(List<TSource> source, Func<TSource, TResult> transform)
        {

            List<TResult> list = new();

            foreach (TSource sourceItem in source)
            {
                list.Add(transform(sourceItem));
            }

            List<Func<string, string>> processors = new();

            processors.Add((str) => str.Trim());
            processors.Add((str) => str.ToUpper());
            processors.Add((str) => str + '.');
            string user = "User ";
            foreach (var func in processors)
            {
                func(user);
            }
        }

        public static List<T> MyFilter<T>(List<T> source, Func<T, bool> condition)
        {
            List<T> ret = new();

            foreach (T item in source)
            {
                if (condition(item))
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        public Dictionary<string, int> CountWords(List<string> words)
        {
            Dictionary<string, int> counts = new();

            foreach (string word in words)
            {
                if (counts.ContainsKey(word))
                {
                    counts[word]++;
                }
                else
                {
                    counts[word] = 1;
                }
            }

            return counts;
        }

        public void TryParseInt()
        {
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int result))
            {
                if (result > 0)
                {
                    Console.WriteLine("Well done");
                }
                else
                {
                    Console.WriteLine("Too small number");
                }
            }
            else
            {
                Console.WriteLine("This is not a number!");
            }
        }

        public interface IMessageSender
        {
            void Send(string message);
        }

        public class EmailSender : IMessageSender
        {
            public void Send(string message)
            {
                Console.WriteLine($"Sending Email {message}");
            }
        }

        public class SMSSender : IMessageSender
        {
            public void Send(string message)
            {
                Console.WriteLine($"Sending Sms {message}");
            }
        }

        public void Logic()
        {
            List<IMessageSender> senders = new();

            EmailSender es = new();
            SMSSender smss = new();

            senders.Add(es);
            senders.Add(smss);

            foreach (var obj in senders)
            {
                obj.Send("Message");
            }
        }

        public class UserAccount
        {
            private string _password;
            public string Password { 
                set
                {
                    if (value.Length < 6)
                    {
                        Console.WriteLine("Error - too short password");
                    }
                    else
                    {
                        _password = value;
                    }
                }
            }

            public string Username { get; set; }
        }

        public class Employee
        {
            public string Name { get; set; }
            public decimal Salary { get; set; }
            public Employee(string name, decimal salary)
            {
                this.Salary = salary;
                this.Name = name;
            }

            public virtual void WriteSalary()
            {
                Console.WriteLine(this.Salary.ToString());
            }
        }

        public class Manager : Employee
        {
            public decimal Bonus { get; set; }
            public Manager(string name, decimal salary, decimal bonus) : base(name, salary)
            {
                this.Bonus = bonus;
            }

            public override void WriteSalary()
            {
                Console.WriteLine(Salary * Bonus);
            }
        }

        public enum DayCategory { Workday, Holiday }

        public DayCategory DayOfWeek(DayOfWeek day) => day switch
        {
            System.DayOfWeek.Monday => DayCategory.Workday,
            System.DayOfWeek.Tuesday => DayCategory.Workday,
            _ => DayCategory.Workday
        };

        public static class Mathhelper
        {
            public static double CalculateAverage(List<int> numbers)
            {
                if (numbers.Count != 0 || numbers is not null) return 0; else return numbers.Average();
            }
        }
    }
}
