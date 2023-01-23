using System;

namespace NavySpade.Meta.Runtime.Quests.LifeTimeTypes
{
    [Serializable]
    [AddTypeMenu("No time limit")]
    public class Infinity : ILifeTime
    {
        public float Progress => 0;
    }
}