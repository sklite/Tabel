﻿using System;
using System.Linq;
using System.Web;
using Tabel.Models;
using Tabel.ViewModels;

namespace Tabel.Dal
{
    public class UserLoginManager
    {
        public Employee Login(LoginViewModel loginVm)
        {
            Employee result;
            using (var db = new TabelContext())
            {
                result = db.Employees.Include("Role").FirstOrDefault(em => em.Email == loginVm.Login
                                                           && em.Pass == loginVm.Password);
            }
            return result;
        }

        
        public static bool IsLogged(HttpSessionStateBase session)
        {
            if (!Convert.ToBoolean(session["Authorised"]))
                return false;

            int id;
            if (!int.TryParse(session["UserId"].ToString(), out id))
                return false;

            using (var db = new TabelContext())
            {
                return db.Employees.FirstOrDefault(em => em.Id == id) != null;
            }
        }

        public static Employee GetCurrentUser(HttpSessionStateBase session)
        {
            if (!Convert.ToBoolean(session["Authorised"]))
                return null;

            int id;
            if (!int.TryParse(session["UserId"].ToString(), out id))
                return null;

            using (var db = new TabelContext())
            {
                return db.Employees.Include("Role").FirstOrDefault(em => em.Id == id);
            }
        }

        public static bool IsUserManager(int userId, TabelContext context)
        {
            var employee = context.Employees.Include("Role").FirstOrDefault(em => em.Id == userId);
            if (employee == null)
                return false;
            return employee.Role.Name == VariablesConfig.AdminRoleName;
        }


    }
}