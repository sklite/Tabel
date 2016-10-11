using System;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using Tabel.ViewModels;
using Tabel.ViewModels.ProjectReport;

namespace Tabel.Bll
{
    public class ProjectReportExcelCreator
    {
        public string CreateFile(ProjectReportViewModel reportVm)
        {


            string path = AppDomain.CurrentDomain.BaseDirectory + "Content/";
            string fileName = Path.GetRandomFileName();//"Отчёт по сотрудникам.xlsx";

            string fullPath = path + fileName;

            SLDocument sl = new SLDocument();

            // set a boolean at "A1"
            //sl.SetCellValue("A1", true);


            SetColumns(sl, reportVm);
            SetData(sl, reportVm);

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

        void SetColumns(SLDocument document, ProjectReportViewModel reportVm)
        {

            SLStyle style = document.CreateStyle();
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);


            style.Alignment.Horizontal = HorizontalAlignmentValues.Left;

            style.Alignment.ReadingOrder = SLAlignmentReadingOrderValues.LeftToRight;
            style.Alignment.ShrinkToFit = true;
            style.Alignment.TextRotation = 90;


            document.SetCellValue(1, 1, "Проект");
            document.SetCellValue(1, 2, "Объект");
            document.SetCellValue(1, 3, "Код проекта");
            document.SetCellValue(1, 4, "Имя");


            document.SetColumnWidth(1, 17);
            document.SetColumnWidth(2, 20);
            document.SetColumnWidth(3, 17);
            document.SetColumnWidth(4, 17);

            for (int i = 0; i < reportVm.MyColumns.Count; i++)
            {
                document.SetCellValue(1, i + 5, reportVm.MyColumns[i]);

                document.SetColumnWidth(i + 5, 4);
                // style.SetCellStyle();
                document.SetCellStyle(1, i + 5, style);
            }

            document.SetCellValue(1, reportVm.MyColumns.Count + 5, "Всего часов");
            document.SetColumnWidth(reportVm.MyColumns.Count + 5, 17);
            document.SetCellValue(1, reportVm.MyColumns.Count + 6, "Деньги");
        }

        void SetData(SLDocument document, ProjectReportViewModel reportVm)
        {
            for (int i = 0; i < reportVm.Rows.Count; i++)
            {
                var currentRow = reportVm.Rows[i];

                int rownum = i + 2;

                document.SetCellValue(rownum, 1, currentRow.ProjectName);
                document.SetCellValue(rownum, 2, currentRow.WorkObject);
                document.SetCellValue(rownum, 3, currentRow.ProjectCode);
                document.SetCellValue(rownum, 4, currentRow.Name);



                for (int j = 0; j < currentRow.Hours.Count; j++)
                {
                    document.SetCellValue(rownum, j + 5, currentRow.Hours[j]);
                }


                document.SetCellValue(rownum, reportVm.MyColumns.Count + 5, currentRow.TotalHours);
                document.SetCellValue(rownum, reportVm.MyColumns.Count + 6, currentRow.Money);

            }
        }
    }
}