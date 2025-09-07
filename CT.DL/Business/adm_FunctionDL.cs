using CT.Models.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DL
{
    public class adm_FunctionDL : BaseDL
    {

        DLUtil dLUtil = new DLUtil();



        public List<ViewPermision> GetPermisionByMyFunctionID(int functionID)
        {


            var sql = @"
               SELECT f.Name
                FROM [adm_Function] f
                WHERE f.FunctionID = @FunctionID

            ";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@functionID", functionID);

            return dLUtil.SelectList<ViewPermision>(cmd);



        }


    }
}
