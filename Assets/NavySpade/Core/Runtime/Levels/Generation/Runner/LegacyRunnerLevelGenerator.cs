using System;
using System.Collections.Generic;
using System.Linq;
using Core.Game;
using EventSystem.Runtime.Core.Dispose;
using EventSystem.Runtime.Core.Managers;
using NavySpade.Core.Runtime.Game;
using NavySpade.Core.Runtime.Levels.Data;
using NavySpade.Core.Runtime.Levels.Generation.Abstract;
using NavySpade.Core.Runtime.Levels.Generation.Segments;
using NavySpade.Modules.Extensions.CsharpTypes;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Generation.Runner
{
    public class LegacyRunnerLevelGenerator : LevelGenerator<LegacyRunnerLevelData>
    {
        [SerializeField] private Transform _parent;
        [Min(0f)] [SerializeField] private float _createDistance = 50f;
        [Min(0f)] [SerializeField] private float _destroyDistance = 50f;
        [Min(0f)] [SerializeField] private float _step = 10f;

        [SerializeField] private List<LevelSegmentBase> _segmentsInGame;

        private Transform _finishPoint;
        private LegacyRunnerLevelData _data;
        private ISceneFactory<LevelSegmentBase> _factory;
        private EventDisposal _disposal;

        private LevelSegmentsData SegmentsData => _data.Segments;
        private Transform FocusPoint => LevelFocusPoint.Instance ? LevelFocusPoint.Instance.transform : null;

        #region Unity Methods
        
        private void Awake()
        {
            _factory = new LevelSegmentFactory();
            _disposal = new EventDisposal();

            //EventManager.Add(GenerateEnumEM.Update, UpdateInfinity).AddTo(_disposal);
            EventManager.Add<Transform>(MainEnumEvent.SetFinishPoint, SetFinishPoint).AddTo(_disposal);
        }

        private void Update()
        {
            if (GameLogic.Instance.States.IsStarted)
            {
                UpdateInfinity();
            }
        }
        
        private void OnDestroy()
        {
            _disposal.Dispose();
        }
        
        #endregion
        
        #region Overrided Methods

        protected override void StartGeneration(LegacyRunnerLevelData levelData)
        {
            _data = levelData;
            if (_data == null)
            {
                throw new NullReferenceException($"Data is empty!");
            }
            
            AddFirstSegment();

            var stepCount = SegmentsData.RandomOrder
                ? Mathf.CeilToInt(_createDistance / _step)
                : SegmentsData.All.Count + 1;

            var distance = Mathf.Abs(SegmentsData.Start.Size.z / 2f);

            for (var i = 0; i < stepCount; i++)
            {
                bool NeedCreateFinishSegment()
                {
                    return SegmentsData.RandomOrder && distance >= _data.Distance ||
                           SegmentsData.RandomOrder == false && i == stepCount - 1;
                }

                var isFinishSegment = NeedCreateFinishSegment();
                var prefab = isFinishSegment ? SegmentsData.Finish : GetNextSegment(i);

                var lastPositionZ = distance + prefab.Size.z / 2f;
                var newSegmentPosition = new Vector3(0, 0, lastPositionZ);
                var newSegment = AddSegment(prefab, newSegmentPosition);

                distance += prefab.Size.z;

                if (isFinishSegment)
                {
                    EventManager.Invoke(MainEnumEvent.SetFinishPoint, newSegment.Origin);
                    return;
                }
            }
        }

        protected override void OnCleanUp()
        {
            foreach (var element in _segmentsInGame)
            {
                Destroy(element.gameObject);
            }

            _segmentsInGame.Clear();

            EventManager.Invoke<Transform>(MainEnumEvent.SetFinishPoint, null);
        }
        
        #endregion

        private void SetFinishPoint(Transform point)
        {
            _finishPoint = point;
        }

        private bool NeedContinue(Vector3 currentPosition, Vector3 lastPosition)
        {
            var desiredPosition = currentPosition + _createDistance * Vector3.forward;
            var result = lastPosition.z < desiredPosition.z && _finishPoint == false;
            
            return result;
        }

        private void UpdateInfinity()
        {
            TryRemoveOldest();

            var lastSegment = _segmentsInGame.LastOrDefault();
            if (lastSegment == null || NeedContinue(FocusPoint.position, lastSegment.transform.position) == false)
            {
                return;
            }

            bool NeedCreateFinishSegment(float distance)
            {
                return distance >= _data.Distance - 1f;
            }

            var distance = Mathf.Abs(lastSegment.transform.position.z + lastSegment.Size.z / 2f);
            var isFinish = NeedCreateFinishSegment(distance);
            var prefab = isFinish ? SegmentsData.Finish : SegmentsData.All.RandomElement();

            var newSegmentPosition = new Vector3(0, 0, distance);
            var newSegment = AddSegment(prefab, newSegmentPosition);

            if (isFinish)
            {
                EventManager.Invoke(MainEnumEvent.SetFinishPoint, newSegment.Origin);
            }
        }
        
        private GameElement GetNextSegment(int index)
        {
            var nextSegment = SegmentsData.RandomOrder ? SegmentsData.All.RandomElement() : SegmentsData.All[index];
            return nextSegment;
        }

        private void AddFirstSegment() => AddSegment(_data.Segments.Start, Vector3.zero);

        private LevelSegmentBase AddSegment(LevelSegmentBase prefab, Vector3 position)
        {
            var segment = _factory.Create(prefab, position, Quaternion.identity, _parent);
            _segmentsInGame ??= new List<LevelSegmentBase>();
            _segmentsInGame.Add(segment);

            return segment;
        }
        
        private void TryRemoveOldest()
        {
            var firstSegment = _segmentsInGame.FirstOrDefault();

            bool NeedDestroyOldest(Transform oldest)
            {
                var destroyPosition = oldest.position + _destroyDistance * Vector3.forward;
                var maxPosition = FocusPoint.position.z;
                return destroyPosition.z < maxPosition;
            }

            if (firstSegment && NeedDestroyOldest(firstSegment.transform))
            {
                RemoveSegment(firstSegment);
            }
        }

        private void RemoveSegment(LevelSegmentBase segment)
        {
            _segmentsInGame.Remove(segment);
            Destroy(segment.gameObject);
        }
    }
}