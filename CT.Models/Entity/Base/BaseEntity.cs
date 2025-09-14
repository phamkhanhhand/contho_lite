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
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public string? created_by { get; set; }
        public string? modified_by { get; set; }

        /// <summary>
        /// Version (DB sql server is TIMESTAMP); not datetime
        /// </summary>
        /// phamkhanhhand Dec 29, 2024
        public byte[]? edit_version { get; set; }



        public EntityState? EntityState { get; set; }

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


    }
}
