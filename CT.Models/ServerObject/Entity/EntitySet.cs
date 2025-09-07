using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.ServerObject
{
    public class EntitySet
    {

        /// <summary>
        /// Tên Entity
        /// </summary>
        public EntityItem Master { get; set; }

        private List<EntityCollection> details { get; set; }
        
        public List<EntityCollection> Details
        {
            get
            {
                if (details == null)
                {
                    details= new List<EntityCollection>();
                }
                return details;
            }
            set { details = value; }
        }

        #region origin

        public EntityItem OriginalMaster { get; set; }

        private List<EntityCollection> originalDetails { get; set; }

        /// <summary>
        /// Đối tượng entity
        /// </summary>
        public List<EntityCollection> OriginalDetails
        {
            get
            {
                if (originalDetails == null)
                {
                    originalDetails = new List<EntityCollection>();
                }
                return originalDetails;
            }
            set { originalDetails = value; }
        }
        #endregion

    }
}
