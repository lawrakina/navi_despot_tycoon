using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;

namespace NavySpade.pj49.Scripts.UnitsQueues
{
    public class PlayerChain : ExtendedMonoBehavior<PlayerChain>
    {
        [field: SerializeField] public Transform ChainStartPos { get; private set; }
    }
}