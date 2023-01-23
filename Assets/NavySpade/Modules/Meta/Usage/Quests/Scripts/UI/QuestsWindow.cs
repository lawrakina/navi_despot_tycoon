using System.Collections;
using System.Collections.Generic;
using Core.Meta.Quests;
using UnityEngine;

namespace NavySpade.Meta.Usage.Quests.Scripts.UI
{
    public class QuestsWindow : MonoBehaviour
    {
        public QuestPanel Prefab;
        public Transform Parent;
        public float _updateDelay = 0.9f;
        
        private List<QuestPanel> _questPanels;

        private static float _previewTickTime;

        private void Awake()
        {
            _questPanels = new List<QuestPanel>(QuestManager.QuestsData.Count);
            UpdatePanels();
        }

        private void OnEnable()
        {
            StartCoroutine(QuestsTick());
            
            QuestManager.AnyQuestDataChanged += QuestManagerOnAnyQuestDataChanged;
        }

        private void OnDisable()
        {
            QuestManager.AnyQuestDataChanged -= QuestManagerOnAnyQuestDataChanged;
        }
        
        private void QuestManagerOnAnyQuestDataChanged()
        {
            UpdatePanels();
        }

        private void UpdatePanels()
        {
            int i;
            
            for (i = 0; i < QuestManager.QuestsData.Count; i++)
            {
                if (i >= _questPanels.Count)
                {
                    _questPanels.Add(Instantiate(Prefab, Parent));
                }
                
                _questPanels[i].UpdateData(QuestManager.QuestsData[i]);
            }

            for (; i < _questPanels.Count; i++)
            {
                _questPanels[i].UpdateData(null);
            }
        }

        private IEnumerator QuestsTick()
        {
            QuestManager.IsAutoSave = true;
            while (true)
            {
                yield return new WaitForSeconds(_updateDelay);
                
                QuestManager.Tick(Time.time - _previewTickTime);
                _previewTickTime = Time.time;
            }
        }
    }
}