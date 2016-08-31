using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Tabel.ViewModels.Report
{
    public class ReportViewModel
    {
        //public int ReportId { get; set; }

        public string EmployeeName { get; set; }
        public string ProjectCode { get; set; }

        public int Hours { get; set; }
        public double Rate { get; set; }

        //[DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public double Money => Hours * Rate;

        public string DateShort => Date.ToShortDateString();
    }
}