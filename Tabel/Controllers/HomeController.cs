using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tabel.Dal;
using Tabel.Models;
using Tabel.Models.Bll;
using Tabel.ViewModels;

namespace Tabel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using (var db = new TabelContext())
            //{
            //    ViewBag.Name = db.Employees.First().Name;
            //}

            return View();
        }

        public ActionResult DoLogin(LoginViewModel loginData)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Login", loginData);
            //var user = Mapper.Map<User>(loginData);

            var userLogin = new UserLoginManager();
            var employee = userLogin.Login(loginData);
            if (employee == null)
                return View("Login");
 

            FormsAuthentication.SetAuthCookie(employee.Email, true);
            Session["Authorised"] = true;
            Session["UserId"] = employee.Id;
            return RedirectToAction("Index", "Work");
        }

        public ActionResult Login(LoginViewModel loginData)
        {
            return View(loginData);
        }


      //  private Employee



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}