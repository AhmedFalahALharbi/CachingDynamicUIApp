using CachingDynamicUIApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CachingDynamicUIApp.Services
{
    public class JsonPlaceholderService
    {
        private readonly HttpClient _httpClient;
        private readonly RedisService _redisService;
        private readonly MemoryCacheService _memoryCacheService;
        private const string BaseUrl = "https://jsonplaceholder.typicode.com";
        private const int CacheExpiryHours = 1;

        public JsonPlaceholderService(RedisService redisService, MemoryCacheService memoryCacheService)
        {
            _httpClient = new HttpClient();
            _redisService = redisService;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            const string cacheKey = "users";
            
            // Try to get from memory cache first (fastest)
            if (_memoryCacheService.TryGetValue<List<User>>(cacheKey, out var memoryUsers))
            {
                return memoryUsers;
            }

            // Try to get from Redis cache next
            if (_redisService.KeyExists(cacheKey))
            {
                var cachedData = _redisService.StringGet(cacheKey);
                var usersFromCache = JsonConvert.DeserializeObject<List<User>>(cachedData);
                
                // Store in memory cache for faster access next time
                if (usersFromCache != null)
                {
                    _memoryCacheService.Set(cacheKey, usersFromCache, TimeSpan.FromMinutes(10));
                    return usersFromCache;
                }
            }
            
            // If not in any cache, fetch from API
            var response = await _httpClient.GetAsync($"{BaseUrl}/users");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(content);
            
            // Store in both caches
            _redisService.StringSet(cacheKey, content, TimeSpan.FromHours(CacheExpiryHours));
            if (users != null)
            {
                _memoryCacheService.Set(cacheKey, users, TimeSpan.FromMinutes(10));
            }
            
            return users ?? new List<User>();
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            const string cacheKey = "posts";
            
            // Try to get from memory cache first
            if (_memoryCacheService.TryGetValue<List<Post>>(cacheKey, out var memoryPosts))
            {
                return memoryPosts;
            }

            // Try to get from Redis cache next
            if (_redisService.KeyExists(cacheKey))
            {
                var cachedData = _redisService.StringGet(cacheKey);
                var postsFromCache = JsonConvert.DeserializeObject<List<Post>>(cachedData);
                
                // Store in memory cache
                if (postsFromCache != null)
                {
                    _memoryCacheService.Set(cacheKey, postsFromCache, TimeSpan.FromMinutes(10));
                    return postsFromCache;
                }
            }
            
            // If not in cache, fetch from API
            var response = await _httpClient.GetAsync($"{BaseUrl}/posts");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<List<Post>>(content);
            
            // Store in both caches
            _redisService.StringSet(cacheKey, content, TimeSpan.FromHours(CacheExpiryHours));
            if (posts != null)
            {
                _memoryCacheService.Set(cacheKey, posts, TimeSpan.FromMinutes(10));
            }
            
            return posts ?? new List<Post>();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            string cacheKey = $"user:{id}";
            
            // Try to get from memory cache first
            if (_memoryCacheService.TryGetValue<User>(cacheKey, out var memoryUser))
            {
                return memoryUser;
            }

            // Try to get from Redis cache next
            if (_redisService.KeyExists(cacheKey))
            {
                var cachedData = _redisService.StringGet(cacheKey);
                var userFromCache = JsonConvert.DeserializeObject<User>(cachedData);
                
                // Store in memory cache
                if (userFromCache != null)
                {
                    _memoryCacheService.Set(cacheKey, userFromCache, TimeSpan.FromMinutes(10));
                    return userFromCache;
                }
            }
            
            // If not in cache, fetch from API
            var response = await _httpClient.GetAsync($"{BaseUrl}/users/{id}");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(content);
            
            // Store in both caches
            _redisService.StringSet(cacheKey, content, TimeSpan.FromHours(CacheExpiryHours));
            if (user != null)
            {
                _memoryCacheService.Set(cacheKey, user, TimeSpan.FromMinutes(10));
            }
            
            return user;
        }

        public async Task<List<Post>> GetPostsByUserIdAsync(int userId)
        {
            string cacheKey = $"user:{userId}:posts";
            
            // Try to get from memory cache first
            if (_memoryCacheService.TryGetValue<List<Post>>(cacheKey, out var memoryPosts))
            {
                return memoryPosts;
            }

            // Try to get from Redis cache next
            if (_redisService.KeyExists(cacheKey))
            {
                var cachedData = _redisService.StringGet(cacheKey);
                var postsFromCache = JsonConvert.DeserializeObject<List<Post>>(cachedData);
                
                // Store in memory cache
                if (postsFromCache != null)
                {
                    _memoryCacheService.Set(cacheKey, postsFromCache, TimeSpan.FromMinutes(10));
                    return postsFromCache;
                }
            }
            
            // If not in cache, fetch from API
            var response = await _httpClient.GetAsync($"{BaseUrl}/users/{userId}/posts");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<List<Post>>(content);
            
            // Store in both caches
            _redisService.StringSet(cacheKey, content, TimeSpan.FromHours(CacheExpiryHours));
            if (posts != null)
            {
                _memoryCacheService.Set(cacheKey, posts, TimeSpan.FromMinutes(10));
            }
            
            return posts ?? new List<Post>();
        }
    }
}