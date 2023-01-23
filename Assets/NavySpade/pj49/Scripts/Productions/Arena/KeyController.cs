using System;
using System.Collections;
using System.Collections.Generic;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.BaseFunctionsFromMVC;
using UniRx;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    public class KeyController : BaseMonoController{
        [SerializeField]
        private ZonePickUpKey _zonePickUpKey;
        [SerializeField]
        private Transform _placeForKey;

        [SerializeField]
        private Transform _targetForLookAtOnEndGame;
        private Transform _target;


        #region PrivateData

        private ArenaModel _model;
        private KeyView _keyPrefab;

        private bool _isOn = false;
        private KeyView _key;
        private Vector3 _offset = new Vector3(0, 4, -3);
        private KeyStage _keyStage;
        private readonly int _isOpenHash = Animator.StringToHash("IsOpen");
        [SerializeField]
        private Animator _barrier;
        private bool _isCompleted = false;

        #endregion


        public void Init(ArenaModel model, KeyView keyPrefab){
            _model = model;
            _keyPrefab = keyPrefab;

            _zonePickUpKey.gameObject.SetActive(false);
            _zonePickUpKey.EnterPlayer.Subscribe(OnStartKeyFrame).AddTo(_subscriptions);
            _model.OnShowKey.Subscribe(OnShowKeyFrame).AddTo(_subscriptions);
            _target = FindObjectOfType<SinglePlayer>().transform;
        }

        private void Awake(){
            
            _target = FindObjectOfType<SinglePlayer>().transform;
        }

        public void SetTarget(Transform target, Vector3 offset, KeyStage keyStage, float yRotation = default){
            _target = target;
            _offset = offset;
            _keyStage = keyStage;
            if (_keyStage == KeyStage.Keyhole){
                StartCoroutine(RotateKey(yRotation));
            }
        }

        private IEnumerator RotateKey(float yRotation){
            if (_isCompleted) yield break;
            // _key.transform.rotation = rootRotation;
            yield return new WaitForSeconds(0.5f);
            
            _key.transform.LookAt(_targetForLookAtOnEndGame);
            // _key.DefaultRotate(yRotation);
            float time = 3;
            while (time > 0){
                yield return new WaitForEndOfFrame();
                time -= Time.deltaTime;
                var angles = _key.transform.localRotation.eulerAngles;
                angles.z += Time.deltaTime * 200;
                _key.transform.localRotation = Quaternion.Euler(angles);
            }
            
            _barrier.SetBool(_isOpenHash, true);
            _isCompleted = true;
        }

        private void OnStartKeyFrame(bool die){
            if (!die) return;
            _isOn = true;
            SetTarget(_target, new Vector3(0,3,0), KeyStage.MoveToPlayer);
        }

        private void Update(){
            if (!_isOn) return;
            switch (_keyStage){
                case KeyStage.Show:
                    _key.transform.position = Vector3.Lerp(_key.transform.position,
                        new Vector3(
                            _target.position.x + _offset.x,
                            _target.position.y + _offset.y,
                            _target.position.z + _offset.z),
                        2* Time.deltaTime);
                    break;
                case KeyStage.MoveToPlayer:
                    _key.transform.position = new Vector3(
                        _target.position.x + _offset.x,
                        _target.position.y + _offset.y,
                        _target.position.z + _offset.z);
                    break;
                case KeyStage.Keyhole:
                    _key.transform.position = Vector3.Lerp(_key.transform.position,
                        new Vector3(
                            _target.position.x + _offset.x,
                            _target.position.y + _offset.y,
                            _target.position.z + _offset.z),
                        5 * Time.deltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnShowKeyFrame(bool show){
            if (!show) return;
            _zonePickUpKey.gameObject.SetActive(true);
            _key = Instantiate(_keyPrefab, _placeForKey, true);
            _key.transform.localPosition = _offset;
            _keyStage = KeyStage.Show;
            _target = FindObjectOfType<SinglePlayer>().transform;
            StartCoroutine(Enable(true));
        }

        private IEnumerator Enable(bool state){
            yield return new WaitForSeconds(3);
            _isOn = state;
            yield return new WaitForSeconds(1);
            OnStartKeyFrame(state);
        }
    }

    public enum KeyStage{
        Show,
        MoveToPlayer,
        Keyhole
    }
}