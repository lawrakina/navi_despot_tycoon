using System;
using System.Collections.Generic;
using NavySpade.Modules.Saving.Runtime.Configuration;
using NavySpade.Modules.Saving.Runtime.Interfaces;

namespace NavySpade.Modules.Saving.Runtime
{
    /// <summary>
    /// Global context. Just for easier syntax. Uses inside <see cref="IRawSaveService"/>.
    /// </summary>
    public static class SaveManager
    {
        private static IRawSaveService InternalService { get; }

        static SaveManager()
        {
            InternalService = SaveConfig.Instance.Backend;
        }

        public static void Save<T>(string key, T value) => InternalService.Save(key, value);

        public static void Save<T>(string key, Func<T> value) => InternalService.Save(key, value());

        public static T Load<T>(string key, T defaultValue = default) => InternalService.Load(key, defaultValue);

        public static bool HasKey(string key) => InternalService.HasKey(key);

        public static void DeleteKey(string key) => InternalService.DeleteKey(key);

        public static void DeleteAll() => InternalService.DeleteAll();

        public static object LoadRaw(string key) => InternalService.LoadRaw(key).ToString();

        public static void SaveRaw(string key, object value) => InternalService.SaveRaw(key, value);

        public static IEnumerable<string> GetAllKeys() => InternalService.GetAllKeys();
    }
}