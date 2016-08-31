using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tabel.ViewModels.Report;

namespace Tabel.Dal.DataServices
{
    public class ReportService
    {

        private TabelContext entities;

        public ReportService(TabelContext entities)
        {
            this.entities = entities;
            DateBegin = DateTime.Today.AddMonths(-1);
            DateEnd = DateTime.Today;
        }

        public IEnumerable<ReportViewModel> Read()
        {
            var result = entities.Timesheets.Include("Employee").Include("Project").Select(ts => new ReportViewModel
            {
                EmployeeName = ts.Employee.Name,
                Date = ts.Date,//ts.Date.ToString("yyyy-MM-dd"),
                Hours = ts.Hours,
                ProjectCode = ts.Project.Name,
                Rate = ts.Employee.Rate
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
            return result;
        }

        //public void Create(ReportViewModel product)
        //{
        //    var entity = new Product();

        //    entity.ProductName = product.ProductName;
        //    entity.UnitPrice = product.UnitPrice;
        //    entity.UnitsInStock = (short)product.UnitsInStock;
        //    entity.Discontinued = product.Discontinued;
        //    entity.CategoryID = product.CategoryID;

        //    if (entity.CategoryID == null)
        //    {
        //        entity.CategoryID = 1;
        //    }

        //    if (product.Category != null)
        //    {
        //        entity.CategoryID = product.Category.CategoryID;
        //    }

        //    entities.Products.Add(entity);
        //    entities.SaveChanges();

        //    product.ProductID = entity.ProductID;
        //}

        //public void Update(ProductViewModel product)
        //{
        //    var entity = new Product();

        //    entity.ProductID = product.ProductID;
        //    entity.ProductName = product.ProductName;
        //    entity.UnitPrice = product.UnitPrice;
        //    entity.UnitsInStock = (short)product.UnitsInStock;
        //    entity.Discontinued = product.Discontinued;
        //    entity.CategoryID = product.CategoryID;

        //    if (product.Category != null)
        //    {
        //        entity.CategoryID = product.Category.CategoryID;
        //    }

        //    entities.Products.Attach(entity);
        //    entities.Entry(entity).State = EntityState.Modified;
        //    entities.SaveChanges();
        //}

        //public void Destroy(ProductViewModel product)
        //{
        //    var entity = new Product();

        //    entity.ProductID = product.ProductID;

        //    entities.Products.Attach(entity);

        //    entities.Products.Remove(entity);

        //    var orderDetails = entities.Order_Details.Where(pd => pd.ProductID == entity.ProductID);

        //    foreach (var orderDetail in orderDetails)
        //    {
        //        entities.Order_Details.Remove(orderDetail);
        //    }

        //    entities.SaveChanges();
        //}

        public void Dispose()
        {
            entities.Dispose();
        }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }
    }
}