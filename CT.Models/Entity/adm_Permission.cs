using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{
    public class adm_Permission :BaseEntity
    {
        public int PermissionID { get; set; }  // ID của quyền hạn
        public int RoleID { get; set; }        // ID của vai trò
        public int FeatureID { get; set; }     // ID của tính năng
        public int FunctionID { get; set; }    // ID của chức năng
        public int EmployeeID { get; set; }    // ID của nhân viên
    }
}
