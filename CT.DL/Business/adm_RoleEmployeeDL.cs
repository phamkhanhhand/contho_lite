using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT.Models.Entity;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.Data.SqlClient;

namespace CT.DL.Business
{
    public class adm_RoleEmployeeDL: BaseDL
    {
        DLUtil dLUtil = new DLUtil();
        
        // hiển thị vai trò theo mã nhân viên
        public List<adm_Role> GetRoleByEmployeeID(int employeeID)
        {

            var sql = @"select r.RoleCode,r.RoleName,r.Description
                        from adm_role r
                        join adm_EmployeeRole er on r.RoleID = er.RoleID
                        join adm_employee e on er.EmployeeID =e.EmployeeID
                        where e.EmployeeID =@employeeID ";

            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.AddWithValue("@employeeID", employeeID);
            cmd.CommandText = sql;


            return dLUtil.SelectList<adm_Role>(cmd);
        }

        // gán vai trò cho nhân viên(Thêm)
        public List<adm_Employee> AssignRoleForEmployee(int employeeID, int roleID)
        {

            var sql = @"insert into adm_EmployeeRole(EmployeeID,RoleID) values(@employeeID, @roleID)";
            
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("employeeID", employeeID);
            cmd.Parameters.AddWithValue("roleID", roleID);
            
            return dLUtil.SelectList<adm_Employee>(cmd);
        }

        //hiển thị nhân viên sau khi gán
        public List<adm_Employee> GetEmployeeRole(int employeeID)
        {
            var sql = @"select e.EmployeeName,e.UserName,r.RoleName,r.Description
                        from adm_Employee e
                        join adm_EmployeeRole er on e.EmployeeID = er.EmployeeID
                        join adm_Role r on er.RoleID = r.RoleID
                        where e.EmployeeID=@employeeID";
           
            
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("employeeID", employeeID);

            return dLUtil.SelectList<adm_Employee>(cmd);
        }

        // hiển thị nhân viên với tất cả vai trò và vai trò tương ứng 
        public List<adm_Employee> GetEmployeeAllRole(int employeeID)
        {
            var sql = @"select e.EmployeeName,e.UserName,r.RoleName,r.Description,
                        CASE
                            when er.EmployeeID is not null then 1
                            else 0
                        END as isChecked
                        from adm_Role r
                        left join adm_EmployeeRole er on r.RoleID = er.RoleID
                        left join adm_Employee e on er.EmployeeID =e.EmployeeID
                        where e.EmployeeID = @employeeID ";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("employeeID", employeeID);

            return dLUtil.SelectList<adm_Employee>(cmd);

        }

        public object AssignRoleForEmployee(int employeeID)
        {
            throw new NotImplementedException();
        }
    }
}
