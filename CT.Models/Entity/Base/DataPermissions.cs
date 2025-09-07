using CT.Models.Enumeration;
using CT.Models.ServerObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace CT.Models.Entity
{
    public class DataPermissions: BaseEntity
    { 
        public int? EmployeeID { get; set; }
        public string? DataPermissionCode { get; set; }


        public string? Condition { get; set; }
        public string ViewName { get; set; }


    }
}
