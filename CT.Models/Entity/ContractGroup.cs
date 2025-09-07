using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{
    /// <summary>
    /// Nhóm hợp đồng
    /// </summary>
    /// phamkhanhhand 15.07.2023
    [DatabaseViewName("V_Contractgroup")]
    public class ContractGroup : BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        /// phamkhanhhand 15.07.2023
        public Guid ContractGroupID { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        /// phamkhanhhand 15.07.2023
        public string ContractGroupCode { get; set; }

        /// <summary>
        /// Tiêu đề hợp đồng
        /// </summary>
        /// phamkhanhhand 15.07.2023
        public string ContractGroupName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// phamkhanhhand 15.07.2023
        public DateTime CreatedDate { get; set; }


        #region Tree

        /// <summary>
        /// 
        /// </summary>
        /// phamkhanhhand 15.07.2023
        public Guid ParentID { get; set; }
        public Guid KeyID { get; set; }

        /// <summary>
        /// sắp xếp
        /// </summary>

        public int SortBy { get; set; }

        public bool isGroup { get; set; }

        #endregion


    }
}
