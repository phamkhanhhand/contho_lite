using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.DTO
{ 
    public class ParentChildDTO
    {
        public Int64 id;
        public List<Int64> ListParent;
        public List<Int64> ListChild;
    }
}
