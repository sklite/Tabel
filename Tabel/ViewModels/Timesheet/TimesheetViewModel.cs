using System;
using System.ComponentModel.DataAnnotations;

namespace Tabel.ViewModels.Timesheet
{
    public class TimesheetViewModel : IDataEditViewModel
    {

        public int TimesheetId { get; set; }

        public int EmployeeId { get; set; }
        
        public TsEmployeeViewModel Employee { get; set; }

        public int ProjectId { get; set; }

        public TsProjectViewModel Project { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int Hours { get; set; }
    }
}