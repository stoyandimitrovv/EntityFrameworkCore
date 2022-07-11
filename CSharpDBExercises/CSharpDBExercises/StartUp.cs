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
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = GetEmployee147(dbContext);
            //Console.WriteLine(answer);

            //Problem 10
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = GetDepartmentsWithMoreThan5Employees(dbContext);
            //Console.WriteLine(answer);

            //Problem 11
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = GetLatestProjects(dbContext);
            //Console.WriteLine(answer);

            //Problem 12
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = IncreaseSalaries(dbContext);
            //Console.WriteLine(answer);

            //Problem 13
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = GetEmployeesByFirstNameStartingWithSa(dbContext);
            //Console.WriteLine(answer);

            //Problem 14
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = DeleteProjectById(dbContext);
            //Console.WriteLine(answer);

            //Problem 15
            //using SoftUniContext dbContext = new SoftUniContext();
            //string answer = RemoveTown(dbContext);
            //Console.WriteLine(answer);
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
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departmentMoreThanFive = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepName = d.Name,
                    ManaFirstName = d.Manager.FirstName,
                    ManaLastName = d.Manager.LastName,
                    Employees = d.Employees
                        .Select(e => new
                        {
                            EmplFirstName = e.FirstName,
                            EmplLastName = e.LastName,
                            EmpJobTitle = e.JobTitle
                        })
                        .OrderBy(e => e.EmplFirstName)
                        .ThenBy(e => e.EmplLastName)
                        .ToArray()
                })
                .ToArray();

            foreach (var d in departmentMoreThanFive)
            {
                sb.AppendLine($"{d.DepName} - {d.ManaFirstName}  {d.ManaLastName}");

                foreach (var e in d.Employees)
                {
                    sb.AppendLine($"{e.EmplFirstName} {e.EmplLastName} - {e.EmpJobTitle}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //Problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    Name = p.Name,
                    Descroption = p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                })
                .OrderBy(p => p.Name)
                .ToArray();

            foreach (var d in projects)
            {
                sb.AppendLine($"{d.Name}\n{d.Descroption}\n{d.StartDate}");
            }
            return sb.ToString().TrimEnd();
        }

        //Problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var salaries = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design"
                                                               || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .ToList()
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);



            foreach (var s in salaries)
            {
                s.Salary *= 1.12M;
                sb.AppendLine($"{s.FirstName} {s.LastName} (${s.Salary:f2})");
            }
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        //Problem 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var findEmployee = context.Employees
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();


            foreach (var s in findEmployee)
            {
                sb.AppendLine($"{s.FirstName} {s.LastName} - {s.JobTitle} - (${s.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Project projTodelete = context
                .Projects
                .Find(2);

            var referensEmplo = context.EmployeesProjects
                .Where(e => e.ProjectId == projTodelete.ProjectId)
                .ToArray();

            context.EmployeesProjects.RemoveRange(referensEmplo);
            context.Projects.Remove(projTodelete);
            context.SaveChanges();

            var projectNames = context
                .Projects
                .Take(10)
                .Select(p => p.Name)
                .ToArray();

            foreach (var p in projectNames)
            {
                sb.AppendLine($"{p}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 15
        public static string RemoveTown(SoftUniContext context)
        {
            var townToDelete = context
                .Towns
                .First(t => t.Name == "Seattle");

            IQueryable<Address> addressesToDelete =
                context
                    .Addresses
                    .Where(a => a.TownId == townToDelete.TownId);

            int addressesCount = addressesToDelete.Count();

            IQueryable<Employee> employeesOnDeletedAddresses =
                context
                    .Employees
                    .Where(e => addressesToDelete.Any(a => a.AddressId == e.AddressId));

            foreach (var employee in employeesOnDeletedAddresses)
            {
                employee.AddressId = null;
            }

            foreach (var address in addressesToDelete)
            {
                context.Addresses.Remove(address);
            }

            context.Remove(townToDelete);

            context.SaveChanges();

            return $"{addressesCount} addresses in Seattle were deleted";

        }
    }
}
