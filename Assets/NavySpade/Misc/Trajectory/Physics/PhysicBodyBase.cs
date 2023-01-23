using System.Collections.Generic;
using UnityEngine;

namespace Misc.Trajectory.Physic
{
    public abstract class PhysicBodyBase : MonoBehaviour
    {
        [HideInInspector] public List<Vector3> _collisionPoints = new List<Vector3>();

        protected Rigidbody rb { get; private set; }


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }

        private void FixedUpdate()
        {
            PhysicStep(Time.fixedDeltaTime);
        }

        public abstract Vector3 velocity { get; set; }
        public abstract Vector3 angularVelocity { get; set; }

        public void PhysicStep(float deltaTime)
        {
            PhysicStepInternal(deltaTime);
        }

        protected abstract void PhysicStepInternal(float deltaTime);

        public abstract void OnEnterToWater();

        public void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnter_(collision);
        }

        protected virtual void OnCollisionEnter_(Collision collision)
        {
        }
    }
}