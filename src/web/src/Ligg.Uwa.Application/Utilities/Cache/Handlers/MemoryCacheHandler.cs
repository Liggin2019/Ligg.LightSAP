
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Ligg.Uwa.Application.Shared;
using Microsoft.Extensions.DependencyInjection;
using Ligg.Infrastructure.Utilities.LogUtil;

namespace Ligg.Uwa.Application.Utilities
{
    public sealed class MemoryCacheHandler : ICacheHandler
    {
        //public MemoryCacheHandler(IMemoryCache handler)
        //{
        //    IMemoryCache _handler = handler;
        //}
        //private IMemoryCache _handler;

        private IMemoryCache _handler = GlobalContext.ServiceProvider.GetService<IMemoryCache>(); //这2种方式皆可, 现在构造函数方式失效對於：GlobalContext.ServiceProvider?.GetService<ICacheManager>();

        public T GetCache<T>(string key)
        {
            var value = _handler.Get<T>(key);
            return value;
        }

        public bool SetCache<T>(string key, T value, DateTime? expireTime = null)
        {
            try
            {
                if (expireTime == null)
                {
                    return _handler.Set<T>(key, value) != null;
                }
                else
                {
                    return _handler.Set(key, value, (expireTime.Value - DateTime.Now)) != null;
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Utilities.LogUtil.LogHelper.Error(ex);
            }
            return false;
        }

        public bool RemoveCache(string key)
        {
            _handler.Remove(key);
            return true;
        }



        //#hash
        //##get
        public T GetHashFieldCache<T>(string key, string fieldKey)
        {
            var dict = GetHashFieldCache<T>(key, new Dictionary<string, T> { { fieldKey, default(T) } });
            return dict[fieldKey];
        }

        public Dictionary<string, T> GetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            var hashFields = _handler.Get<Dictionary<string, T>>(key);
            foreach (KeyValuePair<string, T> keyValuePair in hashFields.Where(p => dict.Keys.Contains(p.Key)))
            {
                dict[keyValuePair.Key] = keyValuePair.Value;
            }
            return dict;
        }

        public Dictionary<string, T> GetHashCache<T>(string key)
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            var hashFields = _handler.Get<Dictionary<string, T>>(key);
            foreach (string field in hashFields.Keys)
            {
                dict[field] = hashFields[field];
            }
            return dict;
        }

        public List<T> GetHashToListCache<T>(string key)
        {
            List<T> list = new List<T>();
            var hashFields = _handler.Get<Dictionary<string, T>>(key);
            foreach (string field in hashFields.Keys)
            {
                list.Add(hashFields[field]);
            }
            return list;
        }

        //##set
        public int SetHashFieldCache<T>(string key, string fieldKey, T fieldValue)
        {
            return SetHashFieldCache<T>(key, new Dictionary<string, T> { { fieldKey, fieldValue } });
        }

        public int SetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            int count = 0;
            foreach (string fieldKey in dict.Keys)
            {
                count += _handler.Set(key, dict) != null ? 1 : 0;
            }
            return count;
        }

        //##remove
        public bool RemoveHashFieldCache(string key, string fieldKey)
        {
            Dictionary<string, bool> dict = new Dictionary<string, bool> { { fieldKey, false } };
            dict = RemoveHashFieldCache(key, dict);
            return dict[fieldKey];
        }

        public Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict)
        {
            var hashFields = _handler.Get<Dictionary<string, object>>(key);
            foreach (string fieldKey in dict.Keys)
            {
                dict[fieldKey] = hashFields.Remove(fieldKey);
            }
            return dict;
        }


    }
}