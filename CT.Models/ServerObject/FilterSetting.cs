using CT.Models.Enumeration;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.ServerObject
{
    public class FilterSetting
    {
        /// <summary>
        /// Dữ liệu
        /// </summary>
        public FilterOperator Operator { get; set; }

        /// <summary>
        /// Số bản ghi trả ra
        /// </summary>
        public string Field { get; set; }
        public DataType DataType { get; set; }



        /// <summary>
        /// Số bản ghi phù hợp ở database
        /// </summary>
        public object Value { get; set; }


    }
}
