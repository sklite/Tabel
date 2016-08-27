using System.Linq;
using Tabel.Dal;
using Tabel.ViewModels;

namespace Tabel.Models.Bll
{
    public class UserLoginManager
    {
        public Employee Login(LoginViewModel loginVm)
        {
            Employee result;
            using (var db = new TabelContext())
            {
                result = db.Employees.FirstOrDefault(em => em.Email == loginVm.Login
                                                           && em.Pass == loginVm.Password);
            }
            return result;
        }
    }
}