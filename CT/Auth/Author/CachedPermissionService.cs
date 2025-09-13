//using CT.BL;
//using Microsoft.Extensions.Caching.Memory; 
//using System.Net.Http.Headers; 
//using System.Text.Json;

//namespace CT.Auth
//{
//    public class CachedPermissionService : IPermissionService
//    {
//        private readonly IMemoryCache _cache;
//        private readonly IHttpClientFactory _httpClientFactory;
//        private readonly ILogger<CachedPermissionService> _logger;

//        public CachedPermissionService(IMemoryCache cache, IHttpClientFactory httpClientFactory, ILogger<CachedPermissionService> logger)
//        {
//            _cache = cache;
//            _httpClientFactory = httpClientFactory;
//            _logger = logger;
//        }

//        public async Task<HashSet<string>> GetPermissionsAsync(string token)
//        {
//            if (string.IsNullOrWhiteSpace(token))
//                return new HashSet<string>();

//            string cacheKey = $"permissions:{token}";

//            if (_cache.TryGetValue(cacheKey, out HashSet<string> cached))
//            {
//                return cached;
//            }

//            try
//            {
//                var client = _httpClientFactory.CreateClient();

//                var request = new HttpRequestMessage(HttpMethod.Get, "https://gateway.example.com/api/user-permissions");
//                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

//                var response = await client.SendAsync(request);
//                response.EnsureSuccessStatusCode();

//                var json = await response.Content.ReadAsStringAsync();
//                var permissions = JsonSerializer.Deserialize<HashSet<string>>(json);

//                _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(10)); // TTL: 10 phút

//                return permissions;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Failed to fetch permissions from gateway.");
//                return new HashSet<string>(); // fail-safe
//            }
//        }



//        //todo lấy trong database
//        public async Task<IUserContext> GetDataContext(string username)
//        {

//            string cacheKey = $"data_context_{username}";

//            if (_cache.TryGetValue(cacheKey, out IUserContext cached))
//            {
//                return cached;
//            }

//            try
//            {
//                IUserContext userContext = new UserContext();

//                userContext.Username= username;

//                adm_EmployeeBL employeeBL = new adm_EmployeeBL();
//                var employee = employeeBL.GetEmployeeByUsername(username);
//                if (employee != null)
//                {
//                    userContext.employee= employee;
//                }


//                _cache.Set(cacheKey, userContext, TimeSpan.FromMinutes(10)); // TTL: 10 phút

//                return userContext;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Failed to fetch permissions from gateway.");
//                return new UserContext(); // fail-safe
//            }
//        }

//    }



//}

