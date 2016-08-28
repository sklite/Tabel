using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Tabel.Dal;
using Tabel.ViewModels;
using Tabel.ViewModels.Timesheet;

namespace Tabel.Controllers
{
    public class TimesheetController : Controller
    {

        TimesheetService _timesheetService = new TimesheetService(new TabelContext());


        // GET: Timesheet
        public ActionResult Edit()
        {
            if (!UserLoginManager.IsLogged(Session))
                return RedirectToAction("Index", "Home");

                ViewData["employees"] = _timesheetService.GetEmployeeNames();
            ViewData["projects"] = _timesheetService.GetProjectNames();
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            


            _timesheetService.UserId = Convert.ToInt32(Session["UserId"]);
            _timesheetService.IsAdmin = Convert.ToBoolean(Session["IsAdmin"]);

            return Json(_timesheetService.Read().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<TimesheetViewModel> timesheets)
        {
            var results = new List<TimesheetViewModel>();

            if (timesheets != null && ModelState.IsValid)
            {
                foreach (var timesheet in timesheets)
                {
                    _timesheetService.Create(timesheet);
                    results.Add(timesheet);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<TimesheetViewModel> timesheets)
        {
            if (timesheets != null && ModelState.IsValid)
            {
                foreach (var timesheet in timesheets)
                {
                    _timesheetService.Update(timesheet);
                }
            }

            return Json(timesheets.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<TimesheetViewModel> timesheets)
        {
            if (timesheets.Any())
            {
                foreach (var timesheet in timesheets)
                {
                    _timesheetService.Destroy(timesheet);
                }
            }

            return Json(timesheets.ToDataSourceResult(request, ModelState));
        }
    }

}