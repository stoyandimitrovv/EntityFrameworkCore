using System;
using System.Linq;
using System.Text;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //Problem 3
            //SoftUniContext dbContext = new SoftUniContext();
            //string result = GetEmployeesFullInformation(dbContext);
            //Console.WriteLine(result);

            //Problem 4
            //SoftUniContext dbContext = new SoftUniContext();
            //string answer = GetEmployeesWithSalaryOver50000(dbContext);
            //Console.WriteLine(answer);

            //Problem 5
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = GetEmployeesFromResearchAndDevelopment(dbContext);
            //Console.WriteLine(answer);

            //Problem 6
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = AddNewAddressToEmployee(dbContext);
            //Console.WriteLine(answer);

            //Problem 7
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = GetEmployeesInPeriod(dbContext);
            //Console.WriteLine(answer);

            //Problem 8
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = GetAddressesByTown(dbContext);
            //Console.WriteLine(answer);

            //Problem 9
            using SoftUniContext dbContext = new SoftUniContext();
            string answer = GetEmployee147(dbContext);
            Console.WriteLine(answer);

            //Problem 10

            //Problem 11

            //Problem 12

            //Problem 13

            //Problem 14

        }

        //Problem 3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();

            Employee[] allEmployees = context
                .Employees
                .OrderBy(e => e.EmployeeId)
                .ToArray();

            foreach (var e in allEmployees)
            {
                output.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return output.ToString().TrimEnd();
        }

        //Problem 4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var allEmployees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .Where(e => e.Salary > 50000)
                .ToArray()
                .OrderBy(e => e.FirstName);

            foreach (var e in allEmployees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var answer = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var e in answer)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(newAddress);

            Employee nakov = context.Employees.First(e => e.LastName == "Nakov");

            nakov.Address = newAddress;

            context.SaveChanges();

            var adress = context.Employees.OrderByDescending(e => e.AddressId)
                .Take(10).Select(e => e.Address.AddressText).ToArray();

            foreach (var adr in adress)
            {
                sb.AppendLine($"{adr}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var emplyesAndProjects = context.Employees
                .Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001
                                                         && p.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFName = e.Manager.FirstName,
                    ManagerLName = e.Manager.LastName,
                    AllProjects = e.EmployeesProjects
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate
                                .ToString("M/d/yyyy h:mm:ss tt"),
                            EndDate = ep.Project.EndDate.HasValue
                                ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt")
                                : "not finished"
                        }).ToArray()
                })
                .ToArray();

            foreach (var emp in emplyesAndProjects)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - Manager: {emp.ManagerFName} {emp.ManagerLName}");

                foreach (var pro in emp.AllProjects)
                {
                    sb.AppendLine($"--{pro.ProjectName} - {pro.StartDate} - {pro.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var adressesByTown = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(a => new
                {
                    NumberEmployees = a.Employees.Count,
                    TownName = a.Town.Name,
                    a.AddressText
                })
                .Take(10)
                .ToArray();

            foreach (var ad in adressesByTown)
            {
                sb.AppendLine($"{ad.AddressText}, {ad.TownName} - {ad.NumberEmployees} employees");
            }
            return sb.ToString().TrimEnd();
        }

        //Problem 9
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeeN = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FName = e.FirstName,
                    LName = e.LastName,
                    JTitle = e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Select(p => new
                        {
                            ProjectName = p.Project.Name
                        })
                        .OrderBy(x => x.ProjectName)
                        .ToArray()
                })
                .ToArray();

            foreach (var ad in employeeN)
            {
                sb.AppendLine($"{ad.FName} {ad.LName} - {ad.JTitle}");

                foreach (var item in ad.Projects)
                {
                    sb.AppendLine($"{item.ProjectName}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //Problem 10


        //Problem 11


        //Problem 12


        //Problem 13


        //Problem 14


    }
}
