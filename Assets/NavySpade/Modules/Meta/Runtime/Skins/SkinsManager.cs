using System;
using System.Collections.Generic;
using NavySpade.Meta.Runtime.Skins.Configuration;
using NavySpade.Modules.Meta.Runtime.Skins.Configuration;
using NavySpade.Modules.Saving.Runtime;

namespace Core.Meta.Skins
{
    public static class SkinsManager
    {
        [Serializable]
        private struct SaveData
        {
            public List<int> OpenedIndexes;
            public float NextItemProgress;
            public int SelectedIndex;
        }

        private const string SaveDataKey = "Skins";

        public static event Action<SkinData> SelectedSkinChange;

        public static float NextSkinProgress
        {
            get => Data.NextItemProgress;
            set
            {
                var newData = Data;
                newData.NextItemProgress = value;
                Data = newData;
            }
        }

        public static SkinData SelectedSkin => Config.GetSkin(SelectedSkinIndex);

        public static int SelectedSkinIndex
        {
            get => Data.SelectedIndex;
            set
            {
                var newData = Data;
                newData.SelectedIndex = value;
                Data = newData;

                SelectedSkinChange?.Invoke(Config.GetSkin(value));
            }
        }

        private static SkinsConfig Config => SkinsConfig.Instance;

        private static SaveData Data
        {
            get
            {
                if (_data != null)
                    return _data.Value;

                if (SaveManager.HasKey(SaveDataKey) == false)
                {
                    var data = new SaveData();

                    data.OpenedIndexes = new List<int>();
                    data.OpenedIndexes.Add(Config.UnlockFromStartIndex);
                    data.SelectedIndex = Config.UnlockFromStartIndex;

                    Data = data;
                }

                _data = SaveManager.Load<SaveData>(SaveDataKey);

                return _data.Value;
            }
            set
            {
                _data = value;
                SaveManager.Save(SaveDataKey, value);
            }
        }

        private static SaveData? _data;
        private static SkinsConfig _config;

        public static bool IsSkinOpen(int index)
        {
            if (Data.OpenedIndexes == null)
                return false;

            return Data.OpenedIndexes.Contains(index);
        }

        public static bool IsAllSkinOpened(int skinCount)
        {
            if (Data.OpenedIndexes == null)
                return false;

            return Data.OpenedIndexes.Count >= skinCount;
        }

        public static void OpenSkin(int index)
        {
            var newData = Data;

            if (newData.OpenedIndexes == null)
                newData.OpenedIndexes = new List<int>();

            if (newData.OpenedIndexes.Contains(index))
                return;

            newData.OpenedIndexes.Add(index);

            Data = newData;
        }
    }
}