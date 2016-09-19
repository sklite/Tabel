using System.Collections.Generic;
using System.Linq;

namespace Tabel.ViewModels.ProjectReport
{
    public class ProjectReportViewModel : IDataEditViewModel
    {
        public List<PrProjectViewModel> Rows { get; set; }


        public List<string> MyColumns { get; set; }
    }

    public class PrProjectViewModel
    {
        double totalMoney = 0;

        public PrProjectViewModel(double totalMoney)
        {
            this.totalMoney = totalMoney;
        }

        public PrProjectViewModel()
        {

        }

        public void SetTotalMoney(double totalMoney)
        {
            this.totalMoney = totalMoney;
        }


        public string Name { get; set; }

        public string Project { get; set; }

        public string WorkObject { get; set; }

        public double Rate { get; set; }
        public double Money
        {
            get
            {
                if (totalMoney == 0)
                    return Rate * (Hours.Sum());
                return totalMoney;
            }
        }
        public int TotalHours => Hours.Sum();

        public List<string> MySecondCol { get; set; }

        public List<int> Hours { get; set; }
    }
}