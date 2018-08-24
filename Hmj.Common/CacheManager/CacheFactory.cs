namespace Hmj.Common.CacheManager
{
    public class CacheFactory
    {
       private static CacheFactory _instance = new CacheFactory();
       public static CacheFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public ICache CreateCoreCacheInstance()
        {
            return new AspNetCache();
        }

        public ICache CreateSessionCacheInstance()
        {
            return new SessionCache();
        }
    }
}
