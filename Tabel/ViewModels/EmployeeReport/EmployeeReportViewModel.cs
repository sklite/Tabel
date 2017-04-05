using System;
using System.Collections.Generic;
using System.Linq;

namespace Tabel.ViewModels
{
    public class EmployeeReportViewModel : IDataEditViewModel
    {
        public List<ErEmployeeViewModel> Rows { get; set; }

        public List<string> Employees { get; set; }


        public List<string> MyColumns { get; set; } 
    }

    public class ErEmployeeViewModel
    {
        double totalMoney = 0;

        public ErEmployeeViewModel(double totalMoney)
        {
            this.totalMoney = totalMoney;
        }

        public ErEmployeeViewModel()
        {
            
        }


        public string Name { get; set; }


        public string ProjectCode { get; set; }
        /// <summary>
        /// Имя проекта
        /// </summary>
        public string Project { get; set; }

        public string WorkObject { get; set; }

        public double Rate { get; set; }
        public double Money {
            get
            {
                if (totalMoney == 0)
                    return Rate*(Hours.Sum());
                return totalMoney;
            }
        }
        public int TotalHours => Hours.Sum();

        public List<string> MySecondCol { get; set; }

        public List<int> Hours { get; set; }
    }
}