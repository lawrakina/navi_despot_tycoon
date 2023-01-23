using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NavySpade.Modules.Pooling.Runtime
{
    public class ObjectPool<T> : BasePool where T : Component
    {
        private const string PoolPrefix = "Pool_of_";
        
        private Transform _parent;
        private int _count;
        private T _pooledObject;
        private List<T> _inGame = new List<T>();
        private Stack<T> _inPool = new Stack<T>();
        private Action<T> _perObjectCreated;

        public void Initialize(Transform parent, int count, T pooledObject, Action<T> perObjectCreated = null)
        {
            _perObjectCreated = perObjectCreated;
            _pooledObject = pooledObject;
            _parent = parent == null ? CreateParentForPool(pooledObject.name) : parent;
            _count = count;
            SetPreload();
        }

        private Transform CreateParentForPool(string prefabName)
        {
            GameObject parent = new GameObject
            {
                name = PoolPrefix + prefabName
            };
            return parent.transform;
        }
        
        private void SetPreload()
        {
            for (var i = 0; i < _count; i++)
            {
                var init = Object.Instantiate(_pooledObject);
                
                _perObjectCreated?.Invoke(init);
                
                _inPool.Push(init);
                _inPool.Peek().transform.SetParent(_parent);
            }
        }

        public T Get()
        {
            T value = null;
            if (_inPool.Count > 0)
            {
                value = _inPool.Pop();
            }
            else
            {
                value = Object.Instantiate(_pooledObject);
                
                _perObjectCreated?.Invoke(value);
                
                value.transform.SetParent(_parent);
            }

            _inGame.Add(value);
            return value;
        }

        public void Return(T value)
        {
            if(_inGame.Contains(value) == false)
                return;
            
            _inGame.Remove(value);
            value.transform.SetParent(_parent);
            _inPool.Push(value);
        }
    }

    public abstract class BasePool
    {
        public Type BaseType;
    }
}