using System;

using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

using CT.DL;
using CT.Models.Entity;
using CT.Models.Enumeration;
using CT.Models.ServerObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; 
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data;
//using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Newtonsoft.Json;

namespace CT.DL
{
    public class BaseDL
    {

        protected DLUtil util = new DLUtil();
        #region Các hàm vẫn lấy dữ liệu

        /// <summary>
        /// Thêm, sửa, xóa
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="resultEntity"></param>
        /// <returns></returns>
        public bool SubmitEntity(BaseEntity entity, BaseEntity resultEntity)
        {
            if (entity.EntityState == EntityState.None)
            {
                throw new Exception();
            }
            else
            {
                DbCommand dbCommand = PreapareCommandBeforeSubmitEntity(entity);
                if (entity.EntityState == EntityState.Delete)
                {
                    dbCommand.ExecuteNonQuery();

                }
                else
                {
                    //  dbCommand.ExecuteNonQuery();
                    //reader và map vào resultEntity
                }
            }

            return true;
        }

        private DbCommand PreapareCommandBeforeSubmitEntity(BaseEntity entity)
        {
            //dựa vào entity state để làm
            DbCommand dbCommand = null;


            return dbCommand;


        }

        ///// <summary>
        ///// Trả về dataset
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //private DataSet ExcuteDataSet (BaseEntity entity)
        //{
        //    DbCommand dbCommand = null;


        //    return dbCommand;

        //}

        #endregion

        #region Các hàm base

        /// <summary>
        /// Lưu EntitySet (bộ master và detail)
        /// </summary>
        /// <param name="changeSet"></param>
        /// <param name="resultEntity"></param>
        /// <returns>Thông báo lỗi</returns>
        public string SaveEntitySet(EntitySet changeSet, BaseEntity resultMasterEntity = null)
        {
            EntityItem masterItem = changeSet.Master;
            List<EntityCollection> detailItems = changeSet.Details;

            //Tạo connect
            //Tạo transaction
            //Lưu master (gọi submitEntity; truyền masterItem.ObjectItem vào)

            //Nếu thành công
            //Duyệt cất detail (cũng gọi hàm submitEntity)

            return null;
        }

        /// <summary>
        /// Lấy entity theo ID truyền vào
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// phamha
        public List<T> SelectDetailByMasterID<T>(object id, string masterIDKey, string detailTableName)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.AddWithValue("@" + masterIDKey, id);


            if (id is Guid)
            {
                id += "\"" + id + "\"";
            }

            var sql = "select * from {0} where {1}=" + id;
            sql = string.Format(sql, detailTableName, masterIDKey);

            cmd.CommandText = sql;

            return util.SelectList<T>(cmd);

        }

        /// <summary>
        /// Lấy entity theo ID truyền vào
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// phamha
        public List<T> SelectList<T>(string tableName)
        {
            SqlCommand cmd = new SqlCommand();

            //cmd.Parameters.AddWithValue("@" + masterIDKey, id);


            var sql = "select top 100 * from {0} ";
            sql = string.Format(sql, tableName);

            cmd.CommandText = sql;

            return util.SelectList<T>(cmd);

        }
        public PagingData GetPagingData(string entityName, string sort, int limit, int start, string filterHeader, string columns, string customParam)
        {
            //build câu lệnh where ở đây FilterHeaderHelper

            //gọi đến ultil: tên, paramwhere, paramcustom => trả về list và count paging with count

            return null;
        }



