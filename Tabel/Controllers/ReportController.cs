using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tabel.Dal;
using Tabel.Dal.DataServices;

namespace Tabel.Controllers
{
    public class ReportController : Controller
    {

        ReportService reportService = new ReportService(new TabelContext());
        // GET: Report
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult View(string dateBegin, string dateEnd)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Input");

            reportService.DateBegin = Convert.ToDateTime(dateBegin);
            reportService.DateEnd = Convert.ToDateTime(dateEnd);

            var model = reportService.Read();

            return View(model);
        }

        public ActionResult Input()
        {
            return View();
        }
    }
}