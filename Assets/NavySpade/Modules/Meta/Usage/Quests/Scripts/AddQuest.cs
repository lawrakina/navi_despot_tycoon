using Core.Meta.Quests;
using NavySpade.Meta.Runtime.Quests;
using UnityEngine;

namespace NavySpade.Meta.Usage.Quests.Scripts
{
    public class AddQuest : MonoBehaviour
    {
        public Quest SO;

        public void Give()
        {
            QuestManager.GiveQuest(SO);
        }
    }
}