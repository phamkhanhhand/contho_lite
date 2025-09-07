using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{
    /// <summary>
    /// Bảng lương
    /// </summary>
    /// phamha 14.04.2023
    [EntityKey("ImportConfigID")]
    public class ImportConfig : BaseEntity
    {

        /// <summary>
        /// ID
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public Guid ?  ImportConfigID { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public string? ImportConfigCode { get; set; }
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public string Comment { get; set; }


        public int  StartTitleRowIndex { get; set; }
        public int  TitleRowCount { get; set; }
        public int  MaxColumnIndexInExcel { get; set; }
        public int  MaxRowIndexInExcel { get; set; }



        public DateTime? CreatedDate { get; set; }
         

    }
}
