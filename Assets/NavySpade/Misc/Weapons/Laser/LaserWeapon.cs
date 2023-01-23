using System;
using System.Collections;
using Core.Damagables;
using Misc.Damagables;
using Misc.Damagables.Effects;
using UnityEngine;

namespace Project19.Weapons.Laser
{
    public class LaserWeapon : WeaponBase
    {
        [SerializeField] private GameObject _laserObj;
        [SerializeField] private bool _isMirrorLaser;
        [SerializeField] private float _endPointOffset;

        [SerializeField] private LineRenderer[] _lineRenderers;
        [SerializeField] private GameObject _endPoint;

        private static RaycastHit[] _results = new RaycastHit[50];

        private Vector3 _direction;

        public override StatBase BaseStats => (AttachedSpell.Data as LaserData).Stats;

        protected override Func<int> SetStartBulletCount => () => BaseStats.BulletCount;

        protected override void OnStartUse_Internal()
        {
            StartCoroutine(DamageTick());
            OnEndShot();
        }

        protected override void OnStartShot()
        {
            _laserObj.SetActive(true);
        }

        protected override void OnEndShot()
        {
            _laserObj.SetActive(false);
        }

        private void Update()
        {
            if (IsShot == false)
                return;

            //TODO: nearest enemy find
            //var nearestEnemy = Enemy.All.FindClosed(transform.position);

            /*
            if (nearestEnemy == null)
            {
                UpdateLaserSettings(BaseStats.CircularDistance);
                return;
            }
            else
            {
                UpdateLaserSettings(Vector3.Distance(transform.position, nearestEnemy.transform.position));
            }

            var dir = nearestEnemy.transform.position - transform.position;
            dir.Normalize();

            _direction = dir;
            */

            _laserObj.transform.forward = _isMirrorLaser ? -_direction : _direction;
            _laserObj.transform.localScale = Vector3.one * BaseStats.ScaleMultiplay;
        }

        private IEnumerator DamageTick()
        {
            while (true)
            {
                yield return new WaitForSeconds(BaseStats.ShotDelay);

                if (IsShot)
                    ThrowDamage();
            }
        }

        
        /// <summary>
        /// луч будет иметь макисмальную дистанцию
        /// </summary>
        /// <param name="distance"></param>
        public void UpdateLaserSettings(float distance)
        {
            foreach (var lineRenderer in _lineRenderers)
            {
                lineRenderer.SetPosition(0, Vector3.back * distance);
            }
    
            _endPoint.transform.localPosition = Vector3.back * (distance + _endPointOffset);
        }

        private void ThrowDamage()
        {
            var ray = new Ray(transform.position, _direction);
            var count = Physics.RaycastNonAlloc(ray, _results);

            for (int i = 0; i < count; i++)
            {
                var damagable = _results[i].transform.GetComponent<Damageable>();
                
                if(damagable == null)
                    continue;

                var dir = damagable.transform.position - transform.position;
                dir.Normalize();
                
                damagable.DealDamage(BaseStats.Damage, CurrentTeam, new RepulsionParameter(dir, BaseStats.RagdollVelocity));
                break;
            }
            
            BulletCount--;
        }
    }
}