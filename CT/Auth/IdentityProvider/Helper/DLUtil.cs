 
using System.Data; 
using System.Reflection; 
using CT.Utils;
using Microsoft.Data.SqlClient;

namespace CT.Auth
{

    /// <summary>
    /// Tiện ích connect
    /// </summary>
    /// phamha
    public class DLUtil : IDisposable
    {

        private string connectionString = "";

        public void Dispose()
        {
            CloseConnection();
        }


        private SqlConnection con;
        //private string connectionString;

        public DLUtil(string connectionString = "")
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                //TODO cái này lúc nào tự viết
                //connectionString = FileManager.ReadConnectString();
            }
              
            connectionString = XmlConfigReader.GetAppSettingValue("db_connect_auth");
            this.connectionString = connectionString;
        }



        /// <summary>
        /// Mở connect
        /// </summary>
        /// phamha
        public void OpenConnection()
        {
            if (con == null || con.State == ConnectionState.Closed)
            { // we make sure we're only opening connection once.
                con = new SqlConnection(this.connectionString);
            }
        }

        /// <summary>
        /// Đóng connect
        /// </summary>
        /// phamha
        public void CloseConnection()
        {
            if (con != null && con.State == ConnectionState.Open)
            { // I'm making stuff up here
                con.Close();
            }
        }

        /// <summary>
        /// Lấy dataset sau khi thực hiện lệnh truy vấn sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// phamha
        public DataSet ExcuteDataSet(string sql)
        {
            OpenConnection();

            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            return ds;
        }

        /// <summary>
        /// Lấy dataset sau khi thực hiện lệnh truy vấn cmd
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        /// pkha 17.04.2023
        public int ExcuteNonQuery(SqlCommand cmd)
        {
            OpenConnection();
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Lấy dataset sau khi thực hiện lệnh truy vấn cmd
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        /// phamha
        public DataSet ExcuteDataSet(SqlCommand cmd)
        {
            OpenConnection();

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            return ds;
        }
          
        /// <summary>
        /// Insert vào bảng
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// phamha
        public bool Insert(string sql)
        {
            OpenConnection();

            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        /// Lấy danh sách entity theo id (vd: danh sách detail theo masterID)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Lấy thông tin theoID
        /// phamhaok
        public bool ExcuteSqlCommand(SqlCommand cmd, string sql)
        {
            OpenConnection();

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            return true;
        }


        /// <summary>
        /// Lấy danh sách column của bảng
        /// </summary>
        /// <returns></returns>
        /// pkha 23.07.2023
        public string[] GetColumnsName(string tableName)
        {
            OpenConnection();

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            List<string> listacolumnas = new List<string>();
            using (SqlCommand command = con.CreateCommand())
            {
                command.Parameters.AddWithValue("@tableName", tableName);

                command.CommandText = "select c.name from sys.columns c inner join sys.tables t on t.object_id = c.object_id and t.name = @tableName and t.type = 'U'";





                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listacolumnas.Add(reader.GetString(0));
                    }
                }
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return listacolumnas.ToArray();
        }


        /// <summary>
        /// Lấy danh sách entity theo id (vd: danh sách detail theo masterID)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Lấy thông tin theoID
        /// pkha 17.04.2023
        public List<object> SelectList(Type type, SqlCommand cmd)
        {
            OpenConnection();

            cmd.Connection = con;

            List<object> result = new List<object>();

            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            if (ds.Tables != null && ds.Tables.Count > 0)
            {
                var dataSet = ExcuteDataSet(cmd);

                if (dataSet.Tables.Count > 0)
                {
                    result = ConvertDataTableToList(type, dataSet.Tables[0]);
                }
            }

            return result;
        }


        /// <summary>
        /// Lấy danh sách entity theo id (vd: danh sách detail theo masterID)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Lấy thông tin theoID
        /// phamha
        public List<T> SelectList<T>(SqlCommand cmd)
        {
            OpenConnection();

            cmd.Connection = con;

            List<T> result = new List<T>();

            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            if (ds.Tables != null && ds.Tables.Count > 0)
            {
                var dataSet = ExcuteDataSet(cmd);

                if (dataSet.Tables.Count > 0)
                {
                    result = ConvertDataTableToList<T>(dataSet.Tables[0]);
                }
            }

            return result;
        }


        /// <summary>
        /// Lấy paging với column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// pkha 05.07.2023
        public List<T> SelectListWithTotal<T>(SqlCommand cmd, out int totalResultRecord, string totalColumnName = "pkha_totalPaging")
        {
            totalResultRecord = 0;

            OpenConnection();

            cmd.Connection = con;

            List<T> result = new List<T>();

            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            if (ds.Tables != null && ds.Tables.Count > 0)
            {
                var dataSet = ExcuteDataSet(cmd);

                if (dataSet.Tables.Count > 0)
                {
                    var dt = dataSet.Tables[0];

                    if (dt.Columns.Contains(totalColumnName) && dt.Rows.Count > 0)
                    {
                        var countString = dt.Rows[0][totalColumnName]?.ToString();

                        int.TryParse(countString, out totalResultRecord);
                    }


                    result = ConvertDataTableToList<T>(dataSet.Tables[0]);
                }
            }

            return result;
        }

        #region Support

        /// <summary>
        /// Convert  từ datatable sang 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// phamha
        private static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItemFromDataRow<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItemFromDataRow<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        var objectVal = dr[column.ColumnName];
                        if (objectVal != DBNull.Value)
                        {
                            pro.SetValue(obj, objectVal, null);
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }





        /// <summary>
        /// Convert  từ datatable sang 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// phamha
        private static List<object> ConvertDataTableToList(Type type, DataTable dt)
        {
            List<object> data = new List<object>();
            foreach (DataRow row in dt.Rows)
            {
                var item = GetItemFromDataRow(type, row);
                data.Add(item);
            }
            return data;
        }


        private static object GetItemFromDataRow(Type type, DataRow dr)
        {
            object obj = Activator.CreateInstance(type);

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in type.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        var objectVal = dr[column.ColumnName];
                        if (objectVal != DBNull.Value)
                        {
                            pro.SetValue(obj, objectVal, null);
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }




        #endregion



        #region Authen project

        /// <summary>
        /// Lấy danh sách entity theo id (vd: danh sách detail theo masterID)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Lấy thông tin theoID
        /// phamha
        public T Select<T>(SqlCommand cmd)
        {
            OpenConnection();

            cmd.Connection = con;

            T result = default(T);

            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            if (ds.Tables != null && ds.Tables.Count > 0)
            {
                var dataSet = ExcuteDataSet(cmd);

                if (dataSet.Tables.Count > 0)
                {
                    result = ConvertDataTableToList<T>(dataSet.Tables[0]).FirstOrDefault();
                }
            }

            return result;
        }
        #endregion
    }
}