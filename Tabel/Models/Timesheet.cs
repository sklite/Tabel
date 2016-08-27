using System;
using System.ComponentModel.DataAnnotations;

namespace Tabel.Models
{
    public class Timesheet
    {
        [Key]
        public int Id { get; set; }

        public virtual Employee Employee { get; set; }

        public DateTime Date { get; set; }

        public int Hours { get; set; }

        public virtual Project Project { get; set; }

    }
}