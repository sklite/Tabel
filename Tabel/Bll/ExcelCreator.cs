﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using Tabel.ViewModels;

namespace Tabel.Bll
{
    public class ExcelCreator
    {


        public string CreateFile(EmployeeReportViewModel reportVm)
        {
         

            string path = AppDomain.CurrentDomain.BaseDirectory + "Content/";
            string fileName = Path.GetRandomFileName();//"Отчёт по сотрудникам.xlsx";

            string fullPath = path + fileName;

            SLDocument sl = new SLDocument();

            // set a boolean at "A1"
            //sl.SetCellValue("A1", true);


            SetColumns(sl, reportVm);
            SetData(sl,reportVm);

            try
            {
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }
            catch (Exception)
            {
                var random = new Random();
                fullPath = path + random.Next() + "_" + fileName;

            }


            sl.SaveAs(fullPath);

            return fullPath;
        }

        void SetColumns(SLDocument document, EmployeeReportViewModel reportVm)
        {

            SLStyle style = document.CreateStyle();
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);


            style.Alignment.Horizontal = HorizontalAlignmentValues.Left;

            style.Alignment.ReadingOrder = SLAlignmentReadingOrderValues.LeftToRight;
            style.Alignment.ShrinkToFit = true;
            style.Alignment.TextRotation = 90;



            document.SetCellValue(1, 1, "Имя");
            document.SetCellValue(1, 2, "Проект");
            document.SetCellValue(1, 3, "Объект");

            document.SetColumnWidth(1, 17);
            document.SetColumnWidth(2, 20);
            document.SetColumnWidth(3, 17);

            for (int i = 0; i < reportVm.MyColumns.Count; i++)
            {
                document.SetCellValue(1, i + 4, reportVm.MyColumns[i]);

                document.SetColumnWidth(i + 4, 4);
                // style.SetCellStyle();
                document.SetCellStyle(1, i + 4, style);
            }

            document.SetCellValue(1, reportVm.MyColumns.Count + 4, "Всего часов");
            document.SetColumnWidth(reportVm.MyColumns.Count + 4, 17);
            document.SetCellValue(1, reportVm.MyColumns.Count + 5, "Деньги");
        }

        void SetData(SLDocument document, EmployeeReportViewModel reportVm)
        {
            for (int i = 0; i < reportVm.Rows.Count; i++)
            {
                var currentRow = reportVm.Rows[i];

                int rownum = i + 2;

                document.SetCellValue(rownum, 1, currentRow.Name);
                document.SetCellValue(rownum, 2, currentRow.Project);
                document.SetCellValue(rownum, 3, currentRow.WorkObject);


                for (int j = 0; j < currentRow.Hours.Count; j++)
                {
                    document.SetCellValue(rownum, j + 4, currentRow.Hours[j]);
                }


                document.SetCellValue(rownum, reportVm.MyColumns.Count + 4, currentRow.TotalHours);
                document.SetCellValue(rownum, reportVm.MyColumns.Count + 5, currentRow.Money);

            }
        }

    }
}