using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{
    public class adm_Function :BaseEntity
    {
        public int FunctionID { get; set; }
        public string FunctionCode { get; set; }
        public string Name{ get; set; }
    }
}
