using CT.Models.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DL
{
    public class adm_RoleDL : BaseDL
    {

        DLUtil dLUtil = new DLUtil();


        /// <summary>
        /// Lấy danh sách quyền theo tính năng
        /// </summary>
        /// <param name="featureID"></param>
        /// <returns></returns>
        /// hathu 12.11.2024
        public List<ViewPermision> GetPermisionByFeatureID(int featureID)
        {


            var sql = @"
select c.FeatureName,
a.Username, 
d.FunctionCode
from adm_Employee a
inner join adm_Permission b on a.EmployeeID = b.EmployeeID
inner join adm_Feature c on b.FeatureID=c.FeatureID
inner join [adm_Function] d on b.FunctionID = d.FunctionID

where b.FeatureID = @featureID

";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@featureID", featureID);

            return dLUtil.SelectList<ViewPermision>(cmd);



        }



        /// hathu 22.11.2024
        // gán quyền nhân viên theo vai trò 
        public List<ViewFeatureRole> AssignPermission(int roleID, int featureID, int employeeID, int functionID)
        {

            // lệnh thêm vào quyền gồm 4 cột roleID, featureID,EmployeeID,FunctionID
            var sql = @"insert into adm_Permission(RoleID,FeatureID,EmployeeID,FunctionID) 
                        values(@roleID,@FeatureID,@EmployeeID,@FunctionID) ";
            //tạo đối tượng cmd để thực thi câu lệnh sql
            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@roleID", roleID);
            cmd.Parameters.AddWithValue("@featureID", featureID);
            cmd.Parameters.AddWithValue("@employeeID", employeeID);
            cmd.Parameters.AddWithValue("@functionID", functionID);

            return dLUtil.SelectList<ViewFeatureRole>(cmd);

        }
        //thu hồi quyền tính năng theo vai trò
        public List<ViewFeatureRole> RevokePermission(int roleID)
        {
            //lệnh delete quyền tính năng theo vai trò 
            var sql = @"delete from adm_Permission (RoleID,FeatureID,EmployeeID,FunctionID)
                        where RoleID = @roleID";
            
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@roleID", roleID);
            return dLUtil.SelectList<ViewFeatureRole>(cmd);
        }

        //hiển thị dữ liệu sau khi gán và thu hồi 
        public List<ViewFeatureRole> GetPermissionFeatureRole(int roleID)
        {
            //lệnh hiển thị dữ liệu sau khi gán, thu hồi
            var sql = @"select r.RoleName, f.FeatureName, e.FullName, mf.FunctionName
                        from adm_Permission p
                        inner join adm_Role r ON p.RoleID =r.RoleID
                        inner join adm_Feature f ON p.FeatureID =f.FeatureID
                        inner join adm_Employee e ON p.EmployeeID =e.EmployeeID
                        inner join adm_Function mf ON p.FunctionID =mf.FunctionID
                        where r.RoleID=@roleID ";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@roleID", roleID);

            cmd.CommandText = sql;
            return dLUtil.SelectList<ViewFeatureRole>(cmd);
        }

        public List<ViewFeatureRole> AssignPermission(int roleID)
        {
            throw new NotImplementedException();
        }
    }
}
