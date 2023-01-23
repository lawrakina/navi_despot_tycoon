using System;
using NavySpade.pj49.Scripts.Productions.Factory;
using UnityEngine;

namespace Main.UI
{
    public class ProductionTimerUI : MonoBehaviour
    {
        [SerializeField] private TimerUI _timer;
        [SerializeField] private Factory _factory;

        private GameObject _timerGO;
        private bool _productionStarted;
        
        private void Start()
        {
            _factory.ProductionStarted += ShowTimer;
            _timerGO = _timer.gameObject;
            _timerGO.SetActive(false);
        }

        private void OnDestroy()
        {
            _factory.ProductionStarted -= ShowTimer;
        }
        
        private void ShowTimer()
        {
            _timerGO.SetActive(true);
            _productionStarted = true;
        }

        private void Update()
        {
            if(_productionStarted == false)
                return;
            
            float timeLeft = _factory.Stats.ProductionTime - _factory.ElapsedTimeOfProduction;
            if (timeLeft <= 0)
            {
                _timerGO.SetActive(false);
            }
            else
            {
                _timerGO.SetActive(true);
                _timer.UpdateTimer(timeLeft);
            }
        }
    }
}