using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
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
    /// Tiện ích Docx
    /// </summary>
    /// phamkhanhhand 23.06.2023
    public static class WordHelper
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


        public static void CreateAndSaveWordDocument(string filePath)
        {
            // Kiểm tra nếu đường dẫn không hợp lệ
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Đường dẫn file không hợp lệ.");
            }

            // Đảm bảo thư mục chứa file đã tồn tại
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory); // Tạo thư mục nếu chưa tồn tại
            }

            // Tạo tài liệu Word mới
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {

                // Tạo phần MainDocumentPart và thêm Document vào
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document(new Body());

                // Thêm nội dung vào tài liệu
                Body body = mainPart.Document.Body;

                wordDoc.MainDocumentPart.Document.Save();
            }

        }


        /// <summary>
        /// Lưu và giải phóng word
        /// </summary>
        /// <param name="wordDoc"></param>
        public static void SaveWordDocument(WordprocessingDocument wordDoc)
        {
                // Lưu tài liệu vào đường dẫn mới
                wordDoc.MainDocumentPart.Document.Save();
                wordDoc.Dispose();
        }

        // Đọc nội dung từ file Word
        public static WordprocessingDocument ReadWordFile(string filePath)
        {
            try
            {
                // Kiểm tra nếu file tồn tại
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File không tồn tại: {filePath}");
                }

                // Kiểm tra nếu file có định dạng .docx (DOCX là phần mở rộng tiêu chuẩn)
                if (Path.GetExtension(filePath).ToLower() != ".docx")
                {
                    throw new InvalidDataException("Định dạng file không phải DOCX.");
                }

                // Mở file Word trong chế độ đọc
                return WordprocessingDocument.Open(filePath, false);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;  // Trả về null nếu không thể mở file
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;  // Trả về null nếu file không đúng định dạng
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Lỗi: Không có quyền truy cập vào file. {ex.Message}");
                return null;  // Trả về null nếu không có quyền truy cập
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Lỗi: Lỗi trong quá trình đọc file. {ex.Message}");
                return null;  // Trả về null nếu gặp lỗi I/O (ví dụ: file bị khóa hoặc đang mở ở nơi khác)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
                return null;  // Trả về null nếu gặp lỗi không xác định
            } 
        }
        // Lưu tài liệu Word vào đường dẫn mới bằng cách tạo một bản sao


        
        public static WordprocessingDocument ReadWordFileNewClone(string sourceFilePath, string newFilePath)
        {
            WordprocessingDocument wordDoc = null;

            try
            {
                // Đảm bảo rằng đường dẫn không trùng lặp với file đang mở
                if (string.IsNullOrEmpty(newFilePath))
                {
                    throw new ArgumentException("Đường dẫn lưu file không hợp lệ.");
                }

                // Đảm bảo thư mục chứa file đã tồn tại
                string directory = Path.GetDirectoryName(newFilePath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory); // Tạo thư mục nếu chưa tồn tại
                }

                // Tạo một bản sao của file source
                File.Copy(sourceFilePath, newFilePath, overwrite: true);
                // Mở tài liệu đã sao chép để thao tác
                  wordDoc = WordprocessingDocument.Open(newFilePath, true);


                return wordDoc;

                //{
                //    // Lưu lại tài liệu
                //    wordDoc.MainDocumentPart.Document.Save(); 
                //}
            }
            catch (Exception ex)
            {
                return null;
            }
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
