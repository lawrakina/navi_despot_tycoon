using UnityEngine;

namespace Misc.Trajectory.Physic
{
    [RequireComponent(typeof(Rigidbody))]
    [HelpURL("https://docs.google.com/document/d/1pOku7G-X-U1qHPqZLVki4D_UomUsONdF8uoXV8D33Ts/edit#heading=h.e49twqm5rglg")]
    public class RigidbodyPhysicBodyWrapper : PhysicBodyBase
    {

        public override Vector3 velocity
        {
            get
            {
                if (rb == null)
                {
                    print("velocity get");
                    return Vector3.zero;
                }
                return rb.velocity;
            }
            set
            {
                if (rb == null)
                {
                    print("velocity set");
                    return;
                }
                
                rb.velocity = value;
            }
        }

        public override Vector3 angularVelocity
        {
            get
            {
                if (rb == null)
                {
                    print("angular velocity get");
                    return Vector3.zero;
                }
                return rb.angularVelocity;
            }
            set
            {
                if (rb == null)
                {
                    print("angular velocity set");
                    return;
                }
                rb.angularVelocity = value;
            }
        }

        protected override void PhysicStepInternal(float deltaTime)
        {
            
        }

        public override void OnEnterToWater()
        {
          //
        }
    }
}