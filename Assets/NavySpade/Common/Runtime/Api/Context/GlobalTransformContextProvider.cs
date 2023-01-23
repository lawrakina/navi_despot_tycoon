using UnityEngine;

namespace NavySpade.Common.Runtime.Api.Context
{
    public class GlobalTransformContextProvider : MonoBehaviour, ITransformContextProvider
    {
        public static GlobalTransformContextProvider Instance { get; private set; }

        [field: SerializeField] public Transform Root { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Reset()
        {
            Root = transform;
        }
    }
}