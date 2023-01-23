using System;
using UnityEngine;

namespace NavySpade.Meta.Runtime.Quests.LifeTimeTypes
{
    [Serializable]
    [AddTypeMenu("Counted Life Time")]
    public abstract class CountedLifeTime : ILifeTime
    {
        [field: SerializeField] public uint Seconds { get; private set; }

        public ulong StartTicks { get; set; }
        public ulong CurrentTicks { get; set; }
        public ulong ElapsedTicks => CurrentTicks - StartTicks;
        public ulong RemainingTicks => SecondsInTicks - ElapsedTicks;
        public ulong SecondsInTicks => (ulong) TimeSpan.FromSeconds(Seconds).Ticks;

        public float Progress => (float) ElapsedTicks / SecondsInTicks;

        public abstract ulong InitStartTime();

        public abstract void Tick(float dt);
    }
}