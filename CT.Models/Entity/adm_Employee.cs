using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{
    /// <summary>
    /// Employee
    /// </summary>
    /// phamkhanhhand 14.04.2023
    [EntityKey("EmployeeID", "adm_Employee_seq")]
    [DatabaseViewName("adm_Employee")]
    public class adm_Employee : BaseEntity
    {

        /// <summary>
        /// ID
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public int EmployeeID { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public string?  EmployeeCode { get; set; }
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public string EmployeeName { get; set; }
        public string? Username { get; set; }
        public DateTime? CreatedDate { get; set; }
         

    }
}
