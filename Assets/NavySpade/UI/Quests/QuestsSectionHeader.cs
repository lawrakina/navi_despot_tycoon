using TMPro;
using UnityEngine;

namespace Core.Meta.Quests
{
    public class QuestsSectionHeader : MonoBehaviour
    {
        [SerializeField] private TMP_Text _time;
        
        [SerializeField] private string _sectionHeaderTextFormat = "{0}:{1}";


        public void SectionUpdate(RealTimeTimer timer, bool isAnyQuestReceived)
        {
            var timeSpan = timer.RemainingTime;
            var allHours = Mathf.FloorToInt((float) timeSpan.TotalHours);
            _time.text = string.Format(_sectionHeaderTextFormat, allHours, timeSpan.Minutes);
        }
    }
}