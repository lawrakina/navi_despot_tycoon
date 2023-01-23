using System;

namespace NavySpade.Modules.Configuration.Runtime.SO
{
    /// <summary>
    /// Has cached Instance.
    /// </summary>
    /// <typeparam name="T">Inherited type</typeparam>
    public abstract class ObjectConfig<T> : ObjectConfig where T : ObjectConfig<T>
    {
        private static T _internalInstance;
        private static readonly Type TypeCache = typeof(T);

        /// <summary>
        /// Cached instance of config, for avoiding expensive GetConfig();
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_internalInstance == null)
                {
                    _internalInstance = GetConfig(TypeCache) as T;
                }

                return _internalInstance;
            }
        }
        
        public static T GetConfig()
        {
            return GetConfig(TypeCache) as T;
        }
    }
}