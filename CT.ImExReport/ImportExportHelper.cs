
using CT.Common;
using CT.ImExReport;
using CT.Models.Entity;
using CT.Utils;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CT.BL;
using CT.DL;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using NPOI.XSSF.UserModel;
//using SkiaSharp;
//using Spire.Xls.Core;

namespace CT.ImExReport
{

    /// <summary>
    /// Import/export
    /// </summary>
    /// phamkhanhhand  29.06.2023
    public class ImportExportHelper
    {

        //public void Import<T>(Stream st) where T : BaseEntity
        //{
        //    IWorkbook wb = WorkbookFactory.Create(st);
        //    var ws = wb.GetSheetAt(0);

        //    var data = this.Import<T>(ws);
        //}


        /// <summary>
        /// Export from screen
        /// Export dữ liệu ở screen
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static FileContentResult ExportScreen<T>() where T : BaseEntity
        {
            var entityType = typeof(T);
            var tempName = entityType.Name;

            //file trắng
            var tempPath = "TemplateFile/TempScreen/tempexportscreen.xlsx";
            IWorkbook wb = ExcelHelper.GetExcelFromPath(tempPath);
            var ws = wb.GetSheetAt(0);

            //xử lý dữ liệu

            // Thêm một số dữ liệu vào sheet  

            var baseBL = BLFactory.CreateBL<T>();

            var lstData =  baseBL.GetPagingByTypeDataOnly<T>();



            BuidDataToSheet<T>(ref ws, tempName, lstData);


            var dataByte = ConvertWorkbookToByteArray(wb);
            //donwload
            return DownloadHelper.GenerateExcelFile(DownloadHelper.CONTENT_TYPE_XLSX, dataByte, tempName);
        }

        public static FileContentResult DonwloadTempFile<T>() where T : BaseEntity
        {
            var entityType = typeof(T);
            var tempName =  "Temp"+ entityType.Name;

            var tempPath = "TemplateFile/"+ tempName + ".xlsx";
            IWorkbook wb = ExcelHelper.GetExcelFromPath(tempPath);

            //Todo bind thêm danh mục
            
            var dataByte = ConvertWorkbookToByteArray(wb);
            //donwload
            return DownloadHelper.GenerateExcelFile(DownloadHelper.CONTENT_TYPE_XLSX, dataByte, tempName);
        }


        // Hàm xử lý ISheet

        static void BuidDataToSheet<T>(ref ISheet ws, string tempName, List<T> lstData) where T : BaseEntity
        {

           
            #region Load config từ db 

            var importConfigBL = new ImportConfigBL();
            var importMappingBL = new ImportMappingBL();

            var importConfig = importConfigBL.GetDataByColumn<ImportConfig>("ImportConfigCode", tempName);

            List<ImportMapping> lstMapping = new List<ImportMapping>();

            if (importConfig != null)
            {
                lstMapping = importMappingBL.GetListDataByColumn<ImportMapping>("ImportConfigID", importConfig.ImportConfigID);

                #region Bind dữ liệu
                Export<T>(ref ws, importConfig, lstMapping, lstData);

                #endregion
            }

            #endregion



        }

         
        public static List<T> ImportFromScreen<T>(IFormFile file) where T : BaseEntity
        {
            var entityType = typeof(T);
            var tempName = entityType.Name; 
            var baseBL = BLFactory.CreateBL<T>();

            var importConfigBL = new ImportConfigBL();
            var importMappingBL = new ImportMappingBL();

            var wb = ReadExcelFileFromStream(file).GetAwaiter().GetResult();


            var importConfig = importConfigBL.GetDataByColumn<ImportConfig>("ImportConfigCode", tempName);

            List<ImportMapping> lstMapping = new List<ImportMapping>();

            var ws = wb.GetSheetAt(0);
            List<T> lstData = null;



            if (importConfig != null)
            {
                lstMapping = importMappingBL.GetListDataByColumn<ImportMapping>("ImportConfigID", importConfig.ImportConfigID);

                #region Bind dữ liệu

                ////mapping
                //lstMapping = SetIndexMapping(ws, lstMapping, importConfig. StartTitleRowIndex, importConfig. TitleRowCount, importConfig. MaxColumnIndexInExcel, importConfig. MaxRowIndexInExcel);

                lstData =  Import<T>(ws, importConfig, lstMapping);

                #endregion
            }

            return lstData;


        }

