using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CT.Utils
{

    /// <summary>
    /// Tiện ích Excel
    /// </summary>
    /// pkha 23.06.2023
    public static class ExcelHelper
    {

        #region Thao tác với excel

        public static string webRootPath = ""; 
        //datable

        public static DataTable ToDataTable(ISheet sheet)
        {

            var dataTable = new DataTable(sheet.SheetName);

            // write the header row
            var headerRow = sheet.GetRow(0);
            foreach (var headerCell in headerRow)
            {
                dataTable.Columns.Add(headerCell.ToString());
            }
            var maxRows = 10;

            // write the rest
            for (int i = 1; i < maxRows; i++)
            {
                var sheetRow = sheet.GetRow(i);
                var dtRow = dataTable.NewRow();
                dtRow.ItemArray = dataTable.Columns
                    .Cast<DataColumn>()
                    .Select(c => sheetRow.GetCell(c.Ordinal, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString())
                    .ToArray();
                dataTable.Rows.Add(dtRow);
            }

            return dataTable;

        }


        #endregion


        #region Lưu trữ, đọc ghi, download



        public static bool SaveExcel(IWorkbook wb, string path)
        {
            string fileName = @"Democ.xlsx";

            var isSuccess = false;
             
            //todo trên mạng copy vào


            return isSuccess;

             
        }


        public static IWorkbook GetExcelFromPath( string filePath)
        { 
            IWorkbook workbook = null;

            // Check the file extension and choose the correct workbook type
            string fileExtension = Path.GetExtension(filePath).ToLower();

            if (fileExtension == ".xls")
            {
                // Load .xls (HSSF) workbook
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new HSSFWorkbook(fs);
                }
            }
            else if (fileExtension == ".xlsx")
            {
                // Load .xlsx (XSSF) workbook
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(fs);
                }
            }
            else
            {
                throw new Exception("Invalid file extension. Only .xls and .xlsx are supported.");
            }

            return workbook;
        }


        #endregion


        public static void ExcelToPdf()
        {
            var folder = "C:\\DuLieu\\CongViec\\KH.VSEL\\";
            var fileName = "abc.xlsx";

            FileStream fs = new FileStream(folder, FileMode.OpenOrCreate);



            XSSFWorkbook xwb = new XSSFWorkbook(fs);

            //Lấy sheet đầu tiên
            ISheet sh = xwb.GetSheetAt(0);
            ICell cel = sh.CreateRow(1).CreateCell(1);

            cel.SetCellValue("kfdkfsfk");


            //Đưa workboox Npoi sang stream
            var st = new MemoryStream();
            xwb.Write(st);

            //Cầu nối giữa Npoi và Spire là stream

            //Đọc Spire workbook từ stream
            var swb = new Workbook();
            swb.LoadFromStream(st);

            //đưa về pdf
            using (var pdfStream = new MemoryStream())
            {
                swb.SaveToStream(pdfStream, Spire.Xls.FileFormat.PDF);

                pdfStream.Position = 0;

                File.WriteAllBytes(folder + "abc.pdf", pdfStream.ToArray());

            } 

        }


    }
}
