using System;
using System.Collections.Generic;
using System.Linq;
using Tabel.ViewModels;

namespace Tabel.Dal.DataServices
{
    public class EmployeeReportService
    {
        TabelContext _tabelContext;


        public EmployeeReportService(TabelContext context)
        {
            _tabelContext = context;
            DateBegin = DateTime.Today.AddMonths(-1);
            DateEnd = DateTime.Today;
        }

        public EmployeeReportViewModel GetData()
        {
            var result = new EmployeeReportViewModel();

            
            var span = DateEnd - DateBegin;

            int countDays = span.Days;
            Tuple<int, int> sdf;

            result.MyColumns = new List<string>();
            for (int i = 0; i < countDays; i++)
            {
                result.MyColumns.Add(DateBegin.AddDays(i).ToShortDateString());
            }

            var timesheetsInterval =
                _tabelContext.Timesheets.Include("Employee")
                    .Include("Project")
                    .Where(ts => ts.Date > DateBegin && ts.Date < DateEnd);


            var neededData = timesheetsInterval.Select(ts => new
            {
                EmployeeName = ts.Employee.Name,
                Project = ts.Project.Name,
                Rate = ts.Employee.Rate,
                ts.Hours,
                ts.Date
            });


            List<DateTime> reportDays = new List<DateTime>();

            for (int i = 0; i < countDays; i++)
            {
                reportDays.Add(DateBegin.AddDays(i));
            }

            var allEmployees = neededData.Select(data => data.EmployeeName).Distinct();

            var employeeProjectDict = new Dictionary<string, List<string>>();


            foreach (var employee in allEmployees)
            {
                employeeProjectDict[employee] =
                    neededData.Where(data => data.EmployeeName == employee).Select(data => data.Project).Distinct().ToList();
            }

            //Заполняем тут данные о сотрудниках и проектах
            List<ReportEmployee> reportEmployee = new List<ReportEmployee>();
            foreach (var employeeProject in employeeProjectDict)
            {

                var newReportEmploy = new ReportEmployee
                {
                    EmployeeName = employeeProject.Key,
                    ReportProject = new List<ReportProject>()
                };
                foreach (var project in employeeProject.Value)
                {
                    newReportEmploy.ReportProject.Add(new ReportProject()
                    {
                        ProjectCode = project
                    });
                }
                newReportEmploy.SetBeginAndEndDate(DateBegin, DateEnd);


                reportEmployee.Add(newReportEmploy);
            }

            //Запихали данные о часах
            foreach (var employee in reportEmployee)
            {
                foreach (var project in employee.ReportProject)
                {
                    var workingHours =
                        neededData.Where(
                            data => data.EmployeeName == employee.EmployeeName && data.Project == project.ProjectCode)
                            .Select(data =>
                                new
                                {
                                    WorkDate = data.Date,
                                    WorkHour = data.Hours
                                }).ToList();

                    foreach (var workingHour in workingHours)
                    {
                        project.Hours[workingHour.WorkDate] = workingHour.WorkHour;
                    }

                }
            }


            //Записываем данные о зарплате
            foreach (var employee in reportEmployee)
            {
                employee.Rate =
                    neededData.Where(data => data.EmployeeName == employee.EmployeeName)
                        .Select(data => data.Rate)
                        .FirstOrDefault();
            }



            //Переделываем в формат грида

            //result.MyColumns = reportEmployee[0].ReportProject[0].Hours.Keys.Select(date => date.ToShortDateString()).ToList();

            result.Rows = new List<ErEmployeeViewModel>();
            foreach (var reportEmploy in reportEmployee)
            {
                foreach (var reportProject in reportEmploy.ReportProject)
                {
                    result.Rows.Add(new ErEmployeeViewModel
                    {
                        Name = reportEmploy.EmployeeName,
                        Rate = reportEmploy.Rate,
                        Project = reportProject.ProjectCode,
                        Hours = reportProject.Hours.Values.ToList()
                    });

                }
            }
            
            
            //result.MyColumns = new List<string>
            //{
            //    "10","11","12","13"
            //};
            //result.Rows = new List<ErEmployeeViewModel>()
            //{
            //    new ErEmployeeViewModel
            //    {
            //        Name = "Лёша",
            //        Project = "Люксофт",
            //        Rate = 100,
            //        MySecondCol = new List<string>() { "10", "11", "12", "13" },
            //        Hours = new List<int> {2, 3, 6, 3}
            //    },
            //    new ErEmployeeViewModel
            //    {
            //        Name = "Лёша",
            //        Project = "Литтраст",
            //        Rate = 50,
            //        MySecondCol = new List<string>() { "10", "11", "12", "13" },
            //        Hours = new List<int> {0, 1, 2, 1}
            //    },
            //    new ErEmployeeViewModel
            //    {
            //        Name = "Иван",
            //        Project = "Без проекта",
            //        Rate = 70,
            //        MySecondCol = new List<string>() { "10", "11", "12", "13" },
            //        Hours = new List<int> {4, 1, 3, 25}
            //    },
            //    new ErEmployeeViewModel
            //    {
            //        Name = "Павел",
            //        Project = "Прогресстех",
            //        Rate = 23,
            //        MySecondCol = new List<string>() { "10", "11", "12", "13" },
            //        Hours = new List<int> {40, 10, 13, 225}
            //    },
            //};
            


            return result;
        }

        public void Dispose()
        {
            _tabelContext.Dispose();
        }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }
    }

    class ReportEmployee
    {
        public string EmployeeName { get; set; }

        public double Rate { get; set; }

        public List<ReportProject> ReportProject { get; set; }


        /// <summary>
        /// Заполнить нулями промежутки дат
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public void SetBeginAndEndDate(DateTime begin, DateTime end)
        {
            var span = end - begin;
            var countDays = span.Days;


            foreach (var reportProject in ReportProject)
            {
                reportProject.Hours = new Dictionary<DateTime, int>();
                for (int i = 0; i < countDays; i++)
                {
                    var resultdate = begin.AddDays(i);
                    reportProject.Hours[resultdate] = 0;
                }
            }
        }
    }

    class ReportProject
    {
        public string ProjectCode { get; set; }

        public Dictionary<DateTime, int> Hours { get; set; }
    }

}