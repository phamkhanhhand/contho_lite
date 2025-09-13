//using CT.Common;
using CT.Models.Entity;
using CT.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;

namespace CT.DL
{

    /// <summary>
    /// Tiện ích connect
    /// </summary>
    /// phamkhanhhand
    public class DLUtil : IDisposable
    {

        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";

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

            connectionString = XmlConfigReader.GetAppSettingValue("db_connect");

            //connectionString = "Server=ALEXANDERKEVIN\\SQLEXPRESS;Database=CT;Trusted_Connection=True;;TrustServerCertificate=True;";
            //connectionString = "Server=ALEXANDERKEVIN\\SQLEXPRESS;Database=CT;User Id=sa;Password=1;TrustServerCertificate=True ";
       
            this.connectionString = connectionString;
        }



        /// <summary>
        /// Mở connect
        /// </summary>
        /// phamkhanhhand
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
        /// phamkhanhhand
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
        /// phamkhanhhand
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
        /// phamkhanhhand 17.04.2023
        public int ExcuteNonQuery(SqlCommand cmd)
        {
            OpenConnection();
            cmd.Connection = con;

            if(con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            return cmd.ExecuteNonQuery();
        }


        public object ExecuteScalar(SqlCommand cmd)
        {
            OpenConnection();
            cmd.Connection = con;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            return cmd.ExecuteScalar();
        }


        /// <summary>
        /// Lấy dataset sau khi thực hiện lệnh truy vấn cmd
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        /// phamkhanhhand
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
        /// Lấy entity theo id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// phamkhanhhand 07.07.2023
        public T GetByID<T>(object id)
        {
            T result = default(T);
            var tableName = typeof(T).Name;

            var keyName = tableName + "ID";

            string viewName = "";
            bool findingViewName = true,//timf view
                findingKey = true;//tim key cua bang


            System.Reflection.MemberInfo info = typeof(T);
            object[] attributes = info.GetCustomAttributes(true);

            for (int i = 0; i < attributes.Length; i++)
            {
                if (findingViewName && attributes[i] is DatabaseViewName)
                {
                    viewName = ((DatabaseViewName)attributes[i]).ViewName;
                    findingViewName = false;
                }

                if (findingKey && attributes[i] is EntityKey)
                {
                    keyName = ((EntityKey)attributes[i]).KeyName;
                    findingKey = false;
                }
                if (!(findingKey || findingViewName))
                {
                    break;
                }

            }

            viewName = string.IsNullOrWhiteSpace(viewName) ? tableName : viewName;

            //Nếu id là guid thì cho dấu nháy vào
            if (id is Guid)
            {
                id += "\"" + id + "\"";
            }

            var sql = "select * from [{0}] where {1}=" + id;

            sql = string.Format(sql, viewName, keyName);



            OpenConnection();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            if (ds.Tables != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var fistRow = ds.Tables[0].Rows[0];

                    result = GetItemFromDataRow<T>(fistRow);
                }

            }

            return result;
        }



        /// <summary>
        /// Lấy entity theo id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// phamkhanhhand
        /// Lấy thông tin theoID
        public T GetByType<T>(SqlCommand cmd)
        {
            OpenConnection();
            var result = default(object);
            var type = typeof(T);


             

            DataSet ds = new DataSet();
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var fistRow = ds.Tables[0].Rows[0];

                result = GetItemFromDataRow(type, fistRow);

            }

            return (T)result;
        }



        /// <summary>
        /// Lấy entity theo id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// phamkhanhhand
        /// Lấy thông tin theoID
        public object GetByID(Type type, object id)
        {
            OpenConnection();
            var result = default(object);
            var tableName = type.Name;

            var keyName = tableName + "ID";


            System.Reflection.MemberInfo info = type;
            object[] attributes = info.GetCustomAttributes(true);

            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i] is EntityKey)
                {
                    keyName = ((EntityKey)attributes[i]).KeyName;
                }
            }


            //Nếu id là guid thì cho dấu nháy vào
            //if (id is Guid)
            //{
            //    id += "\"" + id + "\"";
            //}

            id = "'" + id + "'";


            var sql = "select * from {0} where {0}ID=" + id;

            sql = string.Format(sql, tableName, keyName);

            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var fistRow = ds.Tables[0].Rows[0];

                result = GetItemFromDataRow(type, fistRow);

            }

            return result;
        }

        /// <summary>
        /// Get data by columnvalue
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        /// phamkhanhhand Nov 23, 2024
        public T GetByColumn<T>(string columnName, object columnValue)
        {
            var type = typeof(T);

            OpenConnection();
            var result = default(object);
            var tableName = type.Name; 

            System.Reflection.MemberInfo info = type;
            object[] attributes = info.GetCustomAttributes(true);


            //Nếu id là guid thì cho dấu nháy vào
            //if (id is Guid)
            //{
            //    id += "\"" + id + "\"";
            //}

            columnValue = "'" + columnValue + "'";


            var sql = "select top 1 * from {0} where {1}=" + columnValue;

            sql = string.Format(sql, tableName, columnName);

            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var fistRow = ds.Tables[0].Rows[0];

                result = GetItemFromDataRow(type, fistRow);

            }

            return (T)result;
        }


        /// <summary>
        /// Get data by columnvalue
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        /// phamkhanhhand Nov 23, 2024
        public List<T> GetListByColumn<T>(string columnName, object columnValue)
        {
            var type = typeof(T);

            OpenConnection();
            var result = default(object);
            var tableName = type.Name;

            System.Reflection.MemberInfo info = type;
            object[] attributes = info.GetCustomAttributes(true);


            //Nếu id là guid thì cho dấu nháy vào
            //if (id is Guid)
            //{
            //    id += "\"" + id + "\"";
            //}

            columnValue = "'" + columnValue + "'";


            var sql = "select * from {0} where {1}=" + columnValue;

            sql = string.Format(sql, tableName, columnName);

            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.SelectCommand = cmd;
            da.Fill(ds);

            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var dt = ds.Tables[0]; 

                result = ConvertDataTableToList<T>(dt);

            }

            return (List<T>)result;
        }




        /// <summary>
        /// Insert vào bảng
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// phamkhanhhand
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
        /// phamkhanhhandok
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
        /// Lấy danh sách entity theo id (vd: danh sách detail theo masterID)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Lấy thông tin theoID
        /// phamkhanhhandok
        public object ExcuteSqlCommandScalar(SqlCommand cmd, string sql)
        {
            OpenConnection();

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.Connection = con;
            cmd.CommandText = sql;
            return cmd.ExecuteScalar(); 
        }


        /// <summary>
        /// Lấy danh sách column của bảng
        /// </summary>
        /// <returns></returns>
        /// phamkhanhhand 23.07.2023
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
        /// phamkhanhhand 17.04.2023
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
        /// phamkhanhhand
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
        /// phamkhanhhand 05.07.2023
        public List<T> SelectListWithTotal<T>(SqlCommand cmd, out int totalResultRecord, string totalColumnName = "phamkhanhhand_totalPaging")
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
        /// phamkhanhhand
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
                    //thupth  23.11.2024 fix lỗi phân biệt chữ hoa chữ thường
                    if ( string.Compare(pro.Name, column.ColumnName, StringComparison.OrdinalIgnoreCase)==0)
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
        /// phamkhanhhand
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
                    //thupth  23.11.2024 fix lỗi phân biệt chữ hoa chữ thường
                    if (string.Compare(pro.Name, column.ColumnName, StringComparison.OrdinalIgnoreCase) == 0)
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


    }
}