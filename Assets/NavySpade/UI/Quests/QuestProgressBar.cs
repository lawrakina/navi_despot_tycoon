using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Quests
{
    public class QuestProgressBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private string _progressTextFormat = "{0}/{1}";
        [SerializeField] private string _completedText = "Collect!";

        [SerializeField] private GameObject _completed;
        [SerializeField] private Image _filling;
        
        public void UpdateView(int currentValue, int targetValue)
        {
            _completed.SetActive(currentValue >= targetValue);
            
            if (currentValue < targetValue)
            {
                _progressText.text = string.Format(_progressTextFormat, currentValue, targetValue);
            }
            else
            {
                _progressText.text = _completedText;
            }

            _filling.fillAmount = (float) currentValue / targetValue;
        }
    }
}