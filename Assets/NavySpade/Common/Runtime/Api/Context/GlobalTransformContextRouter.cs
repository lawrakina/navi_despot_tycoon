using UnityEngine;

namespace NavySpade.Common.Runtime.Api.Context
{
    public class GlobalTransformContextRouter : MonoBehaviour, ITransformContextProvider
    {
        public Transform Root => GlobalTransformContextProvider.Instance.Root;
    }
}