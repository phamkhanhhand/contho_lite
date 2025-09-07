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
    [EntityKey("ImportMappingID")]
    public class ImportMapping : BaseEntity
    {

        /// <summary>
        /// ID
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public Guid? ImportMappingID { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public string? Header { get; set; }
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public string DataField { get; set; }
        public int? DataType { get; set; }
        public bool? Require { get; set; }
        public int ColumnIndexInExcel { get; set; } 


        public Guid? ImportConfigID { get; set; }
        public DateTime? CreatedDate { get; set; }


    }
}
