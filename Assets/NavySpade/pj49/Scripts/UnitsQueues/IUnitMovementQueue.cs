using UnityEngine;

namespace NavySpade.pj49.Scripts
{
    public interface IUnitMovementQueue
    {
        Vector3 GetPosition(int unitIndex);
        
        bool IsEnable { get; }
    }
}