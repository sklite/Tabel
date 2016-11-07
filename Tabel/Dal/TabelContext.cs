using System.Data.Entity;
using System.Security.AccessControl;
using System.Security.Policy;
using Tabel.Models;

namespace Tabel.Dal
{
    public class TabelContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }

//#if Release
        public TabelContext() //: base("Tabel")
        {

        }
//#endif
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Project>().ToTable("Projects");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Timesheet>().ToTable("Timesheet");
        }

        /*
         * 
         *             context.Employees.AddOrUpdate(new Employee 
            {
                Email = "sklite@ya.ru",
                Name = "Алексей",
                Pass = "123",
                PositionId = 1
            });
         * */
    }
}