using System;
using System.Collections.Generic;

namespace UILayouTaro
{
    public static class InternalCachePool
    {
        private static Dictionary<Type, IMissingSpriteCache> cacheDict = new Dictionary<Type, IMissingSpriteCache>();
        public static T Get<T>() where T : IMissingSpriteCache, new()
        {
            if (cacheDict.ContainsKey(typeof(T)))
            {
                return (T)cacheDict[typeof(T)];
            }

            var newCacheInstance = new T();
            cacheDict[typeof(T)] = newCacheInstance;
            return newCacheInstance;
        }
    }
}