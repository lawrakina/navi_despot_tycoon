using System;
using UnityEngine;

namespace NavySpade.Meta.Runtime.Quests.LifeTimeTypes
{
    [Serializable]
    [AddTypeMenu("In game time")]
    public class InGameTime : CountedLifeTime
    {
        private float _time;

        public override ulong InitStartTime()
        {
            return 0;
        }

        public override void Tick(float dt)
        {
            _time += dt;

            var floar = Mathf.FloorToInt(_time);
            _time -= floar;
            CurrentTicks += (ulong)TimeSpan.FromSeconds(floar).Ticks;
        }
    }
}