using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
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
    /// DocumentFormat.OpenXml util  
    /// Thêm các hàm extend cho WordprocessingDocument npoi cho nó mạnh
    /// </summary>
    /// phamkhanhhand 23.06.2023
    public static partial class OpenXMLUtil
    {
        // Thay thế giá trị của bookmark trong file Word
        public static void SetBookmarkValue(this WordprocessingDocument wordDocument, string bookmarkName, string newValue)
        {
            // Tìm bookmark theo tên trong tài liệu
            var bookmarkStart = wordDocument.MainDocumentPart.Document.Descendants<BookmarkStart>()
                                      .Where(b => b.Name == bookmarkName)
                                      .FirstOrDefault();

            if (bookmarkStart != null)
            {
                // Tìm đoạn văn bản chứa bookmark và thay thế nội dung
                var bookmarkRun = bookmarkStart.Parent.Descendants<Run>().FirstOrDefault();

                if (bookmarkRun != null)
                {
                    var text = bookmarkRun.GetFirstChild<Text>();
                    if (text != null)
                    {
                        // Thay thế nội dung trong Text của bookmark
                        text.Text = newValue;
                    }
                }
            }


            //todo check xem có cần lưu k
            // Lưu lại tài liệu sau khi thay đổi
            //wordDocument.MainDocumentPart.Document.Save();
             
        }

    }
}
