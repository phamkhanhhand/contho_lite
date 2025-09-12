using CT.Models.Entity;
using CT.Utils;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;

namespace CT.Usermanager
{
    public class CachedPermissionService : IPermissionService
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CachedPermissionService> _logger;

        public CachedPermissionService(IMemoryCache cache, IHttpClientFactory httpClientFactory, ILogger<CachedPermissionService> logger)
        {
            _cache = cache;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<HashSet<string>> GetPermissionsAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return new HashSet<string>();

            string cacheKey = $"permissions:{token}";

            if (_cache.TryGetValue(cacheKey, out HashSet<string> cached))
            {
                return cached;
            }

            try
            {
                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(HttpMethod.Get, "https://gateway.example.com/api/user-permissions");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var permissions = JsonSerializer.Deserialize<HashSet<string>>(json);

                _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(10)); // TTL: 10 phút

                return permissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch permissions from gateway.");
                return new HashSet<string>(); // fail-safe
            }
        }
    }



}

