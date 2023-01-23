using Misc.Trajectory.Physic;
using UnityEngine;

namespace Misc.Trajectory
{
    public class PhysicCalculation : TrajectoryCalculationBase
    {
        protected override void UpdateTrajectory(GameObject prefab, Vector3 origin, Vector3 speed,
            Vector3 angularVelocity)
        {
            Physics.autoSimulation = false;

            // Подготовка:
            foreach (var body in PhysicCalculatedBody.savedBodies)
            {
                var rb = body.Key.Rb;

                if (rb == null)
                    continue;

                body.Value.position = rb.transform.position;
                body.Value.rotation = rb.transform.rotation;
                body.Value.velocity = rb.velocity;
                body.Value.angularVelocity = rb.angularVelocity;
            }

            var bullet = Object.Instantiate(prefab, origin, Quaternion.identity);

            PhysicCalculatedBody.StartSimulation();

            bool isFillPoint = false;
            // Симуляция:
            for (int i = 0; i < Config.SimulationFramesCount; i++)
            {
                Physics.Simulate(Config.TrajectorySimulationToleraste);
                Result_.Points[i] = bullet.transform.position;
            }

            if (isFillPoint == false)
            {
                Result_.SecondCollisionPointIndex = null;
            }

            // Зачистка:

            foreach (var body in PhysicCalculatedBody.savedBodies)
            {
                if (body.Key.Rb == null)
                    continue;

                body.Key.transform.position = body.Value.position;
                body.Key.transform.rotation = body.Value.rotation;
                body.Key.Rb.velocity = body.Value.velocity;
                body.Key.Rb.angularVelocity = body.Value.angularVelocity;
            }

            if (bullet != null)
                Object.DestroyImmediate(bullet.gameObject);

            PhysicCalculatedBody.EndSimulation();
            Physics.autoSimulation = true;
        }
    }
}