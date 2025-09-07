using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT.DL.Business;

namespace CT.BL.Business
{
    public class adm_RoleEmployeeBL: BaseBL
    {

        //hiển thị vai trò theo mã nhân viên
        public Object GetRoleByEmployeeID(int employeeID)
        {
            adm_RoleEmployeeDL roleEmployeeDL = new adm_RoleEmployeeDL();
            return roleEmployeeDL.GetRoleByEmployeeID(employeeID);
        }


        // gán vai trò cho nhân viên
        public Object AssignRoleForEmploy(int employeeID)
        {
            adm_RoleEmployeeDL roleEmployeeDL = new adm_RoleEmployeeDL();
            return roleEmployeeDL.AssignRoleForEmployee(employeeID);
        }


        // hiển thị vai trò cho nhân viên sau khi gán
        public Object GetEmployeeRole(int employeeID)
        {
            adm_RoleEmployeeDL roleEmployeeDL = new adm_RoleEmployeeDL();
            return roleEmployeeDL.GetEmployeeRole(employeeID);
        }

        //hiển thị nhân viên với tất cả vai trò và vai trò tương ứng

        public Object GetEmployeeAllRole(int employeeID)
        {
            adm_RoleEmployeeDL roleEmployeeDL = new adm_RoleEmployeeDL();
            return roleEmployeeDL.GetEmployeeAllRole(employeeID);
        }
    }
}
