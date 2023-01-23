using NavySpade.Meta.Runtime.Analytics;
using TMPro;
using UnityEngine;

namespace NavySpade.Meta.Usage.Quests.Scripts
{
    public class WolfKiller : MonoBehaviour
    {
        [SerializeField] private TMP_Text _KillBar;
        private int _wolfsKills;

        private void Start()
        {
            _KillBar.text = _wolfsKills.ToString();
        
            VariableTracker.UpdateValue("wolfs kills", 0);
        }

        public void KillWolf()
        {
            _wolfsKills++;
            _KillBar.text = _wolfsKills.ToString();
        
            VariableTracker.UpdateValue("wolfs kills", _wolfsKills);
        }
    }
}
