using System.Collections.Generic;
using Tabel.Models;

namespace Tabel.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Tabel.Dal.TabelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Tabel.Dal.TabelContext context)
        {
            //  This method will be called after migrating to the latest version.
            var roles = new List<Role>
            {
                new Role
                {
                    Name = "Менеджер",
                    Id = 1
                },
                new Role
                {
                    Name = "Работник",
                    Id = 2
                }
            };
            context.Roles.AddOrUpdate(roles.ToArray());


            var projects = new List<Project>
            {
                new Project
                {
                    Name = "Работы на скважине",
                    WorkObject = "Когалым"
                },
                new Project
                {
                    Name = "Работы в поле",
                    WorkObject = "Когалым"
                },
                new Project
                {
                    Name = "Администрация",
                    WorkObject = "Москва"
                },
                new Project()
                {
                    Name = "MP",
                    WorkObject = "Торговый центр"
                }
             };
            context.Projects.AddOrUpdate(projects.ToArray());

            var employees = new List<Employee>
            {
                new Employee
                {
                    Email = "sklite@ya.ru",
                    Name = "Алексей",
                    Pass = "123",
                    Role = roles[0],
                    Rate = 200,
                    Id = 1
                },
                new Employee
                {
                    Email = "иванов@я.ру",
                    Name = "Иванов",
                    Pass = "123",
                    Role = roles[1],
                    Rate = 100,
                    Id = 2
                },
                new Employee
                {
                    Email = "blabla@mail.com",
                    Name = "George",
                    Pass = "123",
                    Role = roles[1],
                    Rate = 150,
                    Id=3
                }
            };
            
            context.Employees.AddOrUpdate(employees.ToArray());



            var timesheets = new List<Timesheet>
            {
                new Timesheet
                {
                    Date = DateTime.Today,
                    Employee = employees[0],
                    Hours = 8,
                    Project = projects[0]
                },
                new Timesheet
                {
                    Date = DateTime.Today.AddDays(-1),
                    Employee = employees[0],
                    Hours = 8,
                    Project = projects[0]
                },
                new Timesheet
                {
                    Date = DateTime.Today,
                    Employee = employees[1],
                    Hours = 8,
                    Project = projects[1]
                },
                new Timesheet
                {
                    Date = DateTime.Today.AddDays(-1),
                    Employee = employees[1],
                    Hours = 2,
                    Project = projects[2]
                },
                new Timesheet
                {
                    Date = DateTime.Today.AddDays(-2),
                    Employee = employees[2],
                    Hours = 2,
                    Project = projects[2]
                },
                new Timesheet
                {
                    Date = DateTime.Today.AddDays(-3),
                    Employee = employees[2],
                    Hours = 2,
                    Project = projects[3]
                },
                new Timesheet
                {
                    Date = DateTime.Today.AddDays(-4),
                    Employee = employees[2],
                    Hours = 3,
                    Project = projects[2]
                },
                new Timesheet
                {
                    Date = DateTime.Today.AddDays(-1),
                    Employee = employees[2],
                    Hours = 2,
                    Project = projects[2]
                },
                new Timesheet
                {
                    Date = DateTime.Today.AddDays(-2),
                    Employee = employees[2],
                    Hours = 5,
                    Project = projects[1]
                },
                new Timesheet
                {
                    Date = DateTime.Today.AddDays(-3),
                    Employee = employees[2],
                    Hours = 2,
                    Project = projects[2]
                },
                new Timesheet
                {
                    Date = DateTime.Today.AddDays(-4),
                    Employee = employees[2],
                    Hours = 4,
                    Project = projects[2]
                },
                new Timesheet
                {
                    Date = DateTime.Today.AddDays(-6),
                    Employee = employees[2],
                    Hours = 2,
                    Project = projects[1]
                },
            };

            context.Timesheets.AddOrUpdate(timesheets.ToArray());


            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
