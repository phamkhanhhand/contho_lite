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
    /// phamkhanhhand 14.04.2023
    public class SalaryTableDetail : BaseEntity
    {

        public Guid? SalaryTableDetailID { get; set; }

        public Guid? SalaryTableID { get; set; }
        public Guid? EmployeeID { get; set; }

        /// <summary>
        /// Tên bảng lương
        /// </summary>
        public string EmployeeName { get; set; }

        public decimal? Amount { get; set; }

    }
}
