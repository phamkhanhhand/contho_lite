using CT.Models.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace CT.DL
{
    public class adm_EmployeeDL : BaseDL
    {
        DLUtil dLUtil = new DLUtil();



        /// <summary>
        /// Check permision
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// phamkhanhhand Sep 13, 2025
        public bool CheckPermision(string userName, List<string>listScope, string uri)
        {
            var rs = false;

            var sql = @"[dbo].[proc_CheckPermission]";

            // Tạo DataTable cho TVP
            DataTable scopeTable = new DataTable();
            scopeTable.Columns.Add("value", typeof(string));

            for (int i = 0;i< listScope.Count; i++)
            {
                scopeTable.Rows.Add(listScope[i]); 
            }

            SqlCommand cmd = new SqlCommand(sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Tham số TVP
            SqlParameter tvpParam = cmd.Parameters.AddWithValue("@list_scope", scopeTable);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = "dbo.type_list_string";

             
            cmd.Parameters.AddWithValue("@username", userName);
            cmd.Parameters.AddWithValue("@uri", uri);

            //DataPermissionService permissionService = new DataPermissionService();

            //permissionService.BuidWherePermissionClause(cmd, "employee");

            var rsDB = util.ExecuteScalar(cmd);

            if (rsDB != null && rsDB != DBNull.Value)
            {
                // Chuyển đổi an toàn sang bool
                rs = Convert.ToBoolean(rsDB);
            } 
            return rs;
        }


        /// <summary>
        /// Get employee by EployeeID
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// phamkhanhhand Nov 17, 2024
        public adm_Employee GetEmployeeByUsername(string userName)
        {

            var sql = @"
select *
from adm_Employee a
where a.Username = @Username 
";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@userName", userName);

            //DataPermissionService permissionService = new DataPermissionService();

            //permissionService.BuidWherePermissionClause( cmd, "employee");

            return util.SelectList<adm_Employee>(cmd).FirstOrDefault();

        }

        /// <summary>
        /// sql lấy quyền trực tiếp cho nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        /// thupth 23.11.2024
        
        
        // Lấy quyền trực tiếp cho nhân viên
        public List<ViewFeatureEmployee> DirectPermissionEmployee(int employeeID)
        {

            var sql = @"select
                        from 
                        join
                        where";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@employeeID", employeeID);
            return dLUtil.SelectList<ViewFeatureEmployee>(cmd);
        }



        //public PagedResult<Contract> GetContracts(int userId, string searchTerm, int pageIndex, int pageSize)
        //{

        //    PermissionService permissionService = new PermissionService();
        //    // Lấy các điều kiện phân quyền dữ liệu cho người dùng (đối với nhiều bảng)
        //    var dataPermissions = permissionService.GetDataPermissions(userId, "contract");

        //    // Xây dựng câu truy vấn SQL động cho nhiều bảng
        //    var query = @"
        //    SELECT * 
        //    FROM Contracts c
        //    JOIN Employees e ON e.EmployeeID = c.EmployeeID -- Giả sử có liên kết giữa hợp đồng và nhân viên
        //    JOIN Orders o ON o.OrderID = c.OrderID -- Giả sử có liên kết giữa hợp đồng và đơn hàng
        //    WHERE 1 = 1 ";

        //    // Thêm điều kiện phân quyền cho Contracts, Employees, Orders
        //    foreach (var permission in dataPermissions)
        //    {
        //        if (permission.EntityType == "Contracts")
        //        {
        //            query += $" AND {permission.Condition}";
        //        }
        //        if (permission.EntityType == "Employees")
        //        {
        //            query += $" AND {permission.Condition}";
        //        }
        //        if (permission.EntityType == "Orders")
        //        {
        //            query += $" AND {permission.Condition}";
        //        }
        //    }

        //    // Thêm điều kiện tìm kiếm
        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        query += " AND c.ContractName LIKE @SearchTerm ";
        //    }

        //    // Phân trang
        //    query += @"
        //    ORDER BY c.ContractID
        //    OFFSET @PageIndex * @PageSize ROWS
        //    FETCH NEXT @PageSize ROWS ONLY;";

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        var parameters = new
        //        {
        //            SearchTerm = $"%{searchTerm}%",
        //            PageIndex = pageIndex,
        //            PageSize = pageSize
        //        };

        //        var contracts = connection.Query<Contract>(query, parameters).ToList();

        //        // Lấy tổng số hợp đồng (để tính số trang)
        //        var totalQuery = "SELECT COUNT(*) FROM Contracts c WHERE 1 = 1";
        //        foreach (var permission in dataPermissions)
        //        {
        //            if (permission.EntityType == "Contracts")
        //            {
        //                totalQuery += $" AND {permission.Condition}";
        //            }
        //            if (permission.EntityType == "Employees")
        //            {
        //                totalQuery += $" AND {permission.Condition}";
        //            }
        //            if (permission.EntityType == "Orders")
        //            {
        //                totalQuery += $" AND {permission.Condition}";
        //            }
        //        }

        //        var totalCount = connection.ExecuteScalar<int>(totalQuery, new { SearchTerm = $"%{searchTerm}%" });

        //        return new PagedResult<Contract>
        //        {
        //            Items = contracts,
        //            TotalCount = totalCount,
        //            PageIndex = pageIndex,
        //            PageSize = pageSize
        //        };
        //    }

        //}


        }
}
