using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Misc.Trajectory.Physic
{
    [HelpURL("https://docs.google.com/document/d/1pOku7G-X-U1qHPqZLVki4D_UomUsONdF8uoXV8D33Ts/edit#heading=h.mbwolvju1wwu")]
    public class PhysicCalculatedBody : MonoBehaviour
    {
        public class BodyData
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 velocity;
            public Vector3 angularVelocity;
        }
        
        public static Dictionary<PhysicCalculatedBody, BodyData> savedBodies = new Dictionary<PhysicCalculatedBody, BodyData>();

        public event Action OnEndSimulate;

        [SerializeField] private Behaviour[] _disableBySimulation;
        [SerializeField] private PhysicReact[] _reactors;
        
        private Rigidbody _rb;
        private bool _isSimulate;

        [CanBeNull]
        public Rigidbody Rb => _rb;

        public bool IsSimulate => _isSimulate;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            savedBodies.Add(this, new BodyData());
        }

        private void OnDisable()
        {
            savedBodies.Remove(this);
        }

        public static void StartSimulation()
        {
            foreach (var savedBody in savedBodies)
            {
                savedBody.Key._isSimulate = true;
                foreach (var component in savedBody.Key._disableBySimulation)
                {
                    component.enabled = false;
                }

                foreach (var physicReact in savedBody.Key._reactors)
                {
                    physicReact.IsSimulationEnabled = true;
                }
            }
        }

        public static void EndSimulation()
        {
            foreach (var savedBody in savedBodies)
            {
                savedBody.Key._isSimulate = false;
                foreach (var component in savedBody.Key._disableBySimulation)
                {
                    component.enabled = true;
                }
                
                foreach (var physicReact in savedBody.Key._reactors)
                {
                    physicReact.IsSimulationEnabled = false;
                }
                
                savedBody.Key.OnEndSimulate?.Invoke();
            }
        }
    }
}