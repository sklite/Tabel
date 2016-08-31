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
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<EmployeeViewModel> employees)
        {
            var results = new List<EmployeeViewModel>();

            if (employees != null && ModelState.IsValid)
            {
                foreach (var employee in employees)
                {
                    _employeeService.Create(employee);
                    results.Add(employee);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<EmployeeViewModel> employees)
        {
            if (employees != null && ModelState.IsValid)
            {
                foreach (var employee in employees)
                {
                    _employeeService.Update(employee);
                }
            }

            return Json(employees.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<EmployeeViewModel> employees)
        {
            if (employees.Any())
            {
                foreach (var employee in employees)
                {
                    _employeeService.Destroy(employee);
                }
            }

            return Json(employees.ToDataSourceResult(request, ModelState));
        }

    }
}