 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.ImExReport.Entities
{

    public class ImportConfig1
    {

        public Guid ImportConfigID { get; set; }
        public string ImportConfigCode { get; set; }
        public string Comment { get; set; }


        /// <summary>
        /// Dòng bắt đầu của tiêu đề (excel tính từ dòng 0)
        /// </summary>
        public int StartTitleRowIndex { get; set; }

        /// <summary>
        /// Tổng số dòng tiêu đề
        /// </summary>
        public int TitleRowCount { get; set; }

        /// <summary>
        /// Số cột tối đa quét dữ liệu
        /// </summary>
        public int MaxColumnIndexInExcel { get; set; } = 20;

        /// <summary>
        /// Số dòng tối đa có dữ liệu
        /// </summary>
        public int MaxRowIndexInExcel { get; set; } = 10;



    }
}
