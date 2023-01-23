using UnityEngine;

namespace Misc.Trajectory
{
    public abstract class TrajectoryBase : MonoBehaviour
    {
        public abstract void EnableTrajectory();
        public abstract void UpdateTrajectory(Vector3 targetPos);
        public abstract void DisableTrajectory();
    }
}