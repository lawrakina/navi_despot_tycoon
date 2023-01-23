using UnityEngine;

namespace NavySpade.Common.Runtime.Api.Context
{
    public class LocalTransformContextProvider : MonoBehaviour, ITransformContextProvider
    {
        public Transform Root => transform;
    }
}