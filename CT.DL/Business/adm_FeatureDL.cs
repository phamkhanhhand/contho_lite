using CT.Models.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DL
{
   public class adm_FeatureDL : BaseDL
    {


        DLUtil dLUtil = new DLUtil();

        /// <summary>
        /// Lấy danh sách quyền theo tính năng
        /// </summary>
        /// <param name="featureID"></param>
        /// <returns></returns>
        /// hathu 12.11.2024
        //gán quyền tính năng cho nhân viên(thêm)
        public List<ViewFeatureEmployee>AssignPermissionFeatureEmployee(int featureID,int employeeID,int functionID)
        {
            //gán quyền tính năng cho nhân viên(thêm)
            var sql = @"intsert into adm_Permission(FeatureID,EmployeeID,FunctionID)
                        values(@featureID,@employeeID,@functionID)";

            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.AddWithValue("@fearureID", featureID);
            cmd.Parameters.AddWithValue("@employeeID", employeeID);
            cmd.Parameters.AddWithValue("@functionID", functionID);

            return dLUtil.SelectList<ViewFeatureEmployee>(cmd);
        }

        //thu hồi quyền tính năng cho nhân viên(Xóa)
        public List<ViewFeatureEmployee> RevokePermissionFeatureEmployee(int featureID)
        {
            var sql = @"delete from adm_Permission where FeatureID=@featureID";
            SqlCommand cmd = new SqlCommand();
           
            cmd.Parameters.AddWithValue("@featureID", featureID);

            return dLUtil.SelectList<ViewFeatureEmployee>(cmd);
        }
        public List<ViewFeatureEmployee> GetPermisionByFeatureID(int featureID)
        {

            //lấy quyền hạn của nhân viên theo tính năng, chức năng
            var sql = @" select e.FullName,f.FeatureName,mf.FunctionName
                         from adm_Permission p
                         inner join adm_Employee e on p.EmployeeID = e.EmployeeID
                         inner join adm_Feature f on p.FeatureID = f.FeatureID
                         inner join adm_Function mf on p.FunctionID = mf.FunctionID
                         where f.featureID = @featureID;

";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@featureID", featureID);
            return dLUtil.SelectList<ViewFeatureEmployee>(cmd);
           
         



        }

        public List<ViewFeatureEmployee> AssignPermissionFeatureEmployee(int employeeID)
        {
            throw new NotImplementedException();
        }
    }
}
