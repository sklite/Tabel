using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tabel.Dal;
using Tabel.Models;
using Tabel.ViewModels;

namespace Tabel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return UserLoginManager.IsLogged(Session) 
                ? RedirectToAction("Index", "Work") 
                : RedirectToAction("Login", "Home");
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

            Session["Authorised"] = true;
            Session["UserId"] = employee.Id;
            if (employee.Role.Name == VariablesConfig.AdminRoleName)
                Session["IsAdmin"] = true;
            
            FormsAuthentication.SetAuthCookie(employee.Email, true);

            return RedirectToAction("Index", "Work");
        }

        public ActionResult Login(LoginViewModel loginData)
        {
            return View(loginData);
        }

        public ActionResult Logoff()
        {

            Session.Clear();
            //Session["Authorised"] = false;
            //Session["UserId"] = null;
            
            return RedirectToAction("Index", "Home");
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