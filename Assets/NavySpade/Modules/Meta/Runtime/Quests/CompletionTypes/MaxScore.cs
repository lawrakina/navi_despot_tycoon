using System;
using Core.Meta.Analytics;
using UnityEngine;

namespace NavySpade.Meta.Runtime.Quests.CompletionTypes
{
    [Serializable]
    [AddTypeMenu("Max Score")]
    public class MaxScore : ICompleteType
    {
        [SerializeField] private float _targetScore;
        [Tooltip("если true то макс счёт будет = макс счёт на момент получения квеста + Target Score \nиначе = 0 + Target Score")]
        [SerializeField] private bool _isAttachedToStartScore;

        public float TargetScore => _targetScore;

        public float StartValue(VariableData startData, VariableData currentData)
        {
            var startValue = _isAttachedToStartScore ? startData.MaxValue : 0;
            return startValue;
        }

        float ICompleteType.Value(VariableData startData, VariableData currentData)
        {
            var startValue = _isAttachedToStartScore ? startData.MaxValue : 0;

            return currentData.MaxValue - startValue;
        }

        float ICompleteType.Progress(VariableData startData, VariableData currentData)
        {
            var startValue = _isAttachedToStartScore ? startData.MaxValue : 0;
            
            return (currentData.MaxValue - startValue) / _targetScore;
        }
    }
}