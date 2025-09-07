using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.ServerObject
{
    public class EntityCollectionProxy
    {

        /// <summary>
        /// Tên Entity
        /// </summary>
        public string EntityName { get; set; }
        
        public string Items { get; set; }

        public Dictionary<string, object> Metadata { get; set; }
    }
}
