
using System;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System.Collections.Generic;

namespace Ligg.Uwa.Application.Utilities
{
    public sealed class EnyimCacheHandler : ICacheHandler
    {
        private IMemcachedClient _handler;

        public EnyimCacheHandler(IMemcachedClient handler)
        {
            _handler = handler;
        }

        public T GetCache<T>(string key)
        {
            return _handler.Get<T>(key);
        }

        public bool SetCache<T>(string key, T t, DateTime? expireTime = null)
        {
            return _handler.Store(StoreMode.Set, key, t, expireTime??DateTime.Now.AddDays(100));
        }

        public bool RemoveCache(string key)
        {
            return _handler.Remove(key);
        }

        //#hash
        //##get
        public T GetHashFieldCache<T>(string key, string fieldKey)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, T> GetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, T> GetHashCache<T>(string key)
        {
            throw new NotImplementedException();
        }

        public List<T> GetHashToListCache<T>(string key)
        {
            throw new NotImplementedException();
        }

        //##set
        public int SetHashFieldCache<T>(string key, string fieldKey, T fieldValue)
        {
            throw new NotImplementedException();
        }

        public int SetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            throw new NotImplementedException();
        }

        //##remove
        public bool RemoveHashFieldCache(string key, string fieldKey)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict)
        {
            throw new NotImplementedException();
        }

    }
}