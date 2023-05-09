
using Ligg.Uwa.Application.Shared;

namespace Ligg.Uwa.Application.Utilities

{
    public class CacheFactory
    {
        private static ICacheHandler cacheHandler = null;
        private static readonly object lockHelper = new object();

        public static ICacheHandler Cache
        {
            get
            {
                if (cacheHandler == null)
                {
                    lock (lockHelper)
                    {
                        if (cacheHandler == null)
                        {
                            switch (GlobalContext.SystemSetting.CacheProvider)
                            {
                                case "Redis": cacheHandler = new RedisCacheHandler(); break;

                                default:
                                case "Memory": cacheHandler = new MemoryCacheHandler(); break;
                            }
                        }
                    }
                }
                return cacheHandler;
            }
        }
    }
}
