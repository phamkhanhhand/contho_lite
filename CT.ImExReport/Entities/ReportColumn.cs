using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.ImExReport.Entities
{

    public class ReportColumn
    {
        /// <summary>
        /// Vị trí cột trong Excel
        /// </summary>
        public int IndexColumnInExcel { get; set; }

        /// <summary>
        /// Level của header (cái cha thì nhỏ, càng con cháu thì số càng lớn)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Tiêu đề cột
        /// </summary>
        public string HeaderName { get; set; }

        /// <summary>
        /// Trường dữ liệu
        /// </summary>
        public string DataIndex { get; set; }

        /// <summary>
        /// Dataindex của cha
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// Loại dữ liệu (theo Enume)
        /// </summary>
        public int DataType { get; set; }

        #region Các trường căn chỉnh

        /// <summary>
        /// Độ nặng của chiều rộng
        /// </summary>
        public int Weight { get; set; }

        public decimal Width { get; set; }

        /// <summary>
        /// Sắp xếp
        /// </summary>
        public int SortOrder { get; set; }

        #endregion
    }
}
