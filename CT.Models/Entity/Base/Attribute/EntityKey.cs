using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{
    public class EntityKey : Attribute
    {

        public EntityKey()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyname"></param>
        public EntityKey(string keyname)
        {
            this.KeyName = keyname;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyname"></param>
        /// <param name="sequenceName"></param>
        public EntityKey(string keyname, string sequenceName)
        {
            this.KeyName = keyname;
            this.SequenceName = sequenceName;
        }

        public string KeyName { get; set; }
        public string SequenceName { get; set; }
    }
}
