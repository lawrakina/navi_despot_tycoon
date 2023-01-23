using Core.Meta.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NavySpade.Meta.Usage.Quests.Scripts.UI
{
    public class QuestPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _questNameText;
        [SerializeField] private TMP_Text _questDescriptionText;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TMP_Text _startValueText;
        [SerializeField] private TMP_Text _currentValueText;
        [SerializeField] private TMP_Text _endValueText;
        [SerializeField] private TMP_Text _remainingTimeText;
        [SerializeField] private GameObject _earnRewardButton;

        private QuestData _data;
        
        public void UpdateData(QuestData data)
        {
            gameObject.SetActive(data != null);
            
            if(data == null)
                return;
            
            _data = data;

            _questNameText.text = data.Scriptable.HeaderName;
            _questDescriptionText.text = data.Scriptable.Description;
            _progressSlider.value = data.Progress;

            _startValueText.text = data.StartValue.ToString();
            _currentValueText.text = data.Value.ToString();
            _endValueText.text = data.EndValue.ToString();

            if (data.TryGetRemainingTime(out var time))
            {
                _remainingTimeText.text = $"{Mathf.Floor((float)time.TotalHours)}:{time.Minutes}:{time.Seconds}";
            }
            else
            {
                _remainingTimeText.text = "infinity";
            }
            
            _earnRewardButton.SetActive(data.IsCompleted);
        }

        public void CompleteQuest()
        {
            if(_data.IsCompleted == false)
                return;
            
            _data.Scriptable.Reward.TakeReward();
            QuestManager.RemoveQuest(_data);
        }
    }
}