using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Tabel.Dal;
using Tabel.ViewModels.Employee;

namespace Tabel.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeService _employeeService = new EmployeeService(new TabelContext());


        public ActionResult Edit()
        {
            if (!UserLoginManager.IsLogged(Session))
                return RedirectToAction("Index", "Home");

            ViewData["roles"] = _employeeService.GetRoles();
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(_employeeService.Read().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<EmployeeViewModel> projects)
        {
            var results = new List<EmployeeViewModel>();

            if (projects != null && ModelState.IsValid)
            {
                foreach (var project in projects)
                {
                    _employeeService.Create(project);
                    results.Add(project);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<EmployeeViewModel> projects)
        {
            if (projects != null && ModelState.IsValid)
            {
                foreach (var project in projects)
                {
                    _employeeService.Update(project);
                }
            }

            return Json(projects.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<EmployeeViewModel> projects)
        {
            if (projects.Any())
            {
                foreach (var project in projects)
                {
                    _employeeService.Destroy(project);
                }
            }

            return Json(projects.ToDataSourceResult(request, ModelState));
        }

    }
}