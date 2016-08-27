using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Tabel.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }
         
    }
}