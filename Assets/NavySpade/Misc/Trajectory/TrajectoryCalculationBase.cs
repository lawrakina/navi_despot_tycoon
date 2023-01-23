using UnityEngine;

namespace Misc.Trajectory
{
    public abstract class TrajectoryCalculationBase
    {
        public class Result
        {
            public Vector3[] Points;
            public Vector3? FirstCollisionPoint;
            public int? SecondCollisionPointIndex;

            public Result(int pointCount)
            {
                Points = new Vector3[pointCount];
            }
        }
        
        public TrajectoryCalculationBase()
        {
            Config = TrajectoryConfig.Instance;

            _result = new Result(Config.SimulationFramesCount);
        }

        private Result _result;

        protected TrajectoryConfig Config { get; }

        protected Result Result_ => _result;
        
        public Result ShowTrajectory(GameObject prefab, Vector3 origin, Vector3 speed, Vector3 angularVelocity)
        {
            UpdateTrajectory(prefab, origin, speed, angularVelocity);
            return Result_;
        }

        protected abstract void UpdateTrajectory(GameObject prefab, Vector3 origin, Vector3 speed, Vector3 angularVelocity);
    }
}