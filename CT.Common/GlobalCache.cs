using CT.DL;
using CT.Models.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Common
{
    /// <summary>
    /// Cache dùng chung cho toàn bộ dự án, load 5 phút 1 lần
    /// </summary>
    /// phamkhanhhand Dec 30, 2024
    public class GlobalCache
    {

        public static List<adm_ParameterSetting> lstAdm_ParameterSetting = new List<adm_ParameterSetting>();

        public static void LoadData()
        {
            var parameterSettingDL = new adm_ParameterSettingDL();
            lstAdm_ParameterSetting = parameterSettingDL.GetListDataByColumn<adm_ParameterSetting>("1", "1");
        }


        public static void UpdateSystemDate()
        {
            var parameterSettingDL = new adm_ParameterSettingDL();
            var dlUtil = new DLUtil();

            SqlCommand cmd = new SqlCommand();
            var sql = " exec proc_updateTimeDifference @BEDate ";
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@BEDate", DateTime.Now);
            dlUtil.ExcuteNonQuery(cmd);

        }

    }
}
