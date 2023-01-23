using NaughtyAttributes;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Items
{
    public class StackItem : MonoBehaviour
    {
        public float Height;

        [Foldout("Debug")] public bool IsDrawDebug;

        private void OnDrawGizmos()
        {
            if(IsDrawDebug == false)
                return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * Height);
        }
    }
}