        private string BuildWhereFilter(string filter)
        {
            var rs = "";


            if (!string.IsNullOrWhiteSpace(filter))
            {

                //Json to list object

                List<FilterSetting> filterSettings = JsonConvert.DeserializeObject<List<FilterSetting>>(filter);

                for (int i = 0; i < filterSettings.Count; i++)
                {
                    string currentFIlterString = "";

                    var filterST = filterSettings[i];
                    object filterValue = filterST.Value;


                    #region Convert lại giá trị

                    switch (filterST.DataType)
                    {
                        case DataType.Number:
                            break;
                        case DataType.Currency:
                            break;
                        case DataType.String:
                            break;
                        case DataType.Boolean:
                            break;
                        case DataType.DateTime:
                            break;
                        case DataType.Object:
                            break;
                        case DataType.Collection:
                            break;
                        case DataType.Enum:
                            break;
                        case DataType.Dictionary:
                            break;
                        case DataType.Json:
                            break;
                        case DataType.Null:
                            break;
                        case DataType.ByteArray:
                            break;
                        default:
                            break;
                    }

                    #endregion


                    #region Toán tử

                    switch (filterST.Operator)
                    {
                        case FilterOperator.Equal:
                            rs = string.Format(" and {0} = {1} ", filterST.Field, filterValue);
                            break;
                        case FilterOperator.NotEqual:
                            rs = string.Format(" and {0} <> {1} ", filterST.Field, filterValue);
                            break;
                        case FilterOperator.GreaterThan:
                            rs = string.Format(" and {0} > {1} ", filterST.Field, filterValue);
                            break;
                        case FilterOperator.LessThan:
                            rs = string.Format(" and {0} < {1} ", filterST.Field, filterValue);
                            break;
                        case FilterOperator.GreaterThanOrEqual:
                            rs = string.Format(" and {0} >= {1} ", filterST.Field, filterValue);
                            break;
                        case FilterOperator.LessThanOrEqual:
                            rs = string.Format(" and {0} <= {1} ", filterST.Field, filterValue);
                            break;
                        case FilterOperator.Contains:
                            rs = string.Format(" and {0} like {1} ", filterST.Field, filterValue);
                            break;
                        case FilterOperator.StartsWith:
                            break;
                        case FilterOperator.EndsWith:
                            break;
                        case FilterOperator.Custom:
                            rs = " " + filterValue;

                            break;
                        default:
                            break;
                    }

                    #endregion


                }


                //list object to string

            }

            return rs;
        }

        private string BuildWhereFilter(List<FilterSetting> filterSettings, SqlCommand cmd)
        {

            var rsAll = "";
            if (filterSettings != null)
            {


                //Json to list object

                for (int i = 0; i < filterSettings.Count; i++)
                {
                    string currentFIlterString = "";
                    var rs = "";


                    var filterST = filterSettings[i];
                    object filterValue = filterST.Value;
                    string paramName = "@" + filterST.Field;

                    #region Convert lại giá trị

                    switch (filterST.DataType)
                    {
                        case DataType.Number:
                            break;
                        case DataType.Currency:
                            break;
                        case DataType.String:
                            //filterValue = "N'" + filterST.Value + "'";
                            paramName = "N'@" + filterST.Field + "'";
                            break;
                        case DataType.Boolean:
                            break;
                        case DataType.DateTime:
                            break;
                        case DataType.Object:
                            break;
                        case DataType.Collection:
                            break;
                        case DataType.Enum:
                            break;
                        case DataType.Dictionary:
                            break;
                        case DataType.Json:
                            break;
                        case DataType.Null:
                            break;
                        case DataType.ByteArray:
                            break;
                        default:
                            //filterValue = "N'" + filterST.Value + "'";
                            break;
                    }

                    #endregion


                    #region Toán tử

                    switch (filterST.Operator)
                    {
                        case FilterOperator.Equal:
                            rs = string.Format(" and {0} = {1} ", filterST.Field, paramName);
                            break;
                        case FilterOperator.NotEqual:
                            rs = string.Format(" and {0} <>  {1} ", filterST.Field, paramName);
                            break;
                        case FilterOperator.GreaterThan:
                            rs = string.Format(" and {0} > {1} ", filterST.Field, paramName);
                            break;
                        case FilterOperator.LessThan:
                            rs = string.Format(" and {0} < {1} ", filterST.Field, paramName);

                            break;
                        case FilterOperator.GreaterThanOrEqual:
                            rs = string.Format(" and {0} >= {1} ", filterST.Field, paramName);
                            break;
                        case FilterOperator.LessThanOrEqual:
                            rs = string.Format(" and {0} <= {1} ", filterST.Field, paramName);
                            break;
                        case FilterOperator.Contains://string only

                            if (filterST.DataType == DataType.String)
                            {
                                rs = string.Format(" and [{0}] like N'%'+@{0}+'%'", filterST.Field);
                            }

                            break;
                        case FilterOperator.StartsWith:
                            break;
                        case FilterOperator.EndsWith:
                            break;
                        case FilterOperator.Custom://todo: bug injection
                            rs = " " + filterValue;

                            break;
                        default:
                            break;
                    }


                    cmd.Parameters.AddWithValue("@" + filterST.Field, filterValue);

                    #endregion

                    rsAll += " " + rs;

                }

            }
            //list object to string
            return rsAll;
        }


