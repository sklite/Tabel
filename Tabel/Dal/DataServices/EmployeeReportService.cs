using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Tabel.ViewModels;

namespace Tabel.Dal.DataServices
{
    public class EmployeeReportService
    {
        TabelContext _tabelContext;


        public EmployeeReportService(TabelContext context)
        {
            _tabelContext = context;
            DateBegin = DateTime.Today.AddDays(-15);
            DateEnd = DateTime.Today;
        }

        public EmployeeReportViewModel GetData()
        {
            var result = new EmployeeReportViewModel();

            
            var span = DateEnd - DateBegin;

            int countDays = span.Days;

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
                ProjectName = ts.Project.Name,
                ProjectCode = ts.Project.Code,
                Rate = ts.Employee.Rate,
                ts.Project.WorkObject,
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
                    neededData.Where(data => data.EmployeeName == employee).Select(data => data.ProjectName).Distinct().ToList();
            }

            //Заполняем тут данные о сотрудниках и проектах
            List<ErReportEmployee> reportEmployee = new List<ErReportEmployee>();
            foreach (var employeeProject in employeeProjectDict)
            {

                var newReportEmploy = new ErReportEmployee
                {
                    EmployeeName = employeeProject.Key,
                    ReportProject = new List<ErReportProject>()
                };
                foreach (var project in employeeProject.Value)
                {
                    newReportEmploy.ReportProject.Add(new ErReportProject()
                    {
                        ProjectName = project,
                        ProjectCode = neededData.Where(data => data.ProjectName == project).Select(data=>data.ProjectCode).FirstOrDefault()
                    });
                }
                newReportEmploy.SetBeginAndEndDate(DateBegin, DateEnd);


                reportEmployee.Add(newReportEmploy);
            }

            //Записываем данные о часах
            foreach (var employee in reportEmployee)
            {
                foreach (var project in employee.ReportProject)
                {
                    var workingHours =
                        neededData.Where(
                            data => data.EmployeeName == employee.EmployeeName && data.ProjectName == project.ProjectName)
                            .Select(data =>
                                new
                                {
                                    WorkDate = data.Date,
                                    WorkHour = data.Hours,
                                    data.WorkObject,
                                }).ToList();
                    project.WorkObject = workingHours.First().WorkObject;
                    foreach (var workingHour in workingHours)
                    {
                        var roundedDate = new DateTime(workingHour.WorkDate.Year,workingHour.WorkDate.Month, workingHour.WorkDate.Day);
                        project.Hours[roundedDate] = workingHour.WorkHour;
                    }

                }
            }


            //Записываем данные о зарплате
            foreach (var employee in reportEmployee)
            {
                //var employees = reportEmployee.Where(data => data.EmployeeName == employee.EmployeeName);
                employee.Rate = neededData.Where(data => data.EmployeeName == employee.EmployeeName)
                        .Select(data => data.Rate)
                        .FirstOrDefault();
            }



            //Переделываем в формат грида
            result.Rows = new List<ErEmployeeViewModel>();

            var totalHoursDict = new Dictionary<DateTime, int>();
            var totalMoney = 0.0;

            for (int i = 0; i < countDays; i++)
            {
                totalHoursDict[DateBegin.AddDays(i)] = 0;
            }

            foreach (var reportEmploy in reportEmployee)
            {
                var hoursDict = new Dictionary<DateTime, int>();

                for (int i = 0; i < countDays; i++)
                {
                    hoursDict[DateBegin.AddDays(i)] = 0;
                }


                foreach (var reportProject in reportEmploy.ReportProject)
                {

                    var newEmployee = new ErEmployeeViewModel
                    {
                        Name = reportEmploy.EmployeeName,
                        Rate = reportEmploy.Rate,
                        Project = reportProject.ProjectName,
                        ProjectCode = reportProject.ProjectCode,
                        WorkObject = reportProject.WorkObject,
                        Hours = reportProject.Hours.Values.ToList()
                    };

                    result.Rows.Add(newEmployee);
                    totalMoney += newEmployee.Money;

                    foreach (var hour in reportProject.Hours)
                    {
                        if (!hoursDict.ContainsKey(hour.Key))
                            hoursDict[hour.Key] = 0;
                        hoursDict[hour.Key] += hour.Value;

                        if (!totalHoursDict.ContainsKey(hour.Key))
                            totalHoursDict[hour.Key] = 0;
                        totalHoursDict[hour.Key] += hour.Value;
                    }


                }

                result.Rows.Add(new ErEmployeeViewModel
                {
                    Hours = hoursDict.Values.ToList(),
                    Name = reportEmploy.EmployeeName + " Итого",
                    //Project = "По всем проектам",
                    //WorkObject = "По всем объектам",
                    Rate = reportEmploy.Rate
                });
            }

            

            //result.Rows.Add(new ErEmployeeViewModel(totalMoney)
            //{
            //    Hours = totalHoursDict.Values.ToList(),
            //    Name = "Итого",
            //   // Project = "По всем проектам"
            //});

            return result;
        }

        public void Dispose()
        {
            _tabelContext.Dispose();
        }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }
    }

    class ErReportEmployee
    {
        public string EmployeeName { get; set; }

        public double Rate { get; set; }

        public List<ErReportProject> ReportProject { get; set; }


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

    class ErReportProject
    {
        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }

        public string WorkObject { get; set; }

        public Dictionary<DateTime, int> Hours { get; set; }
    }

}