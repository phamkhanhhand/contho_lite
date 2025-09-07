//using CT.Common;
using CT.DL;
using CT.Models.Entity;
using CT.Models.Enumeration;
using CT.Models.ServerObject;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CT.BL
{
    public class BaseBL
    {
        #region thêm sửa xóa

        protected virtual Type EntityType { get; set; }

        protected virtual List<Type> GetDetailBusinessType()
        {
            return null;
        }

        //GêtntitySetFromChangsetString: changeset=>proxy=>entityset

        //ValidateMaster, detail: chiều dài, giá trị min, max => cho vào errorlist của EntityItem của master, detail

        //PrepareEntitySet: tạo id cho master (nếu update); gán id master của master vào detail

        //GetPagingData, GetDataByID, : gọi dl

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>id của bản xóa (nếu lỗi), và tin nhắn lỗi</returns>
        public Dictionary<string, string> DeleteEntities(string ids)
        {
            var enttityType = this.EntityType;

            //chuyển ids thành ilist, for từng id; tạo entityset, lấy master rồi gọi dl xóa bản ghi đi

            return null;
        }

         

        public ServiceData SaveEntitySet(string changeSet)
        {
            ServiceData res = new ServiceData();

            //Map đối tượng vào
            EntitySet entitySet = GetEntitySetFromChangeSetString(changeSet);

            //getorigin
            res = this.DosubmitEntitySet(entitySet);
            return res;
        }

        /// <summary>
        /// Convert detail ra
        /// </summary>
        /// <param name="changeSet"></param>
        /// <returns></returns>
        /// phamkhanhhand 24.11.2022
        private EntitySet GetEntitySetFromChangeSetString(string changeSet)
        {
            EntitySet entitySet = new EntitySet();

            EntitySetProxy entitySetProxy = JsonConvert.DeserializeObject<EntitySetProxy>(changeSet);
            var entityNameSpace = Models.Enumeration.NameSpace.NameSpaceEntity.GetStringValue();
            var entityAsembly = Models.Enumeration.NameSpace.AssemblyEntity.GetStringValue();

            //Lấy master
            string masterName = entityNameSpace + "." + entitySetProxy.Master.EntityName;


            var masterType = Type.GetType(masterName + "," + entityAsembly);

            //Là object nhưng đã được convert ra entity rồi (debug để thấy nó là entity)
            var masterObj = JsonConvert.DeserializeObject(entitySetProxy.Master.EntityObject, masterType);

            entitySet.Master = new EntityItem()
            {
                EntityName = entitySetProxy.Master.EntityName,
                EntityObject = masterObj,
            };

            //lấy detail


            var detailProxy = entitySetProxy.Details;
            if (detailProxy != null && detailProxy.Count > 0)
            {
                List<EntityCollection> entityCollections = new List<EntityCollection>();

                for (int i = 0; i < detailProxy.Count; i++)
                {

                    EntityCollection collection = new EntityCollection();

                    var dtItem = detailProxy[i];

                    string dtName = entityNameSpace + "." + dtItem.EntityName;
                    var dtType = Type.GetType(dtName + "," + entityAsembly);

                    //Convert ra Entity nhưng vẫn nằm trong object

                    var lstDt = (List<BaseEntity>)JsonConvert.DeserializeObject(dtItem.Items, dtType);

                    collection.EntityName = dtItem.EntityName;

                    //phải convert ngược trở lại object thì mới gán vào được
                    var lstCV = lstDt.Cast<object>().ToList();
                    collection.Items = lstCV;

                    entityCollections.Add(collection);
                }

                entitySet.Details = entityCollections;

            }


            return entitySet;
        }

        private ServiceData DosubmitEntitySet(EntitySet entitySet)
        {

            //validate entitymaster, detail

            var validset = ValidateEntitySet(entitySet);
            //prepare: gán id master vào con

            if (!validset)
            {
                //TODO xử lý cái này
                throw new Exception("Lỗi để trống trường");
            }


            //string err=gọi dl saveEntitySet, ref entity ra ngoài để kiểm tra lỗi gì, kể cả lỗi thay đổi bởi người sd khác

            //Bắt đầu lưu data
            var dl = new BaseDL();
            if (entitySet.Master.EntityObject != null)
            {
                //Lưu master
                var master = (BaseEntity)entitySet.Master.EntityObject;
                //Kiểm tra xem EntityState của nó là gì
                switch (master.EntityState)
                {
                    case Models.Enumeration.EntityState.Add:

                        dl.Insert(master, entitySet.Master.EntityName);
                        break;

                    case Models.Enumeration.EntityState.Update:

                        dl.Update(master, entitySet.Master.EntityName);
                        break;

                    default:
                        break;
                }
            }


            //SaveToDb(entitySet.Master.EntityObject);

            //Nếu không có lỗi gì thì lưu tiếp detail

            for (int i = 0; i < entitySet.Details.Count; i++)
            {
                var dt = entitySet.Details[i];

                //SaveListToDB(dt.Items);

            }

            return null;
        }

        public void SaveListToDB<T>(IList items) where T : BaseEntity
        {
            for (int i = 0; i < items.Count; i++)
            {
                SaveToDb<T>(items[i]);
            }
        }



        private bool ValidateEntitySet(EntitySet entitySet)
        {
            bool valid = true;
            //Check master xem trường nào require

            if (entitySet != null && entitySet.Master != null && entitySet.Master.EntityObject != null)
            {
                //Lấy những trường require
                var entity = entitySet.Master.EntityObject;

                var requireField = entity.GetType().GetProperties()
                    .Where(prop => prop.IsDefined(typeof(Require), true)).ToList();



                for (int i = 0; i < requireField.Count(); i++)
                {
                    var p = requireField[i];

                    var name = p.Name;

                    var propertyInfo = entity.GetType().GetProperty(name);
                    var value = propertyInfo.GetValue(entity, null);

                    if (value == null || string.IsNullOrEmpty(value.ToString()))
                    {
                        //TODO nếu có cho err vào chính entityset này
                        valid = false;
                    }
                }

            }

            return valid;
        }


        #endregion


        #region Select

        public List<T> GetListByType<T>()
        {
            MethodInfo method1 = typeof(BaseDL).GetMethod(nameof(BaseDL.SelectList));
            MethodInfo generic1 = method1.MakeGenericMethod(this.EntityType);
            List<T> lstdetail = null;

            //todo
            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(this.EntityType);


            if (dl != null)
            {


                object[] parametersArray = new object[] { this.EntityType.Name };

                lstdetail = ((List<T>)generic1.Invoke(dl, parametersArray));

                EntityCollection collection = new EntityCollection();
            }

            return lstdetail;

        }

        private Dictionary<string, object> JsonToDict(string json)
        {
            try
            {

                return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            catch
            {
                return null;
            }
        }
         


        /// <summary>
        /// Lấy pagging không dùng store
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="recordPerPage"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        /// phamkhanhhand 05.07.2023
        public PagingData GetPagingByBase(int pageNumber = 1, int recordPerPage = 20, string viewName = "", string filter = "")
        {

            Dictionary<string, object> dctWhere = JsonToDict(filter);

            var from = (pageNumber - 1) * recordPerPage + 1;
            var to = from + recordPerPage - 1;
            int totalRecord = 0;

            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = this.EntityType.Name;

                //rà trong attribute xem có viewName không
                System.Reflection.MemberInfo info = this.EntityType;
                object[] attributes = info.GetCustomAttributes(true);

                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i] is DatabaseViewName)
                    {
                        viewName = ((DatabaseViewName)attributes[i]).ViewName;
                        break;
                    }

                }
            }

            PagingData pg = new PagingData();

            var currentType = this.EntityType;
            MethodInfo method1 = typeof(BaseDL).GetMethod(nameof(BaseDL.SelectPagingNotStoreBasicFilter));
            MethodInfo generic1 = method1.MakeGenericMethod(this.EntityType);

            //todo
            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(this.EntityType);


            object[] parametersArray = new object[] {

                viewName,
                from,
                to,
                null,//totalrecord , 
                null,//sortby,

            };
            var lstdetail = ((IList)generic1.Invoke(dl, parametersArray));

            int sumRecord;
            //output
            sumRecord = (int)parametersArray[3];

            EntityCollection collection = new EntityCollection();

            pg.ListObject = lstdetail;
            pg.CurrentCount = lstdetail.Count;
            pg.TotalRecord = sumRecord;

            return pg;

        }


        public IList CreateList(Type myType)
        {
            Type genericListType = typeof(List<>).MakeGenericType(myType);
            return (IList)Activator.CreateInstance(genericListType);
        }

        /// <summary>
        /// GetPaging
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="recordPerPage"></param>
        /// <param name="column"></param>
        /// <param name="columnKey"></param>
        /// <param name="orderBy"></param>
        /// <param name="where"></param>
        /// <param name="customParam"></param>
        /// <returns></returns>
        /// phamkhanhhand 17.04.2023
        public PagingData GetPaging(
                int pageNum = 0,
            int recordPerPage = 100,
            string column = "*",
            string columnKey = "",
            string orderBy = "",
            string where = "",
            string customParam = ""

            )
        {


            //CurrentContext.GetProfile();


            int skip = (recordPerPage - 1) * recordPerPage;

            PagingData pg = new PagingData();

            var currentType = this.EntityType;
            MethodInfo method1 = typeof(BaseDL).GetMethod(nameof(BaseDL.SelectPaging));
            MethodInfo generic1 = method1.MakeGenericMethod(this.EntityType);

            //todo
            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(this.EntityType);


            object[] parametersArray = new object[] {

                this.EntityType.Name,
                null,//  sumRecord, 
                null,//sumCurrentRecord,
                  skip = 0,
              recordPerPage = 100,
             column = "*",
             columnKey = "",
             orderBy = "",
             where = "",
             customParam=""


            };
            var lstdetail = (generic1.Invoke(dl, parametersArray));

            int sumRecord;
            int sumCurrentRecord;
            //output
            sumRecord = (int)parametersArray[1];
            sumCurrentRecord = (int)parametersArray[2];

            EntityCollection collection = new EntityCollection();

            pg.ListObject = lstdetail;
            pg.CurrentCount = sumRecord;//todo: out từ db

            return pg;

        }

        public PagingData GetPaging(
            string columnMaster,
            object masterID,
        int pageNum = 0,
        int recordPerPage = 100,
        string column = "*",
        string columnKey = "",
        string orderBy = "",
        string where = "",
        string customParam = ""

        )
        {

            int skip = (recordPerPage - 1) * recordPerPage;

            PagingData pg = new PagingData();

            var currentType = this.EntityType;
            MethodInfo method1 = typeof(BaseDL).GetMethod(nameof(BaseDL.SelectPaging));
            MethodInfo generic1 = method1.MakeGenericMethod(this.EntityType);

            //todo
            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(this.EntityType);


            object[] parametersArray = new object[] {

                this.EntityType.Name,
                null,//  sumRecord, 
                null,//sumCurrentRecord,
                  skip = 0,
              recordPerPage = 100,
             column = "*",
             columnKey = "",
             orderBy = "",
             where = "",
             customParam= string.Format("{0} = '{1}'", columnMaster, masterID)


            };
            var lstdetail = (generic1.Invoke(dl, parametersArray));

            int sumRecord;
            int sumCurrentRecord;
            //output
            sumRecord = (int)parametersArray[1];
            sumCurrentRecord = (int)parametersArray[2];

            EntityCollection collection = new EntityCollection();

            pg.ListObject = lstdetail;
            pg.CurrentCount = sumRecord;//todo: out từ db

            return pg;

        }


        #endregion



        #region Đã nhặt lại 2024



        //Lưu thẳng đối tượng xuống database
        //phamkhanhhand ok
        public bool SaveToDb<T>(object entity)
        {
            return this.SaveToDb(entity, typeof(T));
        }

        //Lưu thẳng đối tượng xuống database
        //phamkhanhhand ok
        private bool SaveToDb(object entity, Type type)
        {
            var entityConvert = ((CT.Models.Entity.BaseEntity)entity);

            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(type);

            var entityName = GetViewNameByType(type);

            if (string.IsNullOrWhiteSpace(entityName))
            {
                entityName = type.Name;
            }


            //Kiểm tra entityState
            switch (entityConvert.EntityState)
            {
                case EntityState.Add:
                    //Gọi dl lưu


                    dl.Insert(entityConvert, entityName);


                    break;
                case EntityState.Update:

                    dl.Update(entityConvert, entityName);

                    break;
                default:
                    break;
            }

            return true;

        }


        public object Insert<T>(object entity) where T : BaseEntity
        {
            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(typeof(T));

            return dl.Insert(entity, entity.GetType()); 
        }

        public bool Update<T>(object entity) where T : BaseEntity
        {
            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(typeof(T)); 

            dl.Update(entity, entity.GetType());

            return true;

        }

        public bool DeleteByID<T>(object id) where T : BaseEntity
        { 
            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(typeof(T));
             

            dl.Delete<T>(id);


            return true;

        }


        public T GetDataByID<T>(object id) where T:BaseEntity
        {
            var currentType = typeof(T);
            var dl = DLFactory.CreateDLByType(currentType);

            //lấy master theo id
           var rs = dl.GetDataByID(currentType, id); 

            if(rs == null)
            {
                return null;
            }
            else
            {
                return (T)rs;
            }
        }


        /// <summary>
        /// Get data by columnvalue
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        /// phamkhanhhand Nov 23, 2024
        public T GetDataByColumn<T>(string columnName, object columnValue)  where T : BaseEntity
        {
            var currentType = typeof(T);
            var dl = DLFactory.CreateDLByType(currentType);

            //lấy master theo id
            var rs = dl.GetDataByColumn<T>(columnName, columnValue);

            if (rs == null)
            {
                return null;
            }
            else
            {
                return (T)rs;
            }
        }

        /// <summary>
        /// Get data by columnvalue
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        /// phamkhanhhand Nov 23, 2024
        public List<T> GetListDataByColumn<T>(string columnName, object columnValue) where T : BaseEntity
        {
            var currentType = typeof(T);
            var dl = DLFactory.CreateDLByType(currentType);

            //lấy master theo id
            var rs = dl.GetListDataByColumn<T>(columnName, columnValue);

            if (rs == null)
            {
                return null;
            }
            else
            {
                return (List<T>)rs;
            }
        }


        public EntitySet GetDetailFormData(object id)
        {
            return GetDetailFormData(id, this.EntityType);
        }


        public EntitySet GetDetailFormData<T>(object id)
        {
            var currentType = typeof(T);
            return GetDetailFormData(id, currentType);
        }


        /// <summary>
        /// Lấy detai: bao gồm thông tin chi tiết master và list các bảng con
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>id của bản xóa (nếu lỗi), và tin nhắn lỗi</returns>
        /// phamkhanhhand 17.04.2023
        public EntitySet GetDetailFormData(object id, Type currentType)
        {
            //CurrentContext.GetProfile();

            //var currentType = typeof(T);

            var dl = DLFactory.CreateDLByType(currentType);

            //lấy master theo id
            var master = dl.GetDataByID(currentType, id);

            //lấy detail theo master id
            var lstDetailType = this.GetDetailBusinessType();

            var lstCollectionDetail = new List<EntityCollection>();


            if (lstDetailType != null)
            {

                for (int i = 0; i < lstDetailType.Count; i++)
                {
                    var detailType = lstDetailType[i];

                    //var detailDL = DLFactory.CreateDLByType(detailType);

                    //lấy master theo id
                    var detail = dl.GetDetailsByMasterID(currentType, detailType, id);

                    var detailCollection = new EntityCollection()
                    {
                        EntityName = detailType.Name,
                        Items = detail,
                    };
                    lstCollectionDetail.Add(detailCollection);


                }

            }

            EntitySet entitySet = new EntitySet()
            {
                Master = new EntityItem()
                {
                    EntityObject = master,
                    EntityName = currentType.Name,
                     
                },
                Details = lstCollectionDetail,
            };

            return entitySet;
        }


        public PagingData GetPagingByType(int pageNumber = 1, int recordPerPage = 20, string filter = "", string viewName = "")
        {

            var currentType = this.EntityType;
            return GetPagingByType(currentType, pageNumber, recordPerPage, filter, viewName);

        }

        public PagingData GetPagingByType<T>(int pageNumber = 1, int recordPerPage = 20, string filter = "", string viewName = "") where T : BaseEntity
        {
            Type currentType = typeof(T);

            return GetPagingByType(currentType, pageNumber, recordPerPage, filter, viewName);

        }

        /// <summary>
        /// Get view name object
        /// </summary>
        /// <param name="currentType"></param>
        /// <returns></returns>
        private string GetViewNameByType(Type currentType)
        {
            var viewName = currentType.Name;

            //rà trong attribute xem có viewName không
            System.Reflection.MemberInfo info = currentType;
            object[] attributes = info.GetCustomAttributes(true);

            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i] is DatabaseViewName)
                {
                    viewName = ((DatabaseViewName)attributes[i]).ViewName;
                    break;
                } 
            }

            return viewName;
        }

        /// <summary>
        /// Lấy pagging không dùng store
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="recordPerPage"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        /// phamkhanhhand 05.07.2023
        public PagingData GetPagingByType(Type currentType, int pageNumber = 1, int recordPerPage = 20, string filter = "", string viewName = "")
        {

            List<FilterSetting> dctWhere = JsonConvert.DeserializeObject<List<FilterSetting>>(filter);
             
            var from = (pageNumber - 1) * recordPerPage;//tính từ 0
            var to = recordPerPage;
            int totalRecord = 0;

            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = currentType.Name;

                //rà trong attribute xem có viewName không
                System.Reflection.MemberInfo info = currentType;
                object[] attributes = info.GetCustomAttributes(true);

                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i] is DatabaseViewName)
                    {
                        viewName = ((DatabaseViewName)attributes[i]).ViewName;
                        break;
                    }

                }
            }

            PagingData pg = new PagingData();

            MethodInfo method1 = typeof(BaseDL).GetMethod(nameof(BaseDL.SelectPagingNotStore));
            MethodInfo generic1 = method1.MakeGenericMethod(currentType);

            //todo
            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(currentType);


            object[] parametersArray = new object[] {

                viewName,
                from,
                to,
                null,//totalrecord , 
               null,// "SortBy",//sortby,
                dctWhere,

            };
            var lstdetail = (IList)generic1.Invoke(dl, parametersArray);

            int sumRecord;
            //output
            sumRecord = (int)parametersArray[3];

            EntityCollection collection = new EntityCollection();

            pg.ListObject = lstdetail;
            pg.CurrentCount = lstdetail.Count;
            pg.TotalRecord = sumRecord;

            return pg;

        }

        /// <summary>
        /// Lấy pagging không dùng store
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="recordPerPage"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        /// phamkhanhhand 05.07.2023
        public PagingData GetPagingByTypeBasicfilter(Type currentType, int pageNumber = 1, int recordPerPage = 20, string filter = "", string viewName = "")
        {
            Dictionary<string, object> dctWhere = JsonToDict(filter);




            //var currentType = typeof(T);


            var from = (pageNumber - 1) * recordPerPage;//tính từ 0
            var to = recordPerPage;
            int totalRecord = 0;

            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = currentType.Name;

                //rà trong attribute xem có viewName không
                System.Reflection.MemberInfo info = currentType;
                object[] attributes = info.GetCustomAttributes(true);

                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i] is DatabaseViewName)
                    {
                        viewName = ((DatabaseViewName)attributes[i]).ViewName;
                        break;
                    }

                }
            }

            PagingData pg = new PagingData();

            MethodInfo method1 = typeof(BaseDL).GetMethod(nameof(BaseDL.SelectPagingNotStoreBasicFilter));
            MethodInfo generic1 = method1.MakeGenericMethod(currentType);

            //todo
            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(currentType);


            object[] parametersArray = new object[] {

                viewName,
                from,
                to,
                null,//totalrecord , 
               null,// "SortBy",//sortby,
                dctWhere,

            };
            var lstdetail = (IList)generic1.Invoke(dl, parametersArray);

            int sumRecord;
            //output
            sumRecord = (int)parametersArray[3];

            EntityCollection collection = new EntityCollection();

            pg.ListObject = lstdetail;
            pg.CurrentCount = lstdetail.Count;
            pg.TotalRecord = sumRecord;

            return pg;

        }

        /// <summary>
        /// Lấy paging, nhưng chỉ trả về data, không trả về paging
        /// </summary>
        /// <param name="currentType"></param>
        /// <param name="pageNumber"></param>
        /// <param name="recordPerPage"></param>
        /// <param name="filter"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        /// phamkhanhhand Nov 23, 2024
        public List<T> GetPagingByTypeDataOnly<T>(int pageNumber = 1, int recordPerPage = 20, string filter = "", string viewName = "")
        {
            Type currentType = typeof(T);

            Dictionary<string, object> dctWhere = JsonToDict(filter);

            //var currentType = typeof(T);


            var from = (pageNumber - 1) * recordPerPage;//tính từ 0
            var to = recordPerPage;
            int totalRecord = 0;

            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = currentType.Name;

                //rà trong attribute xem có viewName không
                System.Reflection.MemberInfo info = currentType;
                object[] attributes = info.GetCustomAttributes(true);

                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i] is DatabaseViewName)
                    {
                        viewName = ((DatabaseViewName)attributes[i]).ViewName;
                        break;
                    }

                }
            }

            PagingData pg = new PagingData();

            MethodInfo method1 = typeof(BaseDL).GetMethod(nameof(BaseDL.SelectPagingNotStoreBasicFilter));
            MethodInfo generic1 = method1.MakeGenericMethod(currentType);

            //todo
            BaseDL dl = (BaseDL)DLFactory.CreateDLByType(currentType);


            object[] parametersArray = new object[] {

                viewName,
                from,
                to,
                null,//totalrecord , 
               null,// "SortBy",//sortby,
                dctWhere,

            };
            var lstdetail = (IList)generic1.Invoke(dl, parametersArray);

            return (List<T>)lstdetail;
        }




        #endregion

    }
}
