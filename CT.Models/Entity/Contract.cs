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
    [EntityKey("ContractID")]
    public class Contract : BaseEntity
    {

        /// <summary>
        /// ID
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public Guid ContractID { get; set; }
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public string ContractCode { get; set; }
        /// <summary>
        /// Tiêu đề hợp đồng
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public string ContractName { get; set; }
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Loại tiền, theo enum currency js
        /// </summary>
        public int Currency { get; set; }

    }
}
