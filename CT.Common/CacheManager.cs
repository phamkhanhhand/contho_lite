//using CT.Models.Enumeration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Security.AccessControl;
//using System.Text;
//using System.Threading.Tasks;

//namespace CT.Common
//{

//    /// <summary>
//    /// Cache
//    /// TODO thời gian xóa cache
//    /// </summary>
//    /// phamkhanhhand 13.05.2023
//    public class CacheManager
//    {
//        /// <summary>
//        /// Cache lưu GeneralSetting
//        /// </summary>
//        private static Dictionary<string, object> dctCacheGeneralSetting = new Dictionary<string, object>();

//        /// <summary>
//        /// Cache không xác định
//        /// </summary>
//        private static Dictionary<string, object> dctCacheOther = new Dictionary<string, object>();



//        /// <summary>
//        /// Set cache GeneralSetting
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="obj"></param>
//        /// <param name="isOverload"></param>
//        public static void SetCacheGeneralSetting(string key, object obj, bool isOverload = true)
//        {
//            SetCache((int)CacheType.GeneralSetting, key, obj, isOverload);
//        }

//        /// <summary>
//        /// Dựa vào type lấy cache
//        /// </summary>
//        /// <param name="cacheType"></param>
//        /// <returns></returns>
//        public static Dictionary<string, object> GetCacheDictByType(int cacheType)
//        {

//            Dictionary<string, object> dctCache = null;

//            switch (cacheType)
//            {
//                case (int)CacheType.GeneralSetting:
//                    dctCache = dctCacheGeneralSetting;
//                    break;

//                case (int)CacheType.Other:
//                    dctCache = dctCacheOther;
//                    break;

//                default:
//                    break;
//            }

//            return dctCache;
//        }

//        /// <summary>
//        /// SEt cache
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="obj"></param>
//        /// <param name="isOverload">Ghi đè hay không</param>
//        /// phamkhanhhand 13.05.2023
//        public static void SetCache(int cacheType, string key, object obj, bool isOverload = true)
//        {
//            Dictionary<string, object> dctCache = GetCacheDictByType(cacheType);


//            //Nếu tồn tại
//            if (dctCache.ContainsKey(key))
//            {
//                if (isOverload)
//                {
//                    dctCache[key] = obj;
//                }
//                else
//                {
//                    //nếu không ghi đè thì không làm gì cả
//                }
//            }
//            else
//            {
//                dctCache.Add(key, obj);
//            }

//        }


//        /// <summary>
//        /// lấy giá trị cache GeneralSetting
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public static object GetCacheGeneralSetting(string key)
//        {
//            return GetCache((int)CacheType.GeneralSetting, key);
//        }


//        /// <summary>
//        /// lấy giá trị cache thường
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public static object GetCache(int cacheType, string key)
//        {
//            Dictionary<string, object> dctCache = GetCacheDictByType(cacheType);

//            dctCache.TryGetValue(key, out object obj);

//            return obj;
//        }



//        /// <summary>
//        /// Gen key cache, không bị lặp nhau
//        /// </summary>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        public static string GenKeyCache(string type = "")
//        {
//            var key = "";
//            Guid guid = Guid.NewGuid();

//            key = "_" + guid;

//            return key;
//        }

//    }
//}
