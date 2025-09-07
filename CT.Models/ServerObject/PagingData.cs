using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.ServerObject
{
    public class PagingData
    {
        /// <summary>
        /// Dữ liệu
        /// </summary>
        public object ListObject { get; set; }

        /// <summary>
        /// Số bản ghi trả ra
        /// </summary>
        public int CurrentCount { get; set; }

        /// <summary>
        /// Số bản ghi phù hợp ở database
        /// </summary>
        public int TotalRecord { get; set; }
    }
}
