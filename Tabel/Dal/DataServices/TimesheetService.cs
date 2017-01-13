using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Tabel.Models;
using Tabel.ViewModels;
using Tabel.ViewModels.Timesheet;

namespace Tabel.Dal
{
    public class TimesheetService : BaseDataEditService<TimesheetViewModel>
    {
        public TimesheetService(TabelContext tabelContext)
            :base(tabelContext)
        {
        }

        IEnumerable<TimesheetViewModel> ConvertToVm(IQueryable<Timesheet> timesheets)
        {
            return timesheets.Select(ts => new TimesheetViewModel
            {
                TimesheetId = ts.Id,
                EmployeeId = ts.Employee.Id,
                Employee = new TsEmployeeViewModel
                {
                    EmployeeId = ts.Employee.Id,
                    EmployeeName = ts.Employee.Name
                },
                ProjectId = ts.Project.Id,//ts.Project.Code + "##" + ts.Project.Id,
                Project = new TsProjectViewModel
                {
                    ProjectId = ts.Project.Id,//ts.Project.Code + "##" + ts.Project.Id.ToString(),
                    ProjectCode = ts.Project.Code
                },
                Date = ts.Date,
                Hours = ts.Hours
            });//.OrderBy(ts=>ts.Employee.EmployeeName);
        }



        public IEnumerable<TsEmployeeViewModel> GetEmployeeNames()
        {
            return _tabelContext.Employees.Select(em => new TsEmployeeViewModel()
            {
                EmployeeId = em.Id,
                EmployeeName = em.Name
            });
        }

        public IEnumerable<TsProjectViewModel> GetProjectCodes()
        {
            return _tabelContext.Projects.Select(proj => new TsProjectViewModel()
            {
                ProjectId = proj.Id,//proj.Code + "##" + proj.Id.ToString(),
                ProjectCode = proj.Code,

            }).OrderBy(projVm => projVm.ProjectCode);
        }

        public override IEnumerable<TimesheetViewModel> Read()
        {

            if (UserId == 0)
            {
                return ConvertToVm(_tabelContext.Timesheets.Include("Employee").Include("Project"));
            }
            var isAdmin = UserLoginManager.IsUserManager(UserId, _tabelContext);
            return ConvertToVm(isAdmin 
                ? _tabelContext.Timesheets.Include("Employee").OrderBy(em => em.Employee.Name).Include("Project")
                : _tabelContext.Timesheets.Include("Employee").Include("Project").Where(ts => ts.Employee.Id == UserId));
        }

        public override void Create(TimesheetViewModel timesheetVm)
        {

            if (timesheetVm.EmployeeId == 0 || timesheetVm.ProjectId == 0)
                return;

         //   var shortendDate = new DateTime(timesheetVm.Date.Year, timesheetVm.Date.Month, timesheetVm.Date.Day);


            //var tempTs = _tabelContext.Timesheets
            //    .Include("Employee")
            //    .Include("Project").ToList();

            //проверка на задвоения
            if (_tabelContext.Timesheets
                .Include("Employee")
                .Include("Project")
                .Any(ts => ts.Project.Id == timesheetVm.ProjectId && ts.Employee.Id == timesheetVm.EmployeeId
                && ts.Date.Year == timesheetVm.Date.Year
                && ts.Date.Month == timesheetVm.Date.Month
                && ts.Date.Day == timesheetVm.Date.Day
                && ts.Hours == timesheetVm.Hours))
                return;

            var employee = _tabelContext.Employees.FirstOrDefault(em => em.Id == timesheetVm.EmployeeId);
            var project = _tabelContext.Projects.FirstOrDefault(pr => pr.Id == timesheetVm.ProjectId);//GetIdFromProjectCodeAndId(timesheetVm.ProjectId));




            var timesheet = new Timesheet
            {
                Date = timesheetVm.Date,
                Employee = employee,
                Hours = timesheetVm.Hours,
                Project = project
            };
            _tabelContext.Timesheets.Add(timesheet);
            _tabelContext.SaveChanges();
        }

        //int GetIdFromProjectCodeAndId(string codeAndId)
        //{
        //    var id = codeAndId.Split(new [] {"##"}, StringSplitOptions.RemoveEmptyEntries)[1];
        //    return Convert.ToInt32(id);
        //}

        //string JoinCodeAndId(string code, int id)
        //{
        //    return code + "##" + id;
        //}

        public override void Update(TimesheetViewModel editedTs)
        {
             var edited = _tabelContext.Timesheets.FirstOrDefault(tm => tm.Id == editedTs.TimesheetId);
            edited.Employee = _tabelContext.Employees.FirstOrDefault(em => em.Id == editedTs.EmployeeId);
            edited.Project = _tabelContext.Projects.FirstOrDefault(pr => pr.Id == editedTs.ProjectId);//GetIdFromProjectCodeAndId(editedTs.ProjectId));
            edited.Date = editedTs.Date;
            edited.Hours = editedTs.Hours;
            _tabelContext.SaveChanges();
        }

        public override void Destroy(TimesheetViewModel toDeleteViewModel)
        {
            var timesheetToRemove = _tabelContext.Timesheets.FirstOrDefault(ts => ts.Id == toDeleteViewModel.TimesheetId);
            if (timesheetToRemove == null)
                return;
            _tabelContext.Timesheets.Remove(timesheetToRemove);
            _tabelContext.SaveChanges();
            
        }


        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
    }

    //public static class DataSourceRequestExtensions
    //{
    //    public static void ReplaceSortColumn(this DataSourceRequest instance, string from, string to)
    //    {
    //        instance.Sorts.Where(w => w.Member == from).ToList().ForEach(s => s.Member = to);
    //    }
    //}
}