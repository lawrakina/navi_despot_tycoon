using System;
using Core.Damagables;
using Misc.Damagables;
using NavySpade.Modules.Sound.Runtime.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project19.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        public void Init(Damageable ownedDamageable, SpellData attahcedSpell)
        {
            _onwerDamage = ownedDamageable;
            _attachedSpell = attahcedSpell;
        }
        
        public event Action<int> OnBulletCountChange;
        public event Action OnShot;
        
        
        [SerializeField] private int _bulletCount;
        
        [HideInInspector] public bool UseCustomTearLevel;
        [HideInInspector] public int CustomTearLevel;
        
        private Damageable _onwerDamage;
        private SpellData _attachedSpell;

        private bool _isInitBulletCount;
        
        public bool IsShot { get; private set; }

        public abstract StatBase BaseStats { get; }

        protected abstract Func<int> SetStartBulletCount { get; }

        public int BulletCount
        {
            get => _bulletCount;
            set
            {
                if (_isInitBulletCount)
                {
                    var power = Random.Range(BaseStats.CameraShakePowerMinMax.x, BaseStats.CameraShakePowerMinMax.y);
                    SoundPlayer.PlaySoundFx("Shot");
                }

                if (AttachedSpell.IsInfinityAmmo == false)
                {
                    OnBulletCountChange?.Invoke(value);
                    _bulletCount = value;
                }
            }
        }

        public Damageable AttachedDamageable => _onwerDamage;
        public Damageable.Team CurrentTeam => _onwerDamage.CurrentTeam;
        public SpellData AttachedSpell => _attachedSpell;

        public void OnStartUse()
        {
            BulletCount = SetStartBulletCount.Invoke();
            _isInitBulletCount = true;
            
            OnStartUse_Internal();
        }

        protected void InvokeShotEvent()
        {
            OnShot?.Invoke();
        }

        protected virtual void OnStartUse_Internal()
        {
            
        }

        public virtual void OnEndUse()
        {
            
        }

        /// <summary>
        /// если ты получил ещё один спелл, но не нашлось новое заклинание то тебе дают это
        /// </summary>
        public virtual void AddSynergySpell(PickupType type)
        {
            
        }

        public void StartShot()
        {
            if(IsShot)
                return;
            
            IsShot = true;
            OnStartShot();
        }

        public void EndShot()
        {
            if(IsShot == false)
                return;
            
            IsShot = false;
            OnEndShot();
        }
        
        protected abstract void OnStartShot();
        protected abstract void OnEndShot();
    }
}