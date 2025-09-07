//using CT.Models.Enumeration;
//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Security.AccessControl;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Caching.Memory;

//namespace CT.Common
//{

//    public static class UserCacheService
//    {
//        //private readonly IMemoryCache _memoryCache;
//        //private readonly IHttpContextAccessor _httpContextAccessor;

//        //public UserCacheService(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
//        //{
//        //    _memoryCache = memoryCache;
//        //    _httpContextAccessor = httpContextAccessor;
//        //}

//        private static readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
//        private static readonly IHttpContextAccessor _httpContextAccessor;

//        // Static constructor để khởi tạo IHttpContextAccessor
//        static UserCacheService()
//        {
//            _httpContextAccessor = new HttpContextAccessor(); // Khởi tạo IHttpContextAccessor
//        }


//        public static string GetUserId()
//        {
//            return _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;
//        }

//        // Lấy cache cho người dùng
//        public static T GetCache<T>(string cacheKey)
//        {
//            var userId = GetUserId();
//            if (string.IsNullOrEmpty(userId))
//            {
//                return default;
//            }

//            var fullCacheKey = $"{userId}_{cacheKey}";
//            return _memoryCache.TryGetValue(fullCacheKey, out T value) ? value : default;
//        }

//        // Thêm dữ liệu vào cache cho người dùng
//        public static void SetCache<T>(string cacheKey, T value)
//        {
//            var userId = GetUserId();
//            if (string.IsNullOrEmpty(userId))
//            {
//                return;
//            }

//            var fullCacheKey = $"{userId}_{cacheKey}";
//            _memoryCache.Set(fullCacheKey, value, TimeSpan.FromMinutes(30));
//        }

//        // Xóa cache khi người dùng đăng xuất
//        public static void RemoveCache(string cacheKey)
//        {
//            var userId = GetUserId();
//            if (string.IsNullOrEmpty(userId))
//            {
//                return;
//            }

//            var fullCacheKey = $"{userId}_{cacheKey}";
//            _memoryCache.Remove(fullCacheKey);
//        }
//    }



//}
