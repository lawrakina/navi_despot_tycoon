using System.Linq;
using NavySpade.Modules.Utils.Singletons.Runtime.Unity;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Items
{
    public class ResourcesHolder : MonoSingleton<ResourcesHolder>
    {
        [SerializeField] private ResourceAsset[] _resources;

        public ResourceAsset GetResource(int index)
        {
            return _resources[index];
        }

        public int GetIndexOfResource(ResourceAsset asset)
        {
            for (int i = 0; i < _resources.Length; i++)
            {
                if (_resources[i] == asset)
                    return i;
            }

            return -1;
        }
    }
}