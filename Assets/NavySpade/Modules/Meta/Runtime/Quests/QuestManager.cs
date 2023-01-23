using System;
using System.Collections.Generic;
using NavySpade.Meta.Runtime.Quests;
using NavySpade.Meta.Runtime.Quests.LifeTimeTypes;
using NavySpade.Modules.Saving.Runtime;
using UnityEngine;

namespace Core.Meta.Quests
{
    public static class QuestManager
    {
        private const string QUESTS_COUNT_KEY = "Quests.Count";
        private const string QUESTS_KEY = "Quests.";

        private static List<QuestData> _questsData;
        private static List<int> _questIndexesForRemove;

        public static event Action<QuestData, int> CompletionTimeOut;
        public static event Action<QuestData> QuestReceived;
        public static event Action<QuestData, int> QuestRemoved;
        public static event Action AnyQuestDataChanged;

        public static bool IsAutoSave = true;

        public static List<QuestData> QuestsData
        {
            get
            {
                if (_questsData == null)
                    LoadQuests();

                return _questsData;
            }
            private set => _questsData = value;
        }

        public static void LoadQuests()
        {
            var count = SaveManager.Load<int>(QUESTS_COUNT_KEY);
            QuestsData = new List<QuestData>(count);

            for (int i = 0; i < count; i++)
            {
                var obj = SaveManager.Load<QuestData>($"{QUESTS_KEY}{i}");

                if (obj.Scriptable.LifeTime is CountedLifeTime countedLifeTime)
                {
                    countedLifeTime.StartTicks = obj.StartTime;
                    countedLifeTime.CurrentTicks = obj.Time;
                }

                QuestsData.Add(obj);
            }
        }

        public static void GiveQuest(Quest quest)
        {
            if (quest == null)
                throw new ArgumentException($"quest is null");

            ulong startTime = 0;
            if (quest.LifeTime is CountedLifeTime countedLifeTime)
            {
                startTime = countedLifeTime.InitStartTime();
                countedLifeTime.StartTicks = startTime;
                countedLifeTime.CurrentTicks = startTime;
            }

            var data = new QuestData(quest, startTime);

            QuestsData.Add(data);
            SaveManager.Save(QUESTS_COUNT_KEY, _questsData.Count);
            SaveManager.Save($"{QUESTS_KEY}{_questsData.Count - 1}", data);

            QuestReceived?.Invoke(data);
            AnyQuestDataChanged?.Invoke();
        }


        /// <summary>
        /// не обязательно вызывать каждый кадр, можно вполне это делать каждую секунду.
        /// так как при включённом автосейве, вызов этого метода будет менять плеер префсы
        /// </summary>
        /// <param name="dt"></param>
        public static void Tick(float dt)
        {
            if(_questIndexesForRemove == null)
                _questIndexesForRemove = new List<int>(10);
            
            _questIndexesForRemove.Clear();
            
            for (var i = 0; i < QuestsData.Count; i++)
            {
                var questData = QuestsData[i];
                var lifeTime = questData.Scriptable.LifeTime;

                if (lifeTime is CountedLifeTime countedLifeTime)
                {
                    countedLifeTime.Tick(dt);
                    questData.Time = countedLifeTime.CurrentTicks;
                }

                //quest time out
                if (questData.Scriptable.LifeTime.Progress >= 1)
                {
                    _questIndexesForRemove.Add(i);
                    CompletionTimeOut?.Invoke(questData, i);
                }
            }

            if (_questIndexesForRemove.Count > 0)
            {
                foreach (var index in _questIndexesForRemove)
                {
                    _questsData.RemoveAt(index);
                }
            }

            if (IsAutoSave)
                SaveQuestsData();
            
            AnyQuestDataChanged?.Invoke();
        }

        /// <summary>
        /// если квест выполнен, то нужно вызвать этот метод
        /// </summary>
        /// <param name="data"></param>
        public static void RemoveQuest(QuestData data)
        {
            var index = _questsData.IndexOf(data);

            QuestRemoved?.Invoke(data, index);

            _questsData.RemoveAt(index);
            
            AnyQuestDataChanged?.Invoke();

            if (IsAutoSave)
                SaveQuestsData();
        }

        public static void SaveQuestsData()
        {
            SaveManager.Save(QUESTS_COUNT_KEY, _questsData.Count);

            for (var i = 0; i < _questsData.Count; i++)
            {
                SaveManager.Save($"{QUESTS_KEY}{i}", _questsData[i]);
            }
        }
    }
}