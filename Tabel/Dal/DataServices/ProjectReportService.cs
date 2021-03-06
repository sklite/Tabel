﻿using System;
using System.Collections.Generic;
using System.Linq;
using Tabel.ViewModels;
using Tabel.ViewModels.ProjectReport;

namespace Tabel.Dal.DataServices
{
    public class ProjectReportService
    {
        TabelContext _tabelContext;


        public ProjectReportService(TabelContext context)
        {
            _tabelContext = context;
            DateBegin = DateTime.Today.AddDays(-15);
            DateEnd = DateTime.Today;
        }

        public ProjectReportViewModel GetData()
        {
            var result = new ProjectReportViewModel();


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
                    .Where(ts => ts.Date >= DateBegin && ts.Date < DateEnd);


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

            var allProjects = neededData.Select(data => data.ProjectCode).Distinct();

            var projectEmployeeDict = new Dictionary<string, List<string>>();

            //у этого проекта такие то сотрудники
            foreach (var project in allProjects)
            {
                projectEmployeeDict[project] = neededData.Where(data => data.ProjectCode == project).Select(data => data.EmployeeName).Distinct().ToList();
            }

            //Заполняем тут данные о сотрудниках и проектах
            List<PrReportProject> reportProject = new List<PrReportProject>();
            foreach (var projectEmployee in projectEmployeeDict)
            {

                var newReportProject = new PrReportProject 
                { 
                  //  ProjectName = projectEmployee.Key,
                   ProjectCode = projectEmployee.Key,
                    ReportEmployee = new List<PrReportEmployee>()
                };


                foreach (var employee in projectEmployee.Value)
                {
                    newReportProject.ReportEmployee.Add(new PrReportEmployee()
                    {
                        EmployeeName = employee
                    });
                }
                newReportProject.SetBeginAndEndDate(DateBegin, DateEnd);


                reportProject.Add(newReportProject);
            }

            //Записываем данные о часах
            foreach (var project in reportProject)
            {
                foreach (var employee in project.ReportEmployee)
                {
                    var workingHours =
                        neededData.Where(
                            data => data.EmployeeName == employee.EmployeeName && data.ProjectCode == project.ProjectCode)
                            .Select(data =>
                                new
                                {
                                    WorkDate = data.Date,
                                    WorkHour = data.Hours,
                                    data.WorkObject,
                                    data.ProjectCode
                                }).ToList();
                    project.WorkObject = workingHours.First().WorkObject;
                    project.ProjectCode = workingHours.First().ProjectCode;
                    foreach (var workingHour in workingHours)
                    {
                        var roundedDate = new DateTime(workingHour.WorkDate.Year, workingHour.WorkDate.Month, workingHour.WorkDate.Day);
                        employee.Hours[roundedDate] = workingHour.WorkHour;
                    }
                }
            }


            //Записываем данные о зарплате  НЕДОДЕЛКА
            foreach (var project in reportProject)
            {
                foreach (var employee in project.ReportEmployee)
                {
                    employee.Rate = neededData.Where(data => data.EmployeeName == employee.EmployeeName)
                        .Select(data => data.Rate)
                        .FirstOrDefault();
                }
            }



            //Переделываем в формат грида

            result.Rows = new List<PrProjectViewModel>();

            var totalHoursDict = new Dictionary<DateTime, int>();
            var totalMoney = 0.0;

            for (int i = 0; i < countDays; i++)
            {
                totalHoursDict[DateBegin.AddDays(i)] = 0;
            }


           

            foreach (var reportPrj in reportProject)
            {
                var hoursDict = new Dictionary<DateTime, int>();

                var neededProject =
                    neededData.FirstOrDefault(data => data.ProjectCode == reportPrj.ProjectCode);
                if (neededProject != null)
                    reportPrj.ProjectName = neededProject.ProjectName;

                for (int i = 0; i < countDays; i++)
                {
                    hoursDict[DateBegin.AddDays(i)] = 0;
                }

                double projectMoney = 0;

                foreach (var reportEmploy in reportPrj.ReportEmployee)
                {

                    var newEmployee = new PrProjectViewModel
                    {
                        Name = reportEmploy.EmployeeName,
                        Rate = reportEmploy.Rate,
                        ProjectName = reportPrj.ProjectName,
                        WorkObject = reportPrj.WorkObject,
                        ProjectCode = reportPrj.ProjectCode,
                        Hours = reportEmploy.Hours.Values.ToList()
                    };

                    result.Rows.Add(newEmployee);
                    totalMoney += newEmployee.Money;

                    foreach (var hour in reportEmploy.Hours)
                    {
                        if (!hoursDict.ContainsKey(hour.Key))
                            hoursDict[hour.Key] = 0;
                        hoursDict[hour.Key] += hour.Value;

                        if (!totalHoursDict.ContainsKey(hour.Key))
                            totalHoursDict[hour.Key] = 0;
                        totalHoursDict[hour.Key] += hour.Value;
                    }


                    projectMoney += newEmployee.Money;

                }

                result.Rows = result.Rows.OrderBy(row => row.ProjectCode).ToList();

                result.Rows.Add(new PrProjectViewModel(projectMoney)
                {
                    Hours = hoursDict.Values.ToList(),
                    ProjectCode = reportPrj.ProjectCode + " Итого",
                    ProjectName = reportPrj.ProjectName + " Итого",
                  //  Rate = reportPrj.Rate   НЕДОДЕЛКА
                });
            }


            result.Projects = allProjects.ToList();
            result.Projects.Insert(0, "Все");

            if (!string.IsNullOrEmpty(ProjectFilter) && ProjectFilter != "Все")
                result.Rows.RemoveAll(item => item.ProjectCode != ProjectFilter);

            return result;
        }

        public void Dispose()
        {
            _tabelContext.Dispose();
        }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public string ProjectFilter { get; set; }
    }

    class PrReportProject
    {
        public string ProjectCode { get; set; }

        public string WorkObject { get; set; }

        public string ProjectName { get; set; }



        public List<PrReportEmployee> ReportEmployee { get; set; }


        /// <summary>
        /// Заполнить нулями промежутки дат
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public void SetBeginAndEndDate(DateTime begin, DateTime end)
        {
            var span = end - begin;
            var countDays = span.Days;


            foreach (var reportProject in ReportEmployee)
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

    class PrReportEmployee
    {
        public string EmployeeName { get; set; }

        public double Rate { get; set; }

        // public string WorkObject { get; set; }

        public Dictionary<DateTime, int> Hours { get; set; }
    }

}