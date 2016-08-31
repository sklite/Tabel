using System;
using System.Collections.Generic;
using System.Linq;

namespace Tabel.ViewModels
{
    public class EmployeeReportViewModel : IDataEditViewModel
    {
        public List<ErEmployeeViewModel> Rows { get; set; }


        public List<string> MyColumns { get; set; } 
    }

    public class ErEmployeeViewModel
    {
        public string Name { get; set; }

        public string Project { get; set; }

        public double Rate { get; set; }
        public double Money => Rate*Hours.Sum();

        public List<string> MySecondCol { get; set; }

        public List<int> Hours { get; set; }
    }
}