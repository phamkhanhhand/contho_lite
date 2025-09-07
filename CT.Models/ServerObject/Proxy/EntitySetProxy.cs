using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.ServerObject
{
    public class EntitySetProxy
    {
        /// <summary>
        /// Tên Entity
        /// </summary>
        public EntityItemProxy Master { get; set; }

        private List<EntityCollectionProxy> details { get; set; }

        public List<EntityCollectionProxy> Details
        {
            get
            {
                if (details == null)
                {
                    details = new List<EntityCollectionProxy>();
                }
                return details;
            }
            set { details = value; }
        }

    }
}
