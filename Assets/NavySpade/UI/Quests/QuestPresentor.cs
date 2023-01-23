using Core.Meta.Quests;
using Core.UI.Economic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Quests
{
    public class QuestPresentor : MonoBehaviour
    {
        [SerializeField] private TMP_Text _headerText;
        [SerializeField] private Image _icon;
        [SerializeField] private QuestProgressBar _progressBar;
        [SerializeField] private RewardPresentor pricePresentor;

        private QuestData _data;

        public void UpdateView(QuestData data)
        {
            _data = data;
            
            _headerText.text = data.Scriptable.HeaderName;
            //_icon.sprite = ??? TODO icon
            _progressBar.UpdateView(Mathf.FloorToInt(data.Value), Mathf.FloorToInt(data.EndValue));
            pricePresentor.UpdateView(data.Scriptable.Reward);
        }

        public void EarnRewardButton()
        {
            if(_data.IsCompleted == false || _data == null)
                return;
            
            _data.Scriptable.Reward.TakeReward();
            QuestManager.RemoveQuest(_data);
        }
    }
}