        /// <summary>
        /// Pagging có record
        /// Không nên dùng vì nếu vượt quá số trang => đều trả ra 0 hết & không lấy được tổng
        /// count(*) over() => cái này rất chậm. Thử 4 triệu dòng là biết
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="recordPerPage"></param>
        /// <param name="viewName">Tên bảng, hoặc tên view</param>
        /// <returns></returns>
        /// phamha Dec 28, 2024
        public List<T> SelectPagingNotStore<T>(string viewName, int from, int to, out int totalRecord, string sortBy = "", List<FilterSetting> filterSettings = null)
        {
            SqlCommand cmd = new SqlCommand();

            List<T> lstRs = new List<T>();
             

            sortBy = string.IsNullOrWhiteSpace(sortBy) ? "1" : sortBy;

            bool runningValid = true;

            //
            if (!ValidateFieldStructNameDB(sortBy))
            {
                runningValid = false;
            }


            if (runningValid)
            {
                //Điều kiện filter 
                var whereClause = " " + BuildWhereFilter(filterSettings, cmd);

                //Tên bảng cho ngoặc vuông vào, order by thì thêm asc cho nó xuống dòng

                var sql = @"
                    select  *
                    from [{0}] a
                    where 1=1 {1}
                    order by {2} 
                    asc
                    offset @from rows fetch next @to rows only
 
                 ";

                var sqlCount = @"

                    select   count(*)
                    from [{0}] a
                    where 1=1 {1} 
                ";


                sql = string.Format(sql, viewName, whereClause, sortBy);
                sqlCount = string.Format(sqlCount, viewName, whereClause);

                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to);

                cmd.CommandText = sql;

                lstRs = util.SelectList<T>(cmd);


                var totalRs = util.ExcuteSqlCommandScalar(cmd, sqlCount);


                totalRecord = int.Parse(totalRs.ToString());


            }
            else
            {
                totalRecord = 0;
            }


