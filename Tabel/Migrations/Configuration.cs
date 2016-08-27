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



            context.Employees.AddOrUpdate(new Employee
            {
                Email = "sklite@ya.ru",
                Name = "Алексей",
                Pass = "123",
                Role = roles[0],
                Rate = 200
            }, new Employee {
                Email = "иванов@я.ру",
                Name = "Иванов",
                Pass = "123",
                Role = roles[1],
                Rate = 100
            }
            );


            context.Roles.AddOrUpdate(roles.ToArray());

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