        /// <summary>
        /// Read workbook from Iform file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// phamkhanhhand Nov 23, 2024
        private static async Task<IWorkbook> ReadExcelFileFromStream(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Copy nội dung file vào MemoryStream
                await file.CopyToAsync(memoryStream);

                // Đọc dữ liệu từ MemoryStream
                memoryStream.Position = 0; // Reset vị trí của stream về đầu
                return new XSSFWorkbook(memoryStream); // Dùng XSSFWorkbook cho file .xlsx
            }
        }

        public static byte[] ConvertWorkbookToByteArray(IWorkbook workbook)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Ghi workbook vào memory stream
                workbook.Write(memoryStream);

                // Trả về byte array của memory stream
                return memoryStream.ToArray();
            }
        }


        private static int GetMaxRowData(ISheet ws, int maxPhysical, string columnFind="A")
        {
            var maxRow= 0;
             

            // Duyệt qua từng dòng trong sheet
            //Excelstart address = 1
            for (int rowIndex = 1; rowIndex <= maxPhysical; rowIndex++)
            { 
                    ICell cell = ws.GetCell(columnFind+ rowIndex); // Lấy ô ở cột cần kiểm tra

                    if (cell != null && !string.IsNullOrWhiteSpace(cell.ToString())) // Kiểm tra ô có dữ liệu
                    {
                    maxRow = rowIndex; // Cập nhật dòng có dữ liệu
                    } 
            }


            return maxRow;
        }


        /// <summary>
        /// Todo to row, then validate data field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ws"></param>
        /// <param name="importConfig"></param>
        /// <param name="lstMapping"></param>
        /// <returns></returns>
        public static List<T> Import<T>(ISheet ws, ImportConfig importConfig, List<ImportMapping> lstMapping) where T : BaseEntity
        {
             


            List<T> lstEntity = new List<T>();


            int startTitleRowIndex = importConfig.StartTitleRowIndex;
            int titleRowCount = importConfig.TitleRowCount;
            int maxColumnIndexInExcel = importConfig.MaxColumnIndexInExcel;
            int maxRowIndexInExcel = importConfig.MaxRowIndexInExcel;

        
            if (titleRowCount == 0)
            { 
                titleRowCount = 1;
            }

            maxRowIndexInExcel = GetMaxRowData(ws, ws.PhysicalNumberOfRows, "A");


            int currentRow = startTitleRowIndex+ titleRowCount;
            //vào excel đọc tiêu đề và đánh index 


            lstMapping = SetIndexMapping(ws, lstMapping, startTitleRowIndex, titleRowCount, maxColumnIndexInExcel, maxRowIndexInExcel);

            ///Bắt đầu import 
            ///
            for (int i = currentRow; i < maxRowIndexInExcel; i++)
            {
                var entity = (T)Activator.CreateInstance(typeof(T));


                for (int j = 0; j < lstMapping.Count; j++)
                {
                    var map = lstMapping[j];
                    var field = map.DataField;
                    var excelVal = ws.GetValue(i, map.ColumnIndexInExcel);

                    var prop = entity.GetType().GetProperty(field);

                    if (prop != null && prop.CanWrite)
                    {

                        prop.SetValue(entity, excelVal, null);

                    } 
                }

                entity.EntityState = CT.Models.Enumeration.EntityState.Add;


                


                lstEntity.Add(entity);


            }




            return lstEntity;
        }

        public static IWorkbook Export<T>(ref ISheet ws, ImportConfig importConfig, List<ImportMapping> lstMapping, List<T> lstData) where T : BaseEntity
        {

            int startTitleRowIndex = importConfig.StartTitleRowIndex;
            int titleRowCount = importConfig.TitleRowCount;
            int maxColumnIndexInExcel = importConfig.MaxColumnIndexInExcel;
            int maxRowIndexInExcel = importConfig.MaxRowIndexInExcel;

            maxRowIndexInExcel = (maxRowIndexInExcel == 0 ? 100 : maxRowIndexInExcel);
            maxColumnIndexInExcel = (maxColumnIndexInExcel == 0 ? 100 : maxColumnIndexInExcel);
            titleRowCount = (titleRowCount == 0 ? 1 : 0);

            int currentRow = startTitleRowIndex + titleRowCount;
            //vào excel đọc tiêu đề và đánh index 


            //     lstMapping = SetIndexMapping(ws, lstMapping, startTitleRowIndex, titleRowCount, maxColumnIndexInExcel, maxRowIndexInExcel);

            //header
            for (int i = 0; i < lstMapping.Count; i++)
            {
                var map= lstMapping[i];

                var propVal = map.Header;

                ws.SetValue(propVal, 0, map.ColumnIndexInExcel);

            }

            ///Bắt đầu export 

            for (int i = 0; i < lstData.Count; i++)
            {

                var entity = lstData[i];


                for (int j = 0; j < lstMapping.Count; j++)
                {
                    var map = lstMapping[j];
                    var field = map.DataField;

                    var prop = entity.GetType().GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (prop != null)
                    {
                        var propVal = prop.GetValue(entity);

                        ws.SetValue(propVal, currentRow + i, map.ColumnIndexInExcel);
                    }

                }


            }

              
            return ws.Workbook;
        }




        /// <summary>
        /// Đánh index column ở trên excel vào config (cái nào không map thì bỏ luôn)
        /// TODO có thể đánh theo Name cũng được, nó nhanh hơn là đọc tiêu đề?
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="lstMapping"></param>
        /// <param name="startTitleRowIndex"></param>
        /// <param name="titleRowCount"></param>
        /// <param name="maxColumnIndexInExcel"></param>
        /// <param name="maxRowIndexInExcel"></param>
        /// <returns></returns>
        /// phamkhanhhand 29.06.2023
        public static List<ImportMapping> SetIndexMapping(ISheet ws, List<ImportMapping> lstMapping, int startTitleRowIndex, int titleRowCount, int maxColumnIndexInExcel, int maxRowIndexInExcel)
        {

            //Mapping đã đánh index map vào excel
            var lstMappingIndexed = new List<ImportMapping>();

            //List<T> lstEntity = new List<T>();


            int currentRow = startTitleRowIndex;
            //vào excel đọc tiêu đề và đánh index

            for (int i = 0; i < maxColumnIndexInExcel; i++)
            {
                var lstColTitle = new List<string>();


                for (int j = 0; j < titleRowCount; j++)
                {
                    var cellTitle = ws.GetValueInMerge(currentRow + j, i);

                    cellTitle = cellTitle?.Trim();

                    lstColTitle.Add(cellTitle);

                }

                lstColTitle = lstColTitle.Where(x => !(string.IsNullOrWhiteSpace(x))).Distinct().ToList();
                var colHeaderText = string.Join(";", lstColTitle);

                //Tìm trong mapping, cái nào map thì lấy cho vào list

                var map = lstMapping.Find(x => string.Equals(x.Header, colHeaderText, StringComparison.OrdinalIgnoreCase));

                if (map != null)
                {
                    map.ColumnIndexInExcel = i;
                    lstMapping.Remove(map);
                    lstMappingIndexed.Add(map);

                }
            }

            return lstMappingIndexed;
        }

        //public void Export(ISheet ws)
        //{

        //}


    }
}
