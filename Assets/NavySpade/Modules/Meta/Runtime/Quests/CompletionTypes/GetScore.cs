using System;
using Core.Meta.Analytics;
using UnityEngine;

namespace NavySpade.Meta.Runtime.Quests.CompletionTypes
{
    [Serializable]
    [AddTypeMenu("Earn Score")]
    public class GetScore : ICompleteType
    {
        [Tooltip("true - будет считать полученную валюту, иначе сколько игрок потратил очков")]
        public bool IsPositiveOrNegative = true;
        [SerializeField] private float _targetScore;

        public float TargetScore => _targetScore;

        public float StartValue(VariableData startData, VariableData currentData)
        {
            return 0f;
        }

        float ICompleteType.Value(VariableData startData, VariableData currentData)
        {
            var startValue = IsPositiveOrNegative ? startData.AddValue : startData.ReducedValue;
            var currentValue = IsPositiveOrNegative ? currentData.AddIntValue : currentData.ReducedValue;

            return currentValue - startValue;
        }

        float ICompleteType.Progress(VariableData startData, VariableData currentData)
        {
            var startValue = IsPositiveOrNegative ? startData.AddValue : startData.ReducedValue;
            var currentValue = IsPositiveOrNegative ? currentData.AddIntValue : currentData.ReducedValue;

            return (currentValue - startValue) / _targetScore;
        }
    }
}