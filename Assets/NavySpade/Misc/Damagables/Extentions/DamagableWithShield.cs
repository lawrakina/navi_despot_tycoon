using System.Collections;
using Core.Damagables;
using NavySpade.Core.Runtime.Levels;
using UnityEngine;

namespace Misc.Damagables
{
    public class DamagableWithShield : Damageable
    {
        [SerializeField] private GameObject _shieldVisual;
        [SerializeField] private float _duration;

        private Coroutine _shieldTimer;

        protected override void OnAwake()
        {
            _shieldVisual.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            LevelManager.LevelLoaded += ResetData;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LevelManager.LevelLoaded -= ResetData;
        }

        private void ResetData()
        {
            _shieldVisual.SetActive(false);
            
            if (_shieldTimer != null)
            {
                StopCoroutine(_shieldTimer);
                _shieldTimer = null;
            }
        }

        public override void DealDamage(int value, Team team, params IDamageParameter[] damageParameters)
        {
            if (_shieldTimer != null)
                return;

            value = 1;

            base.DealDamage(value, team, damageParameters);

            if(IsAlive)
                _shieldTimer = StartCoroutine(ShieldTimer());
        }

        private IEnumerator ShieldTimer()
        {
            _shieldVisual.SetActive(true);
            
            yield return new WaitForSeconds(_duration);
            
            _shieldVisual.SetActive(false);
            _shieldTimer = null;
        }
    }
}