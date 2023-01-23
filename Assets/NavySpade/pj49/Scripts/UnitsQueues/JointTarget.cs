using UnityEngine;

namespace NavySpade.pj49.Scripts.UnitsQueues
{
    public class JointTarget : MonoBehaviour
    {
        [field: SerializeField] public Joint Joint { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    }
}