using TMPro;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    public class CounterView : MonoBehaviour{
        [SerializeField]
        private TextMeshProUGUI _healthPoints;
        public int FormatHp{
            set => _healthPoints.text = value.ToString();
        }
    }
}