using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tabel.Models;
using Tabel.ViewModels.Employee;

namespace Tabel.Dal
{
    public class EmployeeService : BaseDataEditService<EmployeeViewModel>
    {
        public EmployeeService(TabelContext tabelContext) : base(tabelContext)
        {
        }

        public IEnumerable<EmRoleViewModel> GetRoles()
        {
            return _tabelContext.Roles.Select(em => new EmRoleViewModel()
            {
                RoleId = em.Id,
                RoleName = em.Name
            });
        }

        public override IEnumerable<EmployeeViewModel> Read()
        {
            var result = _tabelContext.Employees.Include("Role").Select(em => new EmployeeViewModel()
                {
                    EmployeeId = em.Id,
                    Name = em.Name,
                    Email = em.Email,
                    Rate = em.Rate,
                    RoleId = em.Role.Id,
                    Role = new EmRoleViewModel
                    {
                        RoleId = em.Role.Id,
                        RoleName = em.Role.Name
                    }
            });//.ToList();
            return result;
        }

        public override void Create(EmployeeViewModel dataToCreate)
        {

            if (_tabelContext.Employees.Any(emp => emp.Name == dataToCreate.Name))
                return;

            var role = _tabelContext.Roles.FirstOrDefault(em => em.Id == dataToCreate.RoleId);

            var employee = new Employee
            {
                Name = dataToCreate.Name,
                Email = dataToCreate.Email,
                Pass = "123",
                Rate = dataToCreate.Rate,
                Role = role
            };
            _tabelContext.Employees.Add(employee);
            _tabelContext.SaveChanges();
        }

        public override void Update(EmployeeViewModel dataToUpdate)
        {
            var edited = _tabelContext.Employees.FirstOrDefault(em => em.Id == dataToUpdate.EmployeeId);
            if (edited == null)
                return;
            edited.Name = dataToUpdate.Name;
            edited.Email = dataToUpdate.Email;
            //edited.Pass =;dataToUpdate.
            edited.Rate = dataToUpdate.Rate;
            edited.Role = _tabelContext.Roles.FirstOrDefault(role => role.Id == dataToUpdate.RoleId);
            _tabelContext.SaveChanges();
        }

        public override void Destroy(EmployeeViewModel dataToDelete)
        {
            var employeeToRemove = _tabelContext.Employees.FirstOrDefault(em => em.Id == dataToDelete.EmployeeId);
            if (employeeToRemove == null)
                return;

            var linkedTimesheets =
                _tabelContext.Timesheets.Include("Employee").Where(ts => ts.Employee.Id == employeeToRemove.Id);

            _tabelContext.Timesheets.RemoveRange(linkedTimesheets);
            _tabelContext.Employees.Remove(employeeToRemove);


            _tabelContext.SaveChanges();
        }
    }
}