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
    [EntityKey("flex_value_id", "seq_adm_flex_value_sets")]
    [DatabaseViewName("adm_flex_value_sets")]
    public class adm_flex_value_sets : BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 flex_value_set_id { get; set; }
          
        /// <summary>
        /// name
        /// </summary> 
        public string? flex_value_set_name { get; set; }
          
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
