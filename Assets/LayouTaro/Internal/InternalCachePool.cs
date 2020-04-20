namespace UILayouTaro
{
    public static class InternalCachePool
    {
        public static T Get<T>() where T : IMissingSpriteCache, new()
        {
            return new T();
        }
    }
}