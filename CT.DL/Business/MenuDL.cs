using CT.Models.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DL
{
    public class MenuDL : BaseDL
    {

        DLUtil dLUtil = new DLUtil();

         
        public List<adm_Feature> GetMenuByEmployeeID(  int employeeID)
        {
            var sql = @"proc_getAllMenuByEmployeeID ";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", employeeID); 

            return dLUtil.SelectList<adm_Feature>(cmd);



        }

        public List<ViewPermision> GetAllPermisionByFeatureAndEmployeeID(int employeeID, string featureCode)
        {
            var sql = @"proc_getAllPermisionByFeatureAndEmployeeID ";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", employeeID);
            cmd.Parameters.AddWithValue("@featureCode", featureCode);

            return dLUtil.SelectList<ViewPermision>(cmd);



        }


    }
}
