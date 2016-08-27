using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tabel.Dal;
using Tabel.Models;
using Tabel.Models.Bll;
using Tabel.ViewModels;

namespace Tabel.Controllers
{
    public class WorkController : Controller
    {
        // GET: Work
        public ActionResult Index()
        {
            
            if (!UserLoginManager.IsLogged(Session))
                return RedirectToAction("Index", "Home");
            
            var currentUser = UserLoginManager.GetCurrentUser(Session);

            var userVm = new WorkIndexViewModel
            {
                Mail = currentUser.Email,
                Name = currentUser.Name,
                RoleName = currentUser.Role.Name
            };


            return View(userVm);
        }

        public ActionResult Tabel()
        {
            if (!UserLoginManager.IsLogged(Session))
                return RedirectToAction("Index", "Home");


            return View();
        }

        public ActionResult Employees()
        {
            //if (!UserLoginManager.IsLogged(Session))
            //    return RedirectToAction("Index", "Home");

            
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