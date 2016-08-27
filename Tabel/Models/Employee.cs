using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tabel.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Pass { get; set; }

        public string Email { get; set; }

        public virtual Role Role { get; set; }
        
        public double Rate { get; set; }

    }
}