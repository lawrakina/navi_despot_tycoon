using JetBrains.Annotations;
using UnityEngine;

namespace NavySpade.Common.Runtime.Api.Context
{
    public interface ITransformContextProvider
    {
        [PublicAPI]
        Transform Root { get; }
    }
}