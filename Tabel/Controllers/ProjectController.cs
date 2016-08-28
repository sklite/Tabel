using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Tabel.Dal;
using Tabel.Dal.DataServices;
using Tabel.ViewModels.Project;

namespace Tabel.Controllers
{
    public class ProjectController : Controller
    {
        ProjectService _projectService = new ProjectService(new TabelContext());


        public ActionResult Edit()
        {
            if (!UserLoginManager.IsLogged(Session))
                return RedirectToAction("Index", "Home");


            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(_projectService.Read().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProjectViewModel> projects)
        {
            var results = new List<ProjectViewModel>();

            if (projects != null && ModelState.IsValid)
            {
                foreach (var project in projects)
                {
                    _projectService.Create(project);
                    results.Add(project);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProjectViewModel> projects)
        {
            if (projects != null && ModelState.IsValid)
            {
                foreach (var project in projects)
                {
                    _projectService.Update(project);
                }
            }

            return Json(projects.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProjectViewModel> projects)
        {
            if (projects.Any())
            {
                foreach (var project in projects)
                {
                    _projectService.Destroy(project);
                }
            }

            return Json(projects.ToDataSourceResult(request, ModelState));
        }

    }
}