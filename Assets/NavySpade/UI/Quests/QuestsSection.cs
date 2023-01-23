using System.Collections.Generic;
using System.Linq;
using Core.Meta;
using Core.Meta.Quests;
using NavySpade.Meta.Runtime.Quests;
using NavySpade.Modules.Extensions.CsharpTypes;
using NavySpade.Modules.Meta.Runtime.Quests.Configuration;
using UnityEngine;

namespace Core.UI.Quests
{
    public class QuestsSection : MonoBehaviour
    {
        [SerializeField] private QuestsSectionHeader _header;
        [SerializeField] private QuestPresentor[] _questPresentors;

        private RealTimeTimer _timer;
        private QuestsConfig.QuestsTypeInfo _info;
        
        private bool IsAnyQuestReceived { get; set; }

        public void Init(string timerKey, QuestsConfig.QuestsTypeInfo info)
        {
            _timer = new RealTimeTimer(timerKey, info.RefusedTimeSeconds);
            _info = info;

            UpdateData();
            QuestManager.AnyQuestDataChanged += UpdateData;
        }

        private void OnDisable()
        {
            QuestManager.AnyQuestDataChanged -= UpdateData;
        }

        private void UpdateData()
        {
            RequestToResetQuests();
            UpdateQuests();
            
            _header.SectionUpdate(_timer, IsAnyQuestReceived);
        }

        private void RequestToResetQuests()
        {
            if (_timer.IsTimeOut() == false)
            {
                return;
            }

            _timer.ResetTimerToNow();

            var questsForRemove = new Queue<QuestData>((int) _info.AddedQuestCount);

            // O(N^2) and its may take a loooong time if we have a large array
            foreach (var questData in QuestManager.QuestsData)
            {
                if (_info.Quests.Contains(questData.Scriptable))
                {
                    questsForRemove.Enqueue(questData);
                }
            }

            while (questsForRemove.Count > 0)
            {
                var data = questsForRemove.Dequeue();

                QuestManager.RemoveQuest(data);
            }

            uint count;
            if (_info.Quests.Length <= _info.AddedQuestCount)
            {
                UnityEngine.Debug.LogWarning("массив квестов меньше чем количество добавляемых квестов");
                count = (uint) _info.Quests.Length;
            }
            else
            {
                count = _info.AddedQuestCount;
            }

            var array = (Quest[]) _info.Quests.Clone();
            array.Shuffle();

            for (var i = 0; i < count; i++)
            {
                QuestManager.GiveQuest(array[i]);
            }
        }

        private void UpdateQuests()
        {
            var datasForQuests =
                QuestManager.QuestsData.Where(data => _info.Quests.Contains(data.Scriptable)).ToArray();

            int i;
            for (i = 0; i < datasForQuests.Length; i++)
            {
                if (i >= _questPresentors.Length)
                    continue;

                var quest = _questPresentors[i];

                if (quest.gameObject.activeSelf == false)
                    quest.gameObject.SetActive(true);

                quest.UpdateView(datasForQuests[i]);

                IsAnyQuestReceived = false;
            }

            for (; i < _questPresentors.Length; i++)
            {
                var quest = _questPresentors[i];

                if (quest.gameObject.activeSelf)
                    quest.gameObject.SetActive(false);

                IsAnyQuestReceived = true;
            }
        }
    }
}