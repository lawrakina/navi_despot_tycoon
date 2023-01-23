using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NavySpade.Core.Runtime.Levels.Data.Additional
{
    [Serializable]
    [AddTypeMenu("Background")]
    public class BackgroundLevelExtension : ILevelExtensionData
    {
        [SerializeField] private GameObject _prefab;

        private GameObject _instance;
        
        public void Apply()
        {
            _instance = Object.Instantiate(_prefab);
        }

        public void Clear()
        {
            Object.Destroy(_instance);
        }
    }
}