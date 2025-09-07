using System;
 
using Microsoft.Data.SqlClient; 
using CT.Models.Entity;
using CT.CurrentContext;

namespace CT.DL
{

    public class DataPermissionService
    {

        protected DLUtil util = new DLUtil();
        
        public   string BuidWherePermissionClause(SqlCommand cmd, string dataPermissionCode)
        {
            string rs = @"
and exists(
	select  *
	from {0} 
	where 1=1
    and {1}
)
                ";

             
            //todo lấy từ context 
            var currentUserID = CurrentUserService.GetCurrentProfileEmployee()?.EmployeeID;



            cmd.Parameters.AddWithValue("@pms_EmployeeID", currentUserID);
            cmd.Parameters.AddWithValue("@pms_OrganizationCode", "VP0001");//todo lấy từ context

            var dataPermissions = GetDataCondition(dataPermissionCode);

            if (dataPermissions != null)
            {
                var condition = dataPermissions.Condition;


                rs = string.Format(rs, dataPermissions.ViewName, condition);
            }
            else
            {
                rs = " ";
            }


            cmd.CommandText = cmd.CommandText.Replace("@KHPermision", rs);

            return rs;
        }

         


        // Lấy các quyền của người dùng đối với một loại dữ liệu cụ thể (ví dụ: Contracts)
        public List<string> GetUserPermissions(int userId, string dataPermissionCode)
        {
            // Truy vấn quyền của người dùng đối với loại dữ liệu (dataPermissionCode)
            var query = @"
                SELECT p.PermissionName
                FROM UserRoles ur
                JOIN RolePermissions rp ON ur.RoleId = rp.RoleId
                JOIN Permissions p ON rp.PermissionId = p.PermissionId
                JOIN DataPermissions dp ON rp.RoleId = dp.RoleId
                WHERE ur.UserId = @UserId AND dp.dataPermissionCode = @dataPermissionCode";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@dataPermissionCode", dataPermissionCode);

            return util.SelectList<string>(cmd);


        }


        // Lấy điều kiện phân quyền dữ liệu cho người dùng đối với loại dữ liệu cụ thể
        public  DataPermissions GetDataCondition(string dataPermissionCode)
        {
            // Truy vấn điều kiện phân quyền dữ liệu của người dùng đối với loại dữ liệu (DataPermissionCode)
            var query = @"

	   SELECT a.*
  FROM   DataPermissions a  
  WHERE  a.DataPermissionCode = @DataPermissionCode
 ";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
             
            cmd.Parameters.AddWithValue("@DataPermissionCode", dataPermissionCode);

            return util.GetByType<DataPermissions>(cmd);

             
        }
    }


}
