using UnityEngine;

namespace Misc.Trajectory
{
    [HelpURL("https://docs.google.com/document/d/1pOku7G-X-U1qHPqZLVki4D_UomUsONdF8uoXV8D33Ts/edit#heading=h.bpmzdp1j9dz3")]
    public class TrajectoryCallOptimazed : TrajectoryBase
    {
        [SerializeField] private TrajectoryBase _attachedTrajectory;
        
        private TrajectoryConfig _config;
        private Vector3 _xLastPos, _xyLastPos;

        private void Awake()
        {
            _config = TrajectoryConfig.Instance;
        }

        public override void EnableTrajectory()
        {
            _xLastPos = Vector3.zero;
            _xyLastPos = Vector3.zero;
            _attachedTrajectory.EnableTrajectory();
        }

        public override void UpdateTrajectory(Vector3 targetPos)
        {
            var canUpdate = false;

            if (Vector3.Distance(_xLastPos, targetPos) > _config.StartCalculateToleraste)
            {
                _xLastPos = targetPos;   
                canUpdate = true;
            }

            if (canUpdate)
                _attachedTrajectory.UpdateTrajectory(targetPos);
        }

        public override void DisableTrajectory()
        {
            _attachedTrajectory.DisableTrajectory();
        }
    }
}