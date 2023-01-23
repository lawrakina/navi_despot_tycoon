using System;
using System.Collections.Generic;
using System.Linq;
using NavySpade.Modules.Pooling.Runtime;
using UnityEngine;

namespace NavySpade.pj49.Scripts.UnitsQueues
{
    public class UnitChain : MonoBehaviour
    {
        [field: SerializeField] public CollectingUnit CurrentUnit { get; private set; }

        [field: SerializeField] public GameObject Shackle { get; private set; }
        
        [field: SerializeField] public Transform StartChainCollider { get; private set; }
        
        [field: SerializeField] public Transform EndChainCollider { get; private set; }

        public GameObject ChainPrefab;
        public float ChainDistance;

        private List<Transform> _chains;
        private static ObjectPool<Transform> _chainsPool;

        private bool _isActiveChain;
        private UnitChain _forwardInOrderChain;
        private int _indexInSquad;

        private void Awake()
        {
            if (_chainsPool == null)
            {
                _chainsPool = new ObjectPool<Transform>();
                _chainsPool.Initialize(
                    null, 
                    1000, 
                    ChainPrefab.transform,
                    transform1 => transform1.gameObject.SetActive(false));
            }
        }

        private void Start()
        {
            UnitSquad.Instance.UnitSetsInSquad += InstanceOnUnitSetsInSquad;
            Shackle.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            UnitSquad.Instance.UnitSetsInSquad -= InstanceOnUnitSetsInSquad;

            if(_chains == null)
                return;
            
            foreach (var chain in _chains)
            {
                _chainsPool.Return(chain);
                chain.gameObject.SetActive(false);
            }
            _chains.Clear();
        }

        private void Update()
        {
            UpdateChain();
        }
        
        private void InstanceOnUnitSetsInSquad()
        {
            if(_isActiveChain || UnitSquad.Instance.Units.Contains(CurrentUnit) == false)
                return;

            SetChain();
        }

        public void SetChain()
        {
            _isActiveChain = true;
            _indexInSquad = UnitSquad.Instance.Units.IndexOf(CurrentUnit);

            if (_indexInSquad > 0)
                _forwardInOrderChain = UnitSquad.Instance.Units[_indexInSquad - 1].GetComponent<UnitChain>();
            
            Shackle.gameObject.SetActive(true);
        }

        private void UpdateChain()
        {
            if(_isActiveChain == false)
                return;

            if (_indexInSquad <= 0)
            {
                UpdateChain(PlayerChain.Instance.ChainStartPos, StartChainCollider);
            }
            else
            {
                UpdateChain(_forwardInOrderChain.EndChainCollider, StartChainCollider);
            }
        }

        private void UpdateChain(Transform startJoint, Transform endJoint)
        {
            var distance = Vector3.Distance(startJoint.transform.position, endJoint.transform.position);
            var jointCounts = Mathf.CeilToInt(distance / ChainDistance);
            var direction = (endJoint.transform.position - startJoint.transform.position).normalized;

            if (_chains == null)
                _chains = new List<Transform>();
            
            if (jointCounts < _chains.Count)
            {
                int returnCount = _chains.Count - jointCounts;
                for (int i = 0; i < returnCount; i++)
                {
                    var instance = _chains[_chains.Count - 1 - i];
                    instance.gameObject.SetActive(false);
                }
                _chains.RemoveRange(_chains.Count - returnCount, returnCount);
                
                
            }
            else if (jointCounts > _chains.Count)
            {
                var instance = _chainsPool.Get();
                instance.gameObject.SetActive(true);
                _chains.Add(instance);
            }
            
            for (int i = 0; i < _chains.Count; i++)
            {
                Transform chain = _chains[i];
                
                chain.transform.position = 
                    Vector3.Lerp(
                        startJoint.transform.position, 
                        endJoint.transform.position, 
                        (float) i / jointCounts);
                chain.transform.forward = direction;
                chain.transform.right = Quaternion.Euler(0, 0, i % 2 == 0 ? 90 : 0) * chain.transform.right;
            }
        }
    }
}