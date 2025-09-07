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
    /// phamha 14.04.2023
    [EntityKey("AttachmentID", "cm_Attachment_seq")]
    public class cm_Attachment : BaseEntity
    {

        /// <summary>
        /// ID
        /// </summary>
        /// phamha Dec 25,2024
        public int AttachmentID { get; set; }
         
        public string FileName { get; set; }
        public int? RefID { get; set; }
        public int? RefType { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDTG { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDTG { get; set; }
    }
}
