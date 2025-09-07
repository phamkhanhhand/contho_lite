 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.ImExReport.Entities
{

    public class ImportMapping1
    {

        public Guid ImportConfigID { get; set; }


        /// <summary>
        /// Nhiều level thì ngăn cách bằng dấu chấm phảy
        /// </summary>
        public string Header { get; set; }


        public string DataField { get; set; }


        public int DataType { get; set; }

        /// <summary>
        /// Index cột trong file excel
        /// </summary>
        public int ColumnIndexInExcel { get; set; }


        /// <summary>
        /// Trường bắt buộc không
        /// </summary>
        public bool Require { get; set; }



    }
}
