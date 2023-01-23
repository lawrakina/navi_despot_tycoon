using System.Collections;
using NavySpade.Modules.Meta.Runtime.Quests.Configuration;
using UnityEngine;

namespace Core.Meta.Quests
{
    internal class QuestsUpdater : MonoBehaviour
    {
        private static QuestsConfig Config => QuestsConfig.Instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateUpdater()
        {
            if(MetaGameConfig.Instance.EnableQuests == false)
                return;
            
            var obj = new GameObject("Quests Updater").AddComponent<QuestsUpdater>();
            DontDestroyOnLoad(obj);
        }

        private void OnEnable()
        {
            StartCoroutine(UpdateQuests());
        }

        private IEnumerator UpdateQuests()
        {
            var time = 0f;
            while (true)
            {
                yield return new WaitForSeconds(Config.UpdateQuestsTime);
                QuestManager.Tick(Time.time - time);
                time = Time.time;
            }
        }
    }
}