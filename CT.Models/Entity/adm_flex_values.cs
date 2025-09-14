using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{
    /// <summary>
    /// Flex value
    /// </summary>
    /// phamkhanhhand sep 14, 2025
    [EntityKey("flex_value_id", "seq_adm_flex_values")]
    [DatabaseViewName("adm_flex_values")]
    public class adm_flex_values : BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 flex_value_id { get; set; }

        /// <summary>
        /// ID set ref flex_value_sets
        /// </summary>
        public Int64 flex_value_set_id { get; set; }

        /// <summary>
        /// value code
        /// </summary> 
        public string? flex_value { get; set; }

        /// <summary>
        /// value name
        /// </summary> 
        public string flex_value_name { get; set; }

        /// <summary>
        /// Enum (class static) EnableFlag
        /// Y-enable; N-disable
        /// </summary>
        public string? enable_flag { get; set; }

        /// <summary>
        /// Period: ex: 2025, 2026...
        /// </summary>
        public string? period { get; set; }

        /// <summary>
        /// description
        /// </summary>
        public string? description { get; set; }

         
         

    }
}
