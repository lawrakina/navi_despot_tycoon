using System.Collections;
using Cinemachine;
using NavySpade.pj49.Scripts.BaseFunctionsFromMVC;
using ThirdParty.Joystick_Pack.Scripts.Integration;
using UniRx;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    public class GameEndViewController : BaseMonoController{
        [SerializeField]
        private JoystickSettings _joystick;
        [SerializeField]
        private CinemachineVirtualCamera _endGameCamera;
        [SerializeField]
        private GameObject _winZone;
        [SerializeField]
        private GameObject _blockedZone;
        [SerializeField]
        private ArenaModel _model;

        public void Init(ArenaModel arenaModel){
            _model = arenaModel;
            _model.OnBossDie.Subscribe(OnBossDie).AddTo(_subscriptions);
            LockExit(true);
        }

        private void OnBossDie(bool die){
            if (!die) return;
           LockInput(true);
            StartCoroutine(WaitEndAnimation());
        }

        private IEnumerator WaitEndAnimation(){
            yield return new WaitForSeconds(2);
            _model.OnShowKey.Value = true;
            yield return new WaitForSeconds(3);
            LockInput(false);
            LockExit(false);
        }

        private void LockExit(bool state){
            _winZone.SetActive(!state);
            _blockedZone.SetActive(state);
        }

        private void LockInput(bool state){
            JoystickInputProvider.Instance.enabled = !state;
            _endGameCamera.gameObject.SetActive(state);
        }
    }
}