            return lstRs;
        }


        /// <summary>
        /// Pagging có record
        /// Không nên dùng vì nếu vượt quá số trang => đều trả ra 0 hết & không lấy được tổng
        /// count(*) over() => cái này rất chậm. Thử 4 triệu dòng là biết
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="recordPerPage"></param>
        /// <param name="viewName">Tên bảng, hoặc tên view</param>
        /// <returns></returns>
        /// phamha Dec 28, 2024
        public List<T> SelectPagingNotStoreBasicFilter<T>(string viewName, int from, int to, out int totalRecord, string sortBy = "", Dictionary<string, object> dctWhere = null)
        {
            SqlCommand cmd = new SqlCommand();

            List<T> lstRs = new List<T>();

            var totalColumnName = "pkha_totalPaging";

            sortBy = string.IsNullOrWhiteSpace(sortBy) ? "1" : sortBy;

            bool runningValid = true;

            //
            if (!ValidateFieldStructNameDB(sortBy))
            {
                runningValid = false;
            }


            if (runningValid)
            {
                var whereClause = " ";

                //Điều kiện filter

                if (dctWhere != null && dctWhere.Count > 0)
                {
                    for (int i = 0; i < dctWhere.Count; i++)
                    {
                        var itemDict = dctWhere.ElementAt(i);
                        var key = itemDict.Key;
                        var value = itemDict.Value;

                        cmd.Parameters.AddWithValue("@" + key, value);

                        whereClause += string.Format(" and [{0}] like N'%'+@{1}+'%'", key, key);

                    }

                }


                //Tên bảng cho ngoặc vuông vào, order by thì thêm asc cho nó xuống dòng

                var sql = @"
                    select  *
                    from [{0}] a
                    where 1=1 {1}
                    order by {2} 
                    asc
                    offset @from rows fetch next @to rows only
 
                 ";

                var sqlCount = @"

                    select   count(*)
                    from [{0}] a
                    where 1=1 {1} 
                ";


                sql = string.Format(sql, viewName, whereClause, sortBy);
                sqlCount = string.Format(sqlCount, viewName, whereClause);

                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to);

                cmd.CommandText = sql;

                lstRs = util.SelectList<T>(cmd);


                var totalRs = util.ExcuteSqlCommandScalar(cmd, sqlCount);


                totalRecord = int.Parse(totalRs.ToString());


            }
            else
            {
                totalRecord = 0;
            }


            return lstRs;
        }


        /// <summary>
        /// Pagging có record
        /// Không nên dùng vì nếu vượt quá số trang => đều trả ra 0 hết & không lấy được tổng
        /// count(*) over() => cái này rất chậm. Thử 4 triệu dòng là biết
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="recordPerPage"></param>
        /// <param name="viewName">Tên bảng, hoặc tên view</param>
        /// <returns></returns>
        /// pkha 05.07.2023
        public List<T> SelectPagingNotStore_SmallData<T>(string viewName, int from, int to, out int totalRecord, string sortBy = "", Dictionary<string, object> dctWhere = null)
        {
            SqlCommand cmd = new SqlCommand();

            List<T> lstRs = new List<T>();

            var totalColumnName = "pkha_totalPaging";

            sortBy = string.IsNullOrWhiteSpace(sortBy) ? "1" : sortBy;

            bool runningValid = true;

            //
            if (!ValidateFieldStructNameDB(sortBy))
            {
                runningValid = false;
            }


            if (runningValid)
            {
                var whereClause = " ";

                //Điều kiện filter

                if (dctWhere != null && dctWhere.Count > 0)
                {
                    for (int i = 0; i < dctWhere.Count; i++)
                    {
                        var itemDict = dctWhere.ElementAt(i);
                        var key = itemDict.Key;
                        var value = itemDict.Value;

                        cmd.Parameters.AddWithValue("@" + key, value);

                        whereClause += string.Format(" and [{0}] like N'%'+@{1}+'%'", key, key);

                    }

                }




                //Tên bảng cho ngoặc vuông vào, order by thì thêm asc cho nó xuống dòng

                var sql = @"

                    select count(*) over() as {0},
                           *
                    from [{1}] a
                    where 1=1 {2}
                    order by {3} 
                    asc
                    offset @from rows fetch next @to rows only
 
";


                sql = string.Format(sql, totalColumnName, viewName, whereClause, sortBy);

                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to);

                cmd.CommandText = sql;

                lstRs = util.SelectListWithTotal<T>(cmd, out totalRecord, totalColumnName: totalColumnName);



            }
            else
            {
                totalRecord = 0;
            }


            return lstRs;
        }


        private bool ValidateFieldStructNameDB(string name)
        {
            var rs = true;

            if (string.IsNullOrWhiteSpace(name))
            {
                rs = false;
            }
            else
            {
                name = name.Trim();

                if (name.Contains(" ")
                    || name.Contains("--")
                    || name.Contains("'")
                    )
                {
                    rs = false;
                }
            }



            return rs;
        }

        #region GetData


        /// <summary>
        /// Paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectName"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="sumRecord">Outupt</param>
        /// <param name="sumCurrentRecord"></param>
        /// <param name="columnKey"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        /// pkha 15.04.2023
        public List<T> SelectPaging<T>
            (
            string objectName,
            out int sumRecord,
            out int sumCurrentRecord,
            int skip = 0,
            int take = 100,
            string column = "*",
            string columnKey = "",
            string orderBy = "",
            string where = "",
            string customParam = ""
            )
        {

            var sql = "[dbo].[Proc_SelectPaging]";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@take", take);
            cmd.Parameters.AddWithValue("@skip", skip);
            cmd.Parameters.AddWithValue("@where", where);
            cmd.Parameters.AddWithValue("@customParam", customParam);
            cmd.Parameters.AddWithValue("@sort", orderBy);
            cmd.Parameters.AddWithValue("@column", column);

            cmd.Parameters.AddWithValue("@objectName", objectName);
            cmd.Parameters.AddWithValue("@columnKey", columnKey);

            SqlParameter totalCount = new SqlParameter();
            totalCount.ParameterName = "@totalCount";
            totalCount.DbType = DbType.Int32;
            totalCount.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(totalCount);

            var lst = util.SelectList<T>(cmd);

            sumRecord = totalCount != null ? (int)totalCount.Value : 0;
            sumCurrentRecord = (lst != null) ? lst.Count : 0;

            return lst;
        }

        /// <summary>
        /// Lấy data theo ID
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        /// pkha 17.04.2023
        public List<object> GetDetailsByMasterID(Type masterType, Type type, object id, int top = 1000)
        {
            //tạo store rồi get bình thường ; gọi storre chung, truyền querytype

            var tableName = type.Name;
            var masterIDKey = masterType.Name;

            SqlCommand cmd = new SqlCommand();

            //cmd.Parameters.AddWithValue("@" + masterIDKey, id);
            cmd.Parameters.AddWithValue("@id", id);


            var sql = @"select top {0} * from {1} 
                    where {2}ID = @id
";



            sql = string.Format(sql, top, tableName, masterIDKey);

            cmd.CommandText = sql;

            return util.SelectList(type, cmd);

        }

        public List<T> GetDetailsByColumn<T>(string columnName, object id, int top = 1000) where T:BaseEntity
        {
            Type type = typeof(T);
            //tạo store rồi get bình thường ; gọi storre chung, truyền querytype

            var tableName = type.Name; 

            SqlCommand cmd = new SqlCommand();

            //cmd.Parameters.AddWithValue("@" + masterIDKey, id);
            cmd.Parameters.AddWithValue("@id", id);


            var sql = @"select top {0} * from {1} 
                    where {2}  = @id
";
             
            sql = string.Format(sql, top, tableName, columnName);

            cmd.CommandText = sql;

            return util.SelectList<T>( cmd);

        }

        /// <summary>
        /// Lấy data theo ID
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        /// pkha 17.04.2023
        public object GetDataByID(Type type, object id)
        {
            //tạo store rồi get bình thường ; gọi storre chung, truyền querytype

            //            var tableName = type.Name;

            //            SqlCommand cmd = new SqlCommand();

            //            //cmd.Parameters.AddWithValue("@" + masterIDKey, id);


            //            var sql = @"select top 1 * from {0} 
            //                    where {0}ID = {1}
            //";
            //            sql = string.Format(sql, tableName, id);

            //            cmd.CommandText = sql;

            return util.GetByID(type, id);

        }



        /// <summary>
        /// Get data by columnvalue
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        /// phamha Nov 23, 2024
        public T GetDataByColumn<T>(string columnName, object columnValue)
        { 

            return util.GetByColumn<T>(columnName, columnValue);

        }

        /// <summary>
        /// Get data by columnvalue
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        /// phamha Nov 23, 2024
        public List<T> GetListDataByColumn<T>(string columnName, object columnValue)
        {

            return util.GetListByColumn<T>(columnName, columnValue);

        }


        public BaseEntity GetDataByID(string entityName, object id)
        {
            //tạo store rồi get bình thường ; gọi storre chung, truyền querytype
            return null;
        }


        ////viết làm vài hàm vào, theo entity, theo type, theo ....
        //public List<object> GetDetailsByMasterID(Type type, object id)
        //{
        //    //tạo store rồi get bình thường ; gọi storre chung, truyền querytype
        //    return null;
        //}

        //viết làm vài hàm vào, theo entity, theo type, theo ....
        public List<T> GetDetailsByMasterID<T>(string entityName, object id)
        {
            //tạo store rồi get bình thường ; gọi storre chung, truyền querytype
            return null;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns>Lỗi (nếu có)</returns>
        public string DeleteEntity(string entityName, object id)
        {
            //tạo store, excutenonequery
            return null;
        }


        #endregion

        #region Các tiện ích
        public string GetStoreName(string entityName, QueryType queryType)
        {
            //case các trường hợp rồi string format
            return null;
        }

        //public string BuildCustomWhereParam(Dictionary...)//từ dic chuyển sang param thôi
        //{
        //    //case các trường hợp rồi string format
        //    return null;
        //}

        //hàm exec sql script nữa



        #endregion


        #region InsertUpdate




        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityName"></param>
        /// Đã test
        public void Delete<T>(object id) where T : BaseEntity
        { //lấy ra tất cả cái map để insert vào


            var entityType = typeof(T);

            var entityName = entityType.Name;



            EntityKey key = (EntityKey)Attribute.GetCustomAttribute(entityType, typeof(EntityKey));

            SqlCommand cmd = new SqlCommand(); 

            string keyName = key == null ? entityName + "ID" : key.KeyName;

             

            var sql = @" delete from {0} where {1} = {2}  ";
            sql = string.Format(sql, entityName, keyName, id);

            util.ExcuteSqlCommand(cmd, sql);
        }




        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityName"></param>
        /// Đã test
        public object Insert(object entity, string entityName)
        { //lấy ra tất cả cái map để insert vào


            var entityType = entity.GetType();
            var properties = entity.GetType().GetProperties()
                ;//.Where(prop => prop.IsDefined(typeof(PropertyEntity), true)).ToList();


            var lstColumnInDatabase = util.GetColumnsName(entityName);


            EntityKey key = (EntityKey)Attribute.GetCustomAttribute(entityType, typeof(EntityKey));

            SqlCommand cmd = new SqlCommand();
            string lstValue = "";

            string keyName = key == null ? entityName + "ID" : key.KeyName;


            string lstValueTitle = "";



            for (int i = 0; i < properties.Count(); i++)
            {
                var p = properties[i];

                var name = p.Name;

                if (!string.IsNullOrWhiteSpace(name)
                   && !string.Equals(keyName, name, StringComparison.OrdinalIgnoreCase)
                   && lstColumnInDatabase.Contains(name, StringComparer.OrdinalIgnoreCase)
                   )
                {

                    var propertyInfo = entity.GetType().GetProperty(name);
                    var value = propertyInfo.GetValue(entity, null);


                    if (value == null)
                    {
                        value = DBNull.Value;
                    }
                    cmd.Parameters.AddWithValue("@" + name, value);
                    lstValueTitle += ", " + name;
                    lstValue += ", @" + name;

                }
            }


            if (lstValue.StartsWith(","))
            {
                lstValueTitle = lstValueTitle.Substring(1);
                lstValue = lstValue.Substring(1);
            }


            var sql = @"insert into {0}({1})
                    values({2})

                SELECT SCOPE_IDENTITY()
        ";
            sql = string.Format(sql, entityName, lstValueTitle, lstValue);

            return util.ExcuteSqlCommandScalar(cmd, sql);
        }


         
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityName"></param>
        /// Đã test
        public object Insert(object entity, Type entityType)
        { //lấy ra tất cả cái map để insert vào
             
            var properties = entity.GetType().GetProperties()
                ;//.Where(prop => prop.IsDefined(typeof(PropertyEntity), true)).ToList();



            EntityKey key = (EntityKey)Attribute.GetCustomAttribute(entityType, typeof(EntityKey));
            DatabaseViewName tableViewName = (DatabaseViewName)Attribute.GetCustomAttribute(entityType, typeof(DatabaseViewName));

            SqlCommand cmd = new SqlCommand();
            string lstValue = "";


            var tableName = entityType.Name;

            if (tableViewName != null && !string.IsNullOrEmpty(tableViewName.ViewName))
            {
                tableName = tableViewName.ViewName;
            }


            var lstColumnInDatabase = util.GetColumnsName(tableName);

            string keyName = key == null ? tableName + "ID" : key.KeyName;
            string seqenceName = key == null ? string.Empty : key.SequenceName;


            string lstValueTitle = ""; 


             
            for (int i = 0; i < properties.Count(); i++)
            {
                var p = properties[i];

                var name = p.Name;
                 
                if (!string.IsNullOrWhiteSpace(name)
                   && !string.Equals(keyName, name, StringComparison.OrdinalIgnoreCase)
                   && lstColumnInDatabase.Contains(name, StringComparer.OrdinalIgnoreCase)
                   )
                {

                    var propertyInfo = entity.GetType().GetProperty(name);
                    var value = propertyInfo.GetValue(entity, null);
                     

                    if (value == null)
                    {
                        value = DBNull.Value; 
                    }
                    cmd.Parameters.AddWithValue("@" + name, value);
                    lstValueTitle += ", " + name;
                    lstValue += ", @" + name;

                }
            }




            var sql = @"insert into {0}({1})
                    values({2})

                SELECT SCOPE_IDENTITY()
        ";


            if (!string.IsNullOrWhiteSpace(seqenceName))
            {
                sql = @"

declare @nextID int = (NEXT VALUE FOR ["+ seqenceName + @"])
insert into {0}( " + keyName + @",{1})
                    values( @nextID,{2})

                SELECT @nextID
        ";


            }


            if (lstValue.StartsWith(","))
            {
                lstValueTitle = lstValueTitle.Substring(1);
                lstValue = lstValue.Substring(1);
            }

            sql = string.Format(sql, tableName, lstValueTitle, lstValue);

            return  util.ExcuteSqlCommandScalar(cmd, sql);
        }




        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityName"></param>
        /// pkhaok
        public void Update(object entity, string entityName)
        { //lấy ra tất cả cái map để insert vào

            var entityType = entity.GetType();

            var properties = entityType.GetProperties();//.Where(prop => prop.IsDefined(typeof(PropertyEntity), true)).ToList();

            var lstColumnInDatabase = util.GetColumnsName(entityName);


            EntityKey key = (EntityKey)Attribute.GetCustomAttribute(entityType, typeof(EntityKey));

            SqlCommand cmd = new SqlCommand();
            string lstValue = "";

            string keyName = key == null ? entityName + "ID" : key.KeyName;

            //Key để cập nhật
            string lstKey = keyName + "=@" + keyName;

            for (int i = 0; i < properties.Count(); i++)
            {
                var p = properties[i];

                var name = p.Name;

                //var propertyInfo = entityType.GetProperty(name);
                //cập nhật mọi thứ, không cập nhật key
                //Cột phải tồn tại trong bảng thì mới lấy
                if (!string.IsNullOrWhiteSpace(name)
                    && !string.Equals(keyName, name, StringComparison.OrdinalIgnoreCase)
                    && lstColumnInDatabase.Contains(name, StringComparer.OrdinalIgnoreCase)
                    )
                {
                    var value = p.GetValue(entity, null);

                    if (value == null)
                    {
                        value = DBNull.Value;
                    }

                    cmd.Parameters.AddWithValue("@" + name, value);
                    lstValue += ", " + name + " = @" + name;
                }
            }

            if (lstValue.StartsWith(","))
            {
                lstValue = lstValue.Substring(1);
            }

            //Nếu chưa truyền paramID vào thì 
            if (!cmd.Parameters.Contains(keyName))
            {
                var valueKey = entityType.GetProperty(keyName).GetValue(entity, null);

                cmd.Parameters.AddWithValue("@" + keyName, valueKey);
            }


            #region Check version
            var editVersion = ((BaseEntity)entity).EditVersion;

            if (editVersion != null)
            {


                var sqlVersion = @"
            select editversion
from {0}
where {1}
";


                sqlVersion = string.Format(sqlVersion, entityName, lstKey);

                cmd.CommandText = sqlVersion;
                byte[] currentTimestamp = (byte[])cmd.ExecuteScalar(); // Lấy giá trị TimestampColumn hiện tại

                // So sánh timestamp để kiểm tra sự thay đổi
                if (!currentTimestamp.SequenceEqual(editVersion))
                {
                    throw new InvalidOperationException("Dữ liệu đã thay đổi. Vui lòng tải lại và thử lại.");
                }
            }

            #endregion


            var sql = @"update {0}
                        set {1}
                        where {2}
                    ";
            sql = string.Format(sql, entityName, lstValue, lstKey);

            util.ExcuteSqlCommand(cmd, sql);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityName"></param>
        /// pkhaok
        public void Update(object entity, Type entityType, bool isCheckEditVersion = true)
        { //lấy ra tất cả cái map để insert vào
             

            var properties = entityType.GetProperties();//.Where(prop => prop.IsDefined(typeof(PropertyEntity), true)).ToList();


            EntityKey key = (EntityKey)Attribute.GetCustomAttribute(entityType, typeof(EntityKey)); 
            DatabaseViewName tableViewName = (DatabaseViewName)Attribute.GetCustomAttribute(entityType, typeof(DatabaseViewName));


            var tableName = entityType.Name;

            if (tableViewName != null && !string.IsNullOrEmpty(tableViewName.ViewName))
            {
                tableName = tableViewName.ViewName;
            }



            var lstColumnInDatabase = util.GetColumnsName(tableName);



            SqlCommand cmd = new SqlCommand();
            string lstValue = "";

            string keyName = key == null ? tableName + "ID" : key.KeyName;

            //Key để cập nhật
            string lstKey = keyName + "=@" + keyName;

            for (int i = 0; i < properties.Count(); i++)
            {
                var p = properties[i];

                var name = p.Name;

                //var propertyInfo = entityType.GetProperty(name);
                //cập nhật mọi thứ, không cập nhật key
                //Cột phải tồn tại trong bảng thì mới lấy
                if (!string.IsNullOrWhiteSpace(name) 
                    && !string.Equals(keyName, name, StringComparison.OrdinalIgnoreCase)
                    && lstColumnInDatabase.Contains(name, StringComparer.OrdinalIgnoreCase)
                    )
                {
                    var value = p.GetValue(entity, null);

                    if (value == null)
                    {
                        value = DBNull.Value;
                    }

                    cmd.Parameters.AddWithValue("@" + name, value);

                    if (!string.Equals("EditVersion", name, StringComparison.OrdinalIgnoreCase))
                    {
                        lstValue += ", " + name + " = @" + name;
                    }

                }
            }

            if (lstValue.StartsWith(","))
            {
                lstValue = lstValue.Substring(1);
            }

            //Nếu chưa truyền paramID vào thì 
            if (!cmd.Parameters.Contains(keyName))
            {
                var valueKey = entityType.GetProperty(keyName).GetValue(entity, null);

                cmd.Parameters.AddWithValue("@" + keyName, valueKey);
            }


            #region Check version

            if (isCheckEditVersion && lstColumnInDatabase.Contains("EditVersion"))
            {

                var editVersion = ((BaseEntity)entity).EditVersion;

                if (editVersion != null)
                {
                    var sqlVersion = @"
            select editversion
from {0}
where {1}
";

                    sqlVersion = string.Format(sqlVersion, tableName, lstKey);

                    cmd.CommandText = sqlVersion;
                    //byte[] currentTimestamp = (byte[])cmd.ExecuteScalar(); // Lấy giá trị TimestampColumn hiện tại
                    byte[] currentTimestamp = (byte[])util.ExcuteSqlCommandScalar(cmd, sqlVersion);

                    // So sánh timestamp để kiểm tra sự thay đổi
                    if (currentTimestamp != null && !currentTimestamp.SequenceEqual(editVersion))
                    {
                        throw new InvalidOperationException("Dữ liệu đã thay đổi. Vui lòng tải lại và thử lại.");
                    }
                }
            }

            #endregion


            var sql = @"update {0}
                        set {1}
                        where {2}
                    ";
            sql = string.Format(sql, tableName, lstValue, lstKey);

            util.ExcuteSqlCommand(cmd, sql);
        }

        #region Chờ bỏ
         
        //public void Insert<T>(T entity) where T : BaseEntity
        //{
        //    //lấy ra tất cả cái map để insert vào


        //    var properties = typeof(T).GetProperties();//.Where(prop => prop.IsDefined(typeof(PropertyEntity), true)).ToList();


        //    SqlCommand cmd = new SqlCommand();
        //    string lstValue = "";

        //    for (int i = 0; i < properties.Count(); i++)
        //    {
        //        var p = properties[i];

        //        var name = p.Name;

        //        var propertyInfo = entity.GetType().GetProperty(name);
        //        var value = propertyInfo.GetValue(entity, null);

        //        cmd.Parameters.AddWithValue("@" + name, value);
        //        lstValue += ", @" + name;
        //    }


        //    if (lstValue.StartsWith(","))
        //    {
        //        lstValue = lstValue.Substring(1);
        //    }


        //    var sql = @"insert into {0}({1}
        //            values({1})
        //";
        //    sql = string.Format(sql, typeof(T).Name, lstValue);

        //    util.ExcuteSqlCommand(cmd, sql);
        //}

        #endregion



        #endregion

    }
}
