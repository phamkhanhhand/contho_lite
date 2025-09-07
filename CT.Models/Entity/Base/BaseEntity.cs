using CT.Models.Enumeration;
using CT.Models.ServerObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace CT.Models.Entity
{
    public class BaseEntity
    {
        public EntityState ?  EntityState { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        private List<ErrorObject> listErrorObject;

        public List<ErrorObject> ListErrorObject
        {
            get
            {
                if (listErrorObject == null)
                {
                    listErrorObject = new List<ErrorObject>();
                }
                return listErrorObject;
            }
            set { listErrorObject = value; }
        }


        /// <summary>
        /// Version (DB sql server là TIMESTAMP); không phải thời gian
        /// </summary>
        /// phamha Dec 29, 2024
        public byte[]? EditVersion { get; set; }

    }
}
