using NavySpade.Meta.Runtime.Analytics;
using NavySpade.Meta.Runtime.Economic.Rewards.Interfaces;
using NavySpade.Meta.Runtime.Quests.CompletionTypes;
using NavySpade.Meta.Runtime.Quests.LifeTimeTypes;
using UnityEngine;

namespace NavySpade.Meta.Runtime.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Meta/Quests/Quest", order = 51)]
    public class Quest : ScriptableObject
    {
        [field: SerializeField] public string HeaderName { get; private set; }
        [field: TextArea, SerializeField] public string Description { get; private set; }

        [field: SerializeReference, SubclassSelector]
        public ICompleteType CompletionType { get; private set; }

        [field: SerializeField] public TrackingVariable AttachedVariable { get; private set; }

        [field: SerializeReference, SubclassSelector]
        public IReward Reward { get; private set; }

        [field: SerializeReference, SubclassSelector]
        public ILifeTime LifeTime { get; private set; }
    }
}