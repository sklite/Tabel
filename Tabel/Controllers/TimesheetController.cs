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

namespace Tabel.Controllers
{
    public class TimesheetController : Controller
    {


        TimesheetService timesheetService = new TimesheetService(new TabelContext());

        public TimesheetController()
        {
            //timesheetService = new TimesheetService(/*new TabelContext()*/);
        }

        // GET: Timesheet
        public ActionResult Editing()
        {
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(timesheetService.Read().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<TimesheetViewModel> products)
        {
            var results = new List<TimesheetViewModel>();

            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    timesheetService.Create(product);
                    results.Add(product);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<TimesheetViewModel> products)
        {
            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    timesheetService.Update(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<TimesheetViewModel> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    timesheetService.Destroy(product);
                }
            }

            return Json(products.ToDataSourceResult(request, ModelState));
        }
    }

}