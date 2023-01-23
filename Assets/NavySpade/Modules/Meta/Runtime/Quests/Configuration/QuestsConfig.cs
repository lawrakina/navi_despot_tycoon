using System;
using NavySpade.Meta.Runtime.Quests;
using NavySpade.Modules.Configuration.Runtime.Attributes;
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace NavySpade.Modules.Meta.Runtime.Quests.Configuration
{
    [ConfigGroup(MetaConstants.ModuleName)]
    public class QuestsConfig : ObjectConfig<QuestsConfig>
    {
        [Serializable]
        public class QuestsTypeInfo
        {
            [field: SerializeField] public Quest[] Quests { get; private set; }
            [field: SerializeField] public uint RefusedTimeSeconds { get; private set; } = 60 * 60 * 24;
            [field: SerializeField] public uint AddedQuestCount { get; private set; }
        }

        [field: SerializeField] public QuestsTypeInfo DailyQuests { get; private set; }
        [field: SerializeField] public QuestsTypeInfo WeeklyQuests { get; private set; }


        [field: Tooltip("Каждые n секунд обновляет данные для квестов")]
        [field: SerializeField]
        public float UpdateQuestsTime { get; private set; } = .9f;

        [field: SerializeField] public string ScriptablesResourcesPath { get; private set; } = "Quests";
    }
}