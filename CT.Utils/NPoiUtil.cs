using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Spire.Pdf.Fields;
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
    /// Npoi util
    /// Thêm các hàm extend cho ISheet npoi cho nó mạnh
    /// </summary>
    /// phamkhanhhand 23.06.2023
    public static partial class NPoiUtil
    {

        #region Thao tác với excel

        public static string webRootPath = "";


        /// <summary>
        /// Lấy cell
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static ICell GetCell(this ISheet ws, string address)
        {

            var cr = new CellReference(address);
            var row = ws.GetRow(cr.Row);
            return row ? .GetCell(cr.Col);
        }

        /// <summary>
        /// Lấy cell
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static ICell GetCell(this ISheet ws, int row, int col)
        {
            var rObj = ws.GetRow(row);

            if (rObj == null)
            {
                rObj = ws.CreateRow(row);

            }

            var cellObj = rObj.GetCell(col);

            if (cellObj == null)
            {
                cellObj = rObj.CreateCell(col);

            }


            return cellObj;
        }

        /// <summary>
        /// Set value cho cell
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="value"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="type"></param>
        public static void SetValue(this ISheet ws, object value, int row, int col, int type = 0)
        {
            var cell = ws.GetCell(row, col);

            var val = value.ToString();

            cell.SetCellValue(val); 
        }



        public static string GetValue(this ISheet ws, int row, int col, int type = 0)
        {
            var cell = ws.GetCell(row, col);
            return cell?.ToString();
        }

        public static string GetValueInMerge(this ISheet ws, int row, int col, int type = 0)
        {
            var cell = ws.GetCell(row, col);

            if (cell.IsMergedCell)
            {
                cell = GetFirstCellInMergedRegionContainingCell(cell);
            }

            return cell?.ToString();
        }

        public static ICell GetFirstCellInMergedRegionContainingCell(ICell cell)
        {
            if (cell != null && cell.IsMergedCell)
            {
                ISheet sheet = cell.Sheet;
                for (int i = 0; i < sheet.NumMergedRegions; i++)
                {
                    CellRangeAddress region = sheet.GetMergedRegion(i);
                    if (region.ContainsRow(cell.RowIndex) &&
                        region.ContainsColumn(cell.ColumnIndex))
                    {
                        IRow row = sheet.GetRow(region.FirstRow);
                        ICell firstCell = row?.GetCell(region.FirstColumn);
                        return firstCell;
                    }
                }
                return null;
            }
            return cell;
        }
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


        #region Chưa phân loại



        #region Cell

        ///// <summary>
        ///// Lấy/tạo cell theo name
        ///// </summary>
        ///// <param name="sheet"></param>
        ///// <param name="cellName"></param>
        ///// <returns></returns>
        //public static ICell GetCellByName(ISheet sheet, string cellName)
        //{
        //    var cr = new CellReference(cellName);

        //    return GetCellByNumber(sheet, cr.Row, cr.Col);
        //}

        /// <summary>
        /// Lấy/tạo cell theo name
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="cellName">Có cả tên sheet name</param>
        /// <returns></returns>
        public static ICell GetCellByName(this IWorkbook wb, string cellName)
        {
            var cr = new CellReference(cellName);
            var sheet = wb.GetSheet(cr.SheetName);

            return GetCellByNumber(sheet, cr.Row, cr.Col);
        }

        /// <summary>
        /// Lấy/tạo cell theo địa chỉ x, y (từ 0)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static ICell GetCellByNumber(this ISheet sheet, int rowIndex, int colIndex)
        {

            //Nếu chưa có thì tạo
            var row = sheet.GetRow(rowIndex);
            if (row == null)
            {
                sheet.CreateRow(rowIndex);
                row = sheet.GetRow(rowIndex);
            }
            var cell = row.GetCell(colIndex);
            if (cell == null)
            {
                row.CreateCell(colIndex);
                cell = row.GetCell(colIndex);
            }

            return cell;
        }

        #endregion

        #region Lấy/đặt giá trị

        /// <summary>
        /// Lấy value
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static object GetCellValue(ICell cell)
        {
            object cValue = string.Empty;
            switch (cell.CellType)
            {
                case (CellType.Unknown | CellType.Formula | CellType.Blank):
                    cValue = cell.ToString();
                    break;
                case CellType.Numeric:
                    cValue = cell.NumericCellValue;
                    break;
                case CellType.String:
                    cValue = cell.StringCellValue;
                    break;
                case CellType.Boolean:
                    cValue = cell.BooleanCellValue;
                    break;
                case CellType.Error:
                    cValue = cell.ErrorCellValue;
                    break;
                default:
                    cValue = string.Empty;
                    break;
            }
            return cValue;
        }
        //setvalue, nó tự có rồi (nếu cần thì cứ show ra, đến cuối cùng không thuộc kiểu nào thì tostring())

        #endregion

        /// <summary>
        /// Merge cell
        /// </summary>
        /// <param name="ws">sheet</param>
        /// <param name="x">cột</param>
        /// <param name="y">dòng</param>
        /// <param name="dx">số cột merge</param>
        /// <param name="dy">số dòng merge</param>
        public static int MergeCell(this ISheet ws, int x, int y, int dx, int dy)
        {
            int indexRegion = -1;
            if (dx > 0 && dy > 0)
            {
                var cra = new NPOI.SS.Util.CellRangeAddress(x, x + dx, y, y + dy);
                indexRegion = ws.AddMergedRegion(cra);
            }

            return indexRegion;
        }


        /// <summary>
        /// Lấy index của merge cell
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private static int GetIndexIfCellIsInMergedCells(this ISheet ws, int row, int column)
        {
            int numberOfMergedRegions = ws.NumMergedRegions;

            for (int i = 0; i < numberOfMergedRegions; i++)
            {
                CellRangeAddress mergedCell = ws.GetMergedRegion(i);

                if (mergedCell.IsInRange(row, column))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// UnMerge cell
        /// </summary>
        /// <param name="ws">sheet</param>
        /// <param name="x">cột</param>
        /// <param name="y">dòng</param>
        public static void UnMergeCell(this ISheet ws, int x, int y)
        {
            if (x >= 0 && y >= 0)
            {
                var mergeIndex = ws.GetIndexIfCellIsInMergedCells(x, y);

                if (mergeIndex >= 0)
                {
                    ws.RemoveMergedRegion(mergeIndex);
                }
            }
        }

        //public static void SetStyle(ICell cell, ICellStyle style)
        //{
        //    //clone ra trước kẻo áp vào các cái khác (không cần)
        //    cell.CellStyle = style;
        //}


        //Read data to DataTable


        /// <summary>
        /// Đổ dữ liệu từ excel vào dataTable
        /// </summary>
        /// <param name="sheet"></param>
        public static DataTable ReadToDataTable(this ISheet sheet, int startRow = 0, int startCol = 0)
        {
            var dataTable = new DataTable(sheet.SheetName);

            // write the header row
            var headerRow = sheet.GetRow(0);

            foreach (var headerCell in headerRow)
            {
                dataTable.Columns.Add(headerCell.ToString());
            }

            // write the rest (cast từng dòng một). TODO từng cột 1 thì nó đỡ mất nhiều hơn
            for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
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

        /// <summary>
        /// Datatable vào Excel
        /// todo: kèm loại dữ liệu nữa (có thể tách ra thành hàm riêng)
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="dt"></param>
        /// <param name="startRow">Dòng bắt đầu (tính từ 0)</param>
        /// <param name="startCol">Cột bắt đầu (Tính từ 0)</param>
        public static void DataTableToExcel(this ISheet ws, DataTable dt, int startRow = 0, int startCol = 0)
        {
            //TODO (có thể tách hàm riêng), tạo header
            //Below loop is fill content

            var lstColumn = dt.Columns;
            var lstRow = dt.Rows;

            for (int i = 0; i < lstRow.Count; i++)
            {
                var dr = lstRow[i];

                var rowIndex = i;
                var cellRow = ws.CreateRow(rowIndex + startRow);

                for (int j = 0; j < lstColumn.Count; j++)
                {
                    var cell = cellRow.CreateCell(startCol + j);

                    var value = dr[j];
                    cell.SetCellValue(value);
                }
            }

        }

        /// <summary>
        /// Bind vào defind
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="dic">name-value</param>
        public static void BindToDefindName(this IWorkbook wb, Dictionary<string, object> dic)
        {
            if (dic != null && dic.Count > 0)
            {
                foreach (var item in dic)
                {
                    wb.SetValueByName(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// Lấy data theo defined name
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="ws"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetValueByName(this IWorkbook wb, string name)
        {
            var lstName = wb.GetAllNames();

            var currentDefind = lstName.Where(x => x.NameName == name).FirstOrDefault();

            if (currentDefind != null)
            {
                var cell = GetCellByName(wb, currentDefind.RefersToFormula);

                return GetCellValue(cell);
            }

            return null;
        }

        /// <summary>
        /// Đặt giá trị dưới dạng object
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public static void SetCellValue(this ISheet ws, int row, int col, object value)
        {
            var cell = GetCellByNumber(ws, row, col);

            cell.SetCellValue(value);
        }

        /// <summary>
        /// Đặt giá trị dưới dạng object
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public static void SetCellValue(this ICell cell, object value)
        {

            if (cell != null)
            {

                if (value.GetType() == typeof(decimal))
                {
                    cell.SetCellValue((string)value);
                }
                else if (value.GetType() == typeof(double))
                {
                    cell.SetCellValue((double)value);
                }
                else if (value.GetType() == typeof(int))
                {
                    cell.SetCellValue((int)value);
                }
                else if (value.GetType() == typeof(bool))
                {
                    cell.SetCellValue((bool)value);
                }
                else if (value.GetType() == typeof(DateTime))
                {
                    cell.SetCellValue((DateTime)value);
                }
                else
                {
                    cell.SetCellValue(value.ToString());
                }
            }
        }

        /// <summary>
        /// Set value cho excel theo name
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="ws"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetValueByName(this IWorkbook wb, string name, object value)
        {
            var lstName = wb.GetAllNames();

            var currentDefind = lstName.Where(x => x.NameName == name).FirstOrDefault();

            if (currentDefind != null)
            {
                var cell = GetCellByName(wb, currentDefind.RefersToFormula);

                cell.SetCellValue(value);

            }

        }


        #endregion 

    }
}
