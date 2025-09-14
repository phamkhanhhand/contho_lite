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
    [EntityKey("flex_value_id", "seq_adm_flex_hierarchy")]
    [DatabaseViewName("adm_flex_hierarchy")]
    public class adm_flex_hierarchy : BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 flex_hierarchy_id { get; set; }

        /// <summary>
        /// ID set ref flex_value_sets
        /// </summary>
        public Int64 parent_flex_value_set_id { get; set; }
        public Int64 child_flex_value_set_id { get; set; }


        public Int64 parent_flex_value_id { get; set; }

        public Int64 child_flex_value_id { get; set; }

        /// <summary>
        /// child_value
        /// </summary> 
        public string? child_value { get; set; }

        /// <summary>
        /// parent_value
        /// </summary> 
        public string parent_value { get; set; }

        /// <summary>
        /// Enum HierarchyType
        /// 0-flexvalueset; 1-flexvalue
        /// </summary>
        public string? hierarchy_type { get; set; } 

         
         

    }
}
