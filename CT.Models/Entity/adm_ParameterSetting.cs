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
    [EntityKey("ParameterSettingID", "adm_ParameterSetting_seq")]
    [DatabaseViewName("adm_ParameterSetting")]
    public class adm_ParameterSetting : BaseEntity
    {

        /// <summary>
        /// ID
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public int?  ParameterSettingID { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public string? ParameterSettingName { get; set; }
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        /// phamkhanhhand 05.07.2023
        public string?  ParameterSettingValue { get; set; }
        public string? Note { get; set; } 
         

    }
}
