using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Models.Entity
{
    [EntityKey("RoleID")] //đặt role_id là khóa chính
    public class adm_Role : BaseEntity
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string RoleCode { get; set; }
    }
}
