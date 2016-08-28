using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tabel.ViewModels.Employee
{
    public class EmployeeViewModel : IDataEditViewModel
    {

        public int EmployeeId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public double Rate { get; set; }

        public int RoleId { get; set; }
        public EmRoleViewModel Role { get; set; }
    }
}