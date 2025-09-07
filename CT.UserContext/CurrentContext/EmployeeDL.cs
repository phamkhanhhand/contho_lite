using CT.Models.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.UserContext.CurrentContext
{
    public class EmployeeDL
    {
        DLUtil dLUtil = new DLUtil();


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

            return dLUtil.SelectList<adm_Employee>(cmd).FirstOrDefault();

        }
          

        }
}
