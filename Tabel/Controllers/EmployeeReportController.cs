using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Tabel.Dal;
using Tabel.Dal.DataServices;

namespace Tabel.Controllers
{
    public class EmployeeReportController : Controller
    {

        EmployeeReportService _erService = new EmployeeReportService(new TabelContext());


        // GET: EmployeeReport
        public ActionResult View()
        {
            var model = _erService.GetData();

            return View(model);
        }


        //public ActionResult Excel_Export_Read([DataSourceRequest]DataSourceRequest request)
        //{
        //    return Json(_erService.Read().ToDataSourceResult(request));
        //}

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

    }
}