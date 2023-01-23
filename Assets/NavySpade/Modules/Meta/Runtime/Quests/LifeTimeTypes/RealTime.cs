using System;

namespace NavySpade.Meta.Runtime.Quests.LifeTimeTypes
{
    [Serializable]
    [AddTypeMenu("Real time")]
    public class RealTime : CountedLifeTime
    {
        public override ulong InitStartTime()
        {
            return (ulong)DateTime.Now.Ticks;
        }

        public override void Tick(float dt)
        {
            CurrentTicks = (ulong)DateTime.Now.Ticks;
        }
    }
}