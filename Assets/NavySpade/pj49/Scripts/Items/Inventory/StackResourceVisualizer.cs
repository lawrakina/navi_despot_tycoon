using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Items.Inventory
{
    public class StackResourceVisualizer : InventoryVisualizer
    {
        [SerializeField] private ResourceAsset _resourceAsset;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Vector2Int _stackSize;
        [SerializeField] private Vector3 _padding;

        [Foldout("Debug")] 
        [SerializeField] private bool _isDebug;
        [SerializeField] private int _zHeight; 
        
        private int _objectsInStackCount;
        private Stack<GameObject> _resourceStack = new Stack<GameObject>();

        private void Start()
        {
            _inventory.ResourcesCountChanged += UpdateVisual;
            UpdateVisual();
        }

        private void OnDestroy()
        {
            _inventory.ResourcesCountChanged -= UpdateVisual;
        }

        private void UpdateVisual()
        {
            int itemsCount = _inventory.Count(_resourceAsset);
            if (itemsCount < _objectsInStackCount)
            {
                RemoveFromStackUntil(itemsCount);
                return;
            }

            if (itemsCount > _objectsInStackCount)
            {
                AddToStackUntil(itemsCount);
            }
        }

        private void AddToStack()
        {
            var item = Instantiate(_prefab, GetSpawnPosition(_objectsInStackCount), Quaternion.identity, transform);
            _resourceStack.Push(item);
            _objectsInStackCount += 1;
        }

        private Vector3 GetSpawnPosition(int indexOfItem)
        {
            int z = indexOfItem / (_stackSize.x * _stackSize.y);
            indexOfItem -= (z * _stackSize.x * _stackSize.y);
            int y = indexOfItem / _stackSize.x;
            int x = indexOfItem % _stackSize.x;
            
            return transform.TransformPoint(new Vector3(x * _padding.x, y * _padding.y, z * _padding.z));
        }

        private void RemoveFromStackUntil(int value)
        {
            while (value != _objectsInStackCount)
            {
                RemoveFromStack();
            }
        }
        
        private void AddToStackUntil(int value)
        {
            while (value != _objectsInStackCount)
            {
                AddToStack();
            }
        }
        
        private void RemoveFromStack()
        {
            GameObject go = _resourceStack.Pop();
            Destroy(go);
            _objectsInStackCount -= 1;
        }

        private void OnDrawGizmos()
        {
            if (_isDebug == false)
                return;
            
            Gizmos.color = Color.red;
            int total = _stackSize.x * _stackSize.y * _zHeight;
            for (int i = 0; i < total; i++)
            {
                Gizmos.DrawCube(GetSpawnPosition(i), Vector3.one * 0.1f);
            }
        }
    }
}