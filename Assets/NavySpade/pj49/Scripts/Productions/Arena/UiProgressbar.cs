using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    public class UiProgressbar : MonoBehaviour{
        public Image fill;
        public TMP_Text Text;
        public string LimitedResourceFormat = "{0}/{1}";

        [HideInInspector]
        public float maxPoints;
        [HideInInspector]
        public float curPoints;

        void Start(){
            if (maxPoints != default(float) && curPoints != default(float)) SetupProgressbar(maxPoints, curPoints);
        }

        public void SetupProgressbar(float _maxPoints, float _curPoints = -1){
            if (maxPoints != _maxPoints){
                maxPoints = _maxPoints;

                if (_curPoints == -1) curPoints = maxPoints;
                else curPoints = _curPoints;

                UpdateProgressbar(curPoints);
            }
        }

        public void UpdateProgressbar(float points){
            if (maxPoints == 0)
                return;

            // if (curPoints <= 0)
            // {
            //     gameObject.SetActive(false);
            // }
            // else
            // {
            //     gameObject.SetActive(true);
            // }

            curPoints = points;
            fill.fillAmount = curPoints / maxPoints;
            Text.text = string.Format(LimitedResourceFormat, curPoints, maxPoints);
        }
    }
}