using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.ServerObject
{
    public class EntityCollection
    {

        /// <summary>
        /// Tên Entity
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Đối tượng entity
        /// </summary>
        public IList Items { get; set; }

        public Dictionary<string, object> Metadata { get; set; }

    }
}
