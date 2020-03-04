using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CacheHelper.Redis
{
    public static class CacheHelper
    {
        /// <summary>
        /// 获取obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(this IDistributedCache cache,string key)
        {
            string returnString = null;
            var value = await cache.GetAsync(key);
            if (value != null)
            {
                returnString = Encoding.UTF8.GetString(value);
                return JsonConvert.DeserializeObject<T>(returnString, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            else
            {
                return default(T);
            }
        }
        public static async Task<object> GetObjectNoAsync(this IDistributedCache cache, string key)
        {
            string returnString = null;
            var value = await cache.GetAsync(key);
            if (value != null)
            {
                returnString = Encoding.UTF8.GetString(value);
                return JsonConvert.DeserializeObject(returnString, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            else
            {
                await cache.RemoveAsync(key);
                return null;
            }
        }
        public static T GetObject<T>(this IDistributedCache cache, string key)
        {
            string returnString = null;
            var value =  cache.Get(key);
            if (value != null)
            {
                returnString = Encoding.UTF8.GetString(value);
                return JsonConvert.DeserializeObject<T>(returnString);
            }
            else
            {
                cache.Remove(key);
                return default(T);
            }
        }
        public static object GetObjectNo(this IDistributedCache cache, string key)
        {
            string returnString = null;
            var value = cache.Get(key);
            if (value != null)
            {
                returnString = Encoding.UTF8.GetString(value);
                return JsonConvert.DeserializeObject(returnString);
            }
            else
            {
                cache.Remove(key);
                return null;
            }
        }
        public static async Task<string> GetStrAsync(this IDistributedCache cache, string key)
        {
            string returnString = null;
            var value = await cache.GetAsync(key);
            if (value != null)
            {
                returnString = Encoding.UTF8.GetString(value);
                return returnString;
            }
            else
            {
                await cache.RemoveAsync(key);
                return string.Empty;
            }
        }
        public static string GetString(this IDistributedCache cache, string key)
        {
            string returnString = null;
            var value = cache.Get(key);
            if (value != null)
            {
                returnString = Encoding.UTF8.GetString(value);
                return returnString;
            }
            else
            {
                cache.Remove(key);
                return string.Empty;
            }
        }
        
        public static async Task<bool> SetObjectAsync(this IDistributedCache cache,string key, Object value, int SlidingExpirationSec=30)
        {
            byte[] val = null;
            if (value!=null)
            {
                var str = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                val = Encoding.UTF8.GetBytes(str);
            }
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            //设置绝对过期时间 两种写法
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            // options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(30));
            //设置滑动过期时间 两种写法
            options.SlidingExpiration = TimeSpan.FromSeconds(SlidingExpirationSec);
            //options.SetSlidingExpiration(TimeSpan.FromSeconds(30));
            //添加缓存
            await cache.SetAsync(key, val, options);
            //刷新缓存
            cache.Refresh(key);
            return cache.Exists(key);
        }
        public static async Task<bool> SetStrAsync(this IDistributedCache cache, string key, string value, int SlidingExpirationSec = 30)
        {
            byte[] val = null;
            if (value != null)
            {
                val = Encoding.UTF8.GetBytes(value);
            }
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            //设置绝对过期时间 两种写法
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            // options.SetAbsoluteExpiration(DateTime.Now.AddMinutes(30));
            //设置滑动过期时间 两种写法
            options.SlidingExpiration = TimeSpan.FromSeconds(SlidingExpirationSec);
            //options.SetSlidingExpiration(TimeSpan.FromSeconds(30));
            //添加缓存
            await cache.SetAsync(key, val, options);
            //刷新缓存
            cache.Refresh(key);
            return cache.Exists(key);
        }
        public static bool RemoveKey(this IDistributedCache cache,string key)
        {
            bool ReturnBool = false;
            if (key != "" || key != null)
            {
                cache.Remove(key);
                if (cache.Exists(key) == false)
                {
                    ReturnBool = true;
                }
            }
            return ReturnBool;
        }
        public static async Task<bool> ModifyObjectAsync<T>(this IDistributedCache cache, string key, T value)
        {
            bool ReturnBool = false;
            if (key != "" || key != null)
            {
                if (cache.RemoveKey(key))
                {
                    ReturnBool = await cache.SetObjectAsync(key, value);
                }

            }
            return ReturnBool;
        }
        public static async Task<bool> ModifyStrAsync(this IDistributedCache cache, string key, string value)
        {
            bool ReturnBool = false;
            if (key != "" || key != null)
            {
                if (cache.RemoveKey(key))
                {
                    ReturnBool = await cache.SetStrAsync(key, value);
                }

            }
            return ReturnBool;
        }
        public static bool Exists(this IDistributedCache cache, string key)
        {
            bool ReturnBool = true;
            byte[] val = cache.Get(key);
            if (val == null || val.Length == 0)
            {
                ReturnBool = false;
            }
            return ReturnBool;
        }
    }
}
