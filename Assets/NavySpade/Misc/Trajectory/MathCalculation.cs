using Misc.Trajectory;
using UnityEngine;

namespace Misc.Trajectory
{
    public class MathCalculation : TrajectoryCalculationBase
    {
        protected override void UpdateTrajectory(GameObject prefab, Vector3 origin, Vector3 speed, Vector3 angularVelocity)
        {
            var last_pos = origin;
            var velocity = speed;
            var bounce_damping = 1;
            var dt = Config.TrajectorySimulationToleraste;
            
            Result_.Points[0] = origin;
            
            int i = 1;

            Vector3? firstCollisionPoint = null;
            int? secondCollisionPontIndex = null;
            while(i < Config.SimulationFramesCount){
                velocity += Physics.gravity * dt;
                RaycastHit hit;    
                if(Physics.Linecast(last_pos, (last_pos + (velocity * dt)), out hit))
                {
                    if (!hit.collider.isTrigger)
                    {
                        velocity = Vector3.Reflect(velocity * bounce_damping, hit.normal);
                        last_pos = hit.point;

                        if (firstCollisionPoint == null)
                            firstCollisionPoint = last_pos;
                        else
                        {
                            secondCollisionPontIndex = i;
                            break;
                        }
                    }
                }

                Result_.Points[i] = last_pos;
                
                last_pos += velocity * dt;
                
                i++;
            }

            Result_.FirstCollisionPoint = firstCollisionPoint;
            Result_.SecondCollisionPointIndex = secondCollisionPontIndex;

            Result_.FirstCollisionPoint = Result_.Points[Result_.Points.Length - 1];
            Result_.SecondCollisionPointIndex = Result_.Points.Length - 1;
        }
    }
}