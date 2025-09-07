using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CT.Utils
{
    /// <summary>
    /// Example
    /// var user = _cacheHelper.Get<dynamic>(cacheKey);
    /// _cacheHelper.Set(cacheKey, user);
    /// </summary>
    public class MemoryCacheHelper
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheHelper(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Thêm vào cache
        public void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null)
        {
            var expiration = absoluteExpiration ?? TimeSpan.FromMinutes(30);
            _cache.Set(key, value, expiration);
        }

        // Lấy từ cache
        public T Get<T>(string key)
        {
            return _cache.TryGetValue(key, out T value) ? value : default;
        }

        // Kiểm tra sự tồn tại trong cache
        public bool Contains(string key)
        {
            return _cache.TryGetValue(key, out _);
        }

        // Xóa khỏi cache
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
    }
