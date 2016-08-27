using System;
using System.ComponentModel.DataAnnotations;

namespace Tabel.ViewModels
{
    public class TimesheetViewModel
    {

        public int TimesheetId { get; set; }

        public int Employee { get; set; }

        public int Project { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int Hours { get; set; }
    }
}