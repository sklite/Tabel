using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tabel.Dal;
using Tabel.Models;
using Tabel.ViewModels;

namespace Tabel.Controllers
{
    public class WorkController : Controller
    {
        // GET: Work
        public ActionResult Index()
        {
            if (!Convert.ToBoolean(Session["Authorised"]))
                return RedirectToAction("Index", "Home");

            int id;
            if (!int.TryParse(Session["UserId"].ToString(), out id))
                return RedirectToAction("Index", "Home");
            //= Convert.ToInt32(Session["UserId"]);


            Employee employee;



            using (var db = new TabelContext())
            {
                employee = db.Employees.FirstOrDefault(em => em.Id == id);
            }


            if (employee == null)
                return RedirectToAction("Index", "Home");



            return View();
        }

        public ActionResult Tabel()
        {
            return View();
        }

        public ActionResult Employees()
        {
            var employeeListVm = new EmployeeListViewModel();

            using (var db = new TabelContext())
            {
                foreach (var employee in db.Employees)
                {
                    employeeListVm.Employees.Add(new EmployeeViewModel
                    {
                        Name = employee.Name
                    });
                }
            }

            return View(employeeListVm);
        }


        public ActionResult Projects()
        {
            return View();
        }
    }
}