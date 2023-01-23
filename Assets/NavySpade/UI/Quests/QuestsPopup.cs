using Core.Meta;
using Core.UI.Popups.Abstract;
using NavySpade.Modules.Meta.Runtime.Quests.Configuration;
using NavySpade.UI.Popups.Abstract;
using UnityEngine;

namespace Core.UI.Quests
{
    public class QuestsPopup : Popup
    {
        public const string PREFS_KEY_DAILY = "Meta.QuestsRefushTime.Daily";
        public const string PREFS_KEY_WEEKLY = "Meta.QuestsRefushTime.Weekly";
        
        [SerializeField] private QuestsSection _dailySecton;
        [SerializeField] private QuestsSection _weeklySecton;

        private RealTimeTimer _dailyTimer;
        private RealTimeTimer _weeklyTimer;

        
        public override void OnAwake()
        {
            var config = QuestsConfig.Instance;
            
            _dailySecton.Init(PREFS_KEY_DAILY, config.DailyQuests);
            _weeklySecton.Init(PREFS_KEY_WEEKLY, config.WeeklyQuests);
        }

        public override void OnStart()
        {
        }
    }
}