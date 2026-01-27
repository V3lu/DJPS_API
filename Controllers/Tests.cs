using System;
using System.Collections.Generic;
using System.Linq;

namespace AJPS_API.Controllers
{

    public record Person(string Name, string City);

    public class Department1
    {
        public string Name { get; set; }
        public List<string> Employees { get; set; }
    }

    public class LinqMasterClass
    {
        public void RunExercises()
        {          
            var people = new List<Person>
            {
                new("Jan", "Warszawa"),
                new("Ania", "Kraków"),
                new("Piotr", "Warszawa"),
                new("Kasia", "Gdańsk")
            };

            var movedPeople = people.Select(x => x.City == "Warszawa" ? x with { City = "Kraków" } : x).ToList();

            string secret = "abracadabra";
            var uniqueSorted = secret.Distinct().OrderBy(x => x);
            string result = string.Concat(uniqueSorted);

            List<int> grades = new List<int>{ 45, 76, 88, 30, 92, 55, 61 };
            var query = grades.GroupBy(x => x < 50 ? "Niezaliczone" : "Zaliczone").ToList();


            var departments = new List<Department1>
            {
                new() { Name = "IT", Employees = new() { "Tomek", "Marek" } },
                new() { Name = "HR", Employees = new() { "Ewa", "Kasia" } },
                new() { Name = "Marketing", Employees = new() { "Adam" } }
            };

            var allEmployees = departments.SelectMany(d => d.Employees, (dep, employee) => $"{dep} with employee {employee}");
        }
    }
}
