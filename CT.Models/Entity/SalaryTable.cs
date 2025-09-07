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
    public class SalaryTable : BaseEntity
    {

        public Guid? SalaryTableID { get; set; }

        /// <summary>
        /// Tên bảng lương
        /// </summary>
        public string SalaryTableName { get; set; }
        public int SalaryMonth { get; set; }
        public int SalaryYear { get; set; }


    }
}
