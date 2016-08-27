using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tabel.Models;
using Tabel.ViewModels;

namespace Tabel.Dal
{
    public class TimesheetService// : IDisposable
    {
        private TabelContext tabelContext;

        public TimesheetService(TabelContext tabelContext)
        {
            this.tabelContext = tabelContext;
        }

        public IEnumerable<TimesheetViewModel> Read()
        {

           // using (var tabelContext = new TabelContext())
            {
                return tabelContext.Timesheets.Include("Employee").Select(ts => new TimesheetViewModel
                {
                    TimesheetId = ts.Id,
                    Employee = ts.Employee.Id,
                    Project = 1,
                    Date = ts.Date,
                    Hours = ts.Hours
                    //ProductID = product.ProductID,
                    //ProductName = product.ProductName,
                    //UnitPrice = product.UnitPrice.HasValue ? product.UnitPrice.Value : default(decimal),
                    //UnitsInStock = product.UnitsInStock.HasValue ? product.UnitsInStock.Value : default(short),
                    //QuantityPerUnit = product.QuantityPerUnit,
                    //Discontinued = product.Discontinued,
                    //UnitsOnOrder = product.UnitsOnOrder.HasValue ? (int)product.UnitsOnOrder.Value : default(int),
                    //CategoryID = product.CategoryID,
                    //Category = new CategoryViewModel()
                    //{
                    //    CategoryID = product.Category.CategoryID,
                    //    CategoryName = product.Category.CategoryName
                    //},
                    //LastSupply = DateTime.Today
                });
            }
            


        }

        public void Create(TimesheetViewModel timesheetVm)
        {

            var timesheet = new Timesheet
            {
                Date = timesheetVm.Date,
                Employee = new Employee()
                {
                    Id = timesheetVm.Employee
                },
                Hours = timesheetVm.Hours,
                Project = new Project()
                {
                    Id = timesheetVm.Project
                }
            };
         //   using (var db = new TabelContext())
            {
                tabelContext.Timesheets.Add(timesheet);
                tabelContext.SaveChanges();
            }

            //var entity = new Product();

            //entity.ProductName = product.ProductName;
            //entity.UnitPrice = product.UnitPrice;
            //entity.UnitsInStock = (short)product.UnitsInStock;
            //entity.Discontinued = product.Discontinued;
            //entity.CategoryID = product.CategoryID;

            //if (entity.CategoryID == null)
            //{
            //    entity.CategoryID = 1;
            //}

            //if (product.Category != null)
            //{
            //    entity.CategoryID = product.Category.CategoryID;
            //}

            //entities.Products.Add(entity);
            //entities.SaveChanges();

            //product.ProductID = entity.ProductID;
        }

        public void Update(TimesheetViewModel editedTs)
        {


           // using (var tabelContext = new TabelContext())
            {
                var edited = tabelContext.Timesheets.FirstOrDefault(tm => tm.Id == editedTs.TimesheetId);
                //edited.Employee = editedTs.Employee

                tabelContext.SaveChanges();
            }
            //var entity = new Product();

            //entity.ProductID = product.ProductID;
            //entity.ProductName = product.ProductName;
            //entity.UnitPrice = product.UnitPrice;
            //entity.UnitsInStock = (short)product.UnitsInStock;
            //entity.Discontinued = product.Discontinued;
            //entity.CategoryID = product.CategoryID;

            //if (product.Category != null)
            //{
            //    entity.CategoryID = product.Category.CategoryID;
            //}

            //entities.Products.Attach(entity);
            //entities.Entry(entity).State = EntityState.Modified;
            //entities.SaveChanges();
        }

        public void Destroy(TimesheetViewModel toDeleteViewModel)
        {

         //   using (var tabelContext = new TabelContext())
            {
                //var toDelete = tabelContext.Timesheets.FirstOrDefault(tm => tm.Id == toDeleteViewModel.TimesheetId);
                //edited.Employee = editedTs.Employee

                var timesheetToRemove = new Timesheet
                {
                    Id = toDeleteViewModel.TimesheetId
                };


                tabelContext.Timesheets.Remove(timesheetToRemove);
                tabelContext.SaveChanges();
            }


            //var entity = new Product();

            //entity.ProductID = product.ProductID;

            //entities.Products.Attach(entity);

            //entities.Products.Remove(entity);

            //var orderDetails = entities.Order_Details.Where(pd => pd.ProductID == entity.ProductID);

            //foreach (var orderDetail in orderDetails)
            //{
            //    entities.Order_Details.Remove(orderDetail);
            //}

            //entities.SaveChanges();
        }

        public void Dispose()
        {
            tabelContext.Dispose();
        }
    }
}