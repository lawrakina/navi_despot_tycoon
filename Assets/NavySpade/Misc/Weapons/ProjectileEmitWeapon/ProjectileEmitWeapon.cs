using System;
using System.Collections;
using Core.Damagables;
using Extensions;
using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;

namespace Project19.Weapons.ProjectileEmitWeapon
{
    public abstract class ProjectileEmitWeapon<T, U> : WeaponBase where T : ProjectileBase where U : StatBase
    {
        [SerializeField] private T _prefab;


        private bool _isReloading;
        private Coroutine _reloadRoutine;

        public bool CanShot => _isReloading == false && IsShot;
        public bool IsAutoShot { get; set; } = true;

        public override StatBase BaseStats => Stat;

        protected abstract U Stat { get; }
        protected override Func<int> SetStartBulletCount => () => Stat.BulletCount;

        private IterationFilter<Damageable> _findEnemyFilter;

        private IterationFilter<Damageable> FindEnemyFilter => _findEnemyFilter ??= new IterationFilter<Damageable>().Where(d => d.IsAlive == false || Bound.Contains(d.transform.position) == false);
        
        private Bounds Bound => new Bounds(transform.position, new Vector3(Stat.Distance.x, 100, Stat.Distance.y));

        public T Prefab => _prefab;

        private void FixedUpdate()
        {
            if (IsAutoShot)
            {
                RequestShot();
            }
        }

        public void RequestShot()
        {
            if (CanShot && _reloadRoutine == null)
            {
                _reloadRoutine = StartCoroutine(ShotReload());
                Shot();
            }
        }

        private IEnumerator ShotReload()
        {
            _isReloading = true;

            yield return new WaitForSeconds(Stat.ShotDelay);

            _isReloading = false;
            _reloadRoutine = null;
        }

        private void Shot()
        {
            var target = Damageable.GetAllEnemysOfTeam(CurrentTeam).FindClosed(transform.position, FindEnemyFilter);
            
            if(target == null)
                return;
            
            var dir = target.transform.position - transform.position;
            var dir2D = new Vector2(dir.x, dir.z);
            
            Shot(dir2D.normalized, target.transform);
            InvokeShotEvent();
        }

        public virtual void Shot(Vector2 direction, Transform target)
        {
            var obj = Instantiate(_prefab, transform.position, Quaternion.identity);

            var dir = new Vector3(direction.x, 0, direction.y);
            _dir = dir;
            
            obj.transform.localScale *= Stat.ScaleMultiplay;
            obj.transform.forward = dir;
          //  obj.Rb.velocity = dir;
            
            obj.InitBase(CurrentTeam, Stat, AttachedSpell.Data, target);
            OnEmitBullet(obj);
            BulletCount -= 1;
        }

        private Vector3 _dir;

        protected abstract void OnEmitBullet(T projectile);

        protected override void OnStartUse_Internal()
        {
            base.OnStartUse_Internal();
            _reloadRoutine = StartCoroutine(ShotReload());
        }

        protected override void OnStartShot()
        {
            
        }

        protected override void OnEndShot()
        {
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, _dir);
        }
    }
}