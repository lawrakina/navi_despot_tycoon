using System;
using System.Collections.Generic;
using NaughtyAttributes;
using NavySpade.Modules.Extensions.CsharpTypes;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Generation.Segments
{
    [Serializable]
    public class LevelSegmentsData
    {
        [field: SerializeField] public bool RandomOrder { get; private set; } = true;

        [Required, SerializeField] private List<GameElement> _startSegments;
        [SerializeField] private List<GameElement> _finishSegments;
        [SerializeField] private List<GameElement> _intermediateSegments;

        public GameElement Start => _startSegments.RandomElement();
        public GameElement Finish => _finishSegments.RandomElement();
        public List<GameElement> All => _intermediateSegments;
    }
}