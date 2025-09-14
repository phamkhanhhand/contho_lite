
using CT.Models.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CT.DL
{
    public class adm_flex_value_setsDL : BaseDL
    {



        /// <summary>
        /// Check permision
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// phamkhanhhand Sep 13, 2025
        public bool AddLink(ParentChildDTO parentChildDTO)
        {
            var rs = false;

            var sql = @"[dbo].[proc_save_hierarchy_set]";

            // Tạo DataTable cho TVP
            DataTable scopeTableChild = new DataTable();
            scopeTableChild.Columns.Add("value", typeof(string));

            for (int i = 0; i < parentChildDTO.ListChild?.Count; i++)
            {
                scopeTableChild.Rows.Add(parentChildDTO.ListChild[i]);
            }

            DataTable scopeTableParent = new DataTable();
            scopeTableParent.Columns.Add("value", typeof(string));

            for (int i = 0; i < parentChildDTO.ListParent?.Count; i++)
            {
                scopeTableParent.Rows.Add(parentChildDTO.ListParent[i]);
            }


            SqlCommand cmd = new SqlCommand(sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Tham số TVP
            SqlParameter tvpParam = cmd.Parameters.AddWithValue("@list_child", scopeTableChild);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = "dbo.type_list_bigint";


            SqlParameter tvpParamParent = cmd.Parameters.AddWithValue("@list_parent", scopeTableParent);
            tvpParamParent.SqlDbType = SqlDbType.Structured;
            tvpParamParent.TypeName = "dbo.type_list_bigint";


            cmd.Parameters.AddWithValue("@id", parentChildDTO.id);


            var rsDB = util.ExecuteScalar(cmd);

            if (rsDB != null && rsDB != DBNull.Value)
            {
                // Chuyển đổi an toàn sang bool
                rs = Convert.ToBoolean(rsDB);
            }
            return rs;
        }


    }
}
