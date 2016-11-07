using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Tabel.Bll;
using Tabel.Dal;
using Tabel.Dal.DataServices;
using Tabel.ViewModels;

namespace Tabel.Controllers
{
    public class EmployeeReportController : Controller
    {

        EmployeeReportService _erService = new EmployeeReportService(new TabelContext());

        public ActionResult View(string dateBegin, string dateEnd, string command)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("View");

            if (!string.IsNullOrEmpty(dateBegin) && !string.IsNullOrEmpty(dateEnd))
            {

                _erService.DateBegin = Convert.ToDateTime(dateBegin);
                _erService.DateEnd = Convert.ToDateTime(dateEnd).AddDays(1);
            }
            
            EmployeeReportViewModel model;

            if (command == "Загрузить Excel")
            {
                var excelCreator = new ExcelCreator();
                model = _erService.GetData();
                var fullPath = excelCreator.CreateFile(model);
                return File(new FileStream(fullPath, FileMode.Open), "application/vnd.ms-excel", "Отчёт по сотрудникам.xlsx");
            }
            
            model = _erService.GetData();
            return View(model);
  

        }



        public ActionResult Input()
        {
            return View();
        }
        
    }
}