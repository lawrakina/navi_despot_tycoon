using System.Collections;
using UnityEngine;

namespace Misc.Trajectory.Physic
{
    public class PhysicReact : MonoBehaviour
    {
        private bool isSimulationEnabled;

        public bool IsSimulationEnabled
        {
            get => isSimulationEnabled;
            set
            {
                if (value == false)
                {
                    if (_waiter != null)
                    {
                        StopCoroutine(_waiter);
                        _waiter = null;
                    }

                    _waiter = StartCoroutine(WaitForMiracle());
                }
                else
                {
                    isSimulationEnabled = true;
                }
            }
        }

        private Coroutine _waiter;
        
        private IEnumerator WaitForMiracle()
        {
            yield return new WaitForEndOfFrame();
            isSimulationEnabled = false;
            _waiter = null;
        }

        public void OnCollisionEnter(Collision other)
        {
            if(isSimulationEnabled)
                return;
            
            OnCollisionEnterSave(other);
        }


        /// <summary>
        /// не срабатывает на симуляцию физики
        /// </summary>
        /// <param name="other"></param>
        protected virtual void OnCollisionEnterSave(Collision other)
        {
            
        }
    }
}