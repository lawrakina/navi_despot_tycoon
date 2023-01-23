using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Core.Meta.Analytics;
using NavySpade.Meta.Runtime.Analytics;
using NavySpade.Modules.Sound.Runtime.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Damagables
{
    public partial class Damageable : MonoBehaviour
    {
        [Serializable]
        public class Events
        {
            public UnityEvent OnDead;
            public UnityEvent OnTakeDamage;
            public UnityEvent OnHeal;
        }

        public enum Team
        {
            Player,
            Enemy,
            Neitral
        }

        [Trackable(EnemyKillsAnalyticsKey)] private const string EnemyKillsAnalyticsKey = "Enemy Kills";

        [SerializeField] private Team _team;

        [SerializeField] private bool _isImmortal;
        [SerializeField] private int _maxHp;
        [SerializeField] private Events _events = new Events();

        public static event Action<Damageable> OnCreate, OnDestroy;
        public static event Action<Damageable, int> OnTakeDamageGlobal;

        private bool _isAlive;
        private int _hp;
        private Vector3 _currentPunchForce;
        private DamagablesEffect[] _damagablesEffects;

        private static Dictionary<Team, List<Damageable>> _allDamagableByTeam;

        public event Action<int> OnHPChange;
        public event Action<int> MaxHPChanged; 
        public event Action<int> TakeDamage, OnHeal;
        public event Action OnDeath;
        public event Action<Damageable> OnDeathDamagable;

        public virtual int MAXHp
        {
            get => _maxHp;
            set
            {
                var diff = value - _maxHp;
                _maxHp = value;
                
                MaxHPChanged?.Invoke(diff);
            }
        }

        public virtual int HP
        {
            get => _hp;
            set
            {
                if (value == _hp)
                    return;

                if (value > _maxHp || IsImmortal)
                    value = _maxHp;

                if (value < 0)
                    value = 0;

                if (value < _hp)
                {
                    TakeDamage?.Invoke(_hp - value);
                    OnTakeDamageGlobal?.Invoke(this, value);
                    _events.OnTakeDamage.Invoke();
                }

                _hp = value;
                OnHPChange?.Invoke(value);
                OnTakeDamageGlobal?.Invoke(this, value);
            }
        }

        public virtual Vector3 RagdollDir { get; set; }

        public virtual Team CurrentTeam
        {
            get => _team;
            set
            {
                if (_team == value)
                    return;

                if (_allDamagableByTeam == null)
                    _allDamagableByTeam = new Dictionary<Team, List<Damageable>>();

                if (_allDamagableByTeam.ContainsKey(_team))
                    _allDamagableByTeam[_team].Remove(this);

                if (_allDamagableByTeam.ContainsKey(value) == false)
                    _allDamagableByTeam.Add(value, new List<Damageable>());

                _allDamagableByTeam[value].Add(this);

                _team = value;
            }
        }

        public virtual bool IsImmortal
        {
            get => _isImmortal;
            set => _isImmortal = value;
        }

        public virtual bool IsAlive => _isAlive;

        private void Awake()
        {
            _damagablesEffects = GetComponents<DamagablesEffect>();

            ResetHP();

            OnAwake();
        }

        protected virtual void OnEnable()
        {
            if (_allDamagableByTeam == null)
                _allDamagableByTeam = new Dictionary<Team, List<Damageable>>();

            if (_allDamagableByTeam.ContainsKey(CurrentTeam) == false)
                _allDamagableByTeam.Add(CurrentTeam, new List<Damageable>());

            _allDamagableByTeam[CurrentTeam].Add(this);

            OnCreate?.Invoke(this);

            VariableTracker.BindKey(EnemyKillsAnalyticsKey);
        }

        protected virtual void OnDisable()
        {
            _allDamagableByTeam[CurrentTeam].Remove(this);
            OnDestroy?.Invoke(this);
        }

        protected virtual void OnAwake()
        {
        }

        [CanBeNull]
        public static List<Damageable> GetAllEnemysOfTeam(Team team)
        {
            switch (team)
            {
                case Team.Neitral:
                {
                    var list = new List<Damageable>();
                    list.AddRange(_allDamagableByTeam[Team.Player]);
                    list.AddRange(_allDamagableByTeam[Team.Enemy]);
                    return list;
                }
                case Team.Enemy:
                    
                    if (_allDamagableByTeam.ContainsKey(Team.Player) == false)
                        return null;
                    
                    return _allDamagableByTeam[Team.Player];
                case Team.Player:
                    
                    if (_allDamagableByTeam.ContainsKey(Team.Enemy) == false)
                        return null;
                    
                    return _allDamagableByTeam[Team.Enemy];
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }

        public static List<Damageable> GetAll(Team team)
        {
            return _allDamagableByTeam[team];
        }

        public virtual void DealDamage(int value, Team team, params IDamageParameter[] damageParameters)
        {
            if (IsImmortal)
                return;

            if (_team == team && team != Team.Neitral)
                return;

            if (CurrentTeam == Team.Enemy)
            {
                SoundPlayer.PlaySoundFx("EnemyHit");
            }

            foreach (var damagablesEffect in _damagablesEffects)
            {
                damagablesEffect.TakeDamage(value, team, damageParameters);
            }

            HP -= value;

            if (HP == 0 && IsAlive == true)
            {
                if (CurrentTeam == Team.Enemy)
                {
                    SoundPlayer.PlaySoundFx("EnemyDie");
                    VariableTracker.AddValue(EnemyKillsAnalyticsKey, 1);
                }

                OnDeath?.Invoke();
                _events.OnDead.Invoke();
                OnDeathDamagable?.Invoke(this);
                _isAlive = false;
            }
        }

        public virtual void Heal(int value, Team team)
        {
            if (_team != team)
                return;

            OnHeal?.Invoke(value);
            HP += value;
        }

        public virtual void ResetHP()
        {
            _hp = _maxHp;
            _isAlive = true;
        }

        public void ForceKill()
        {
            var isImmortal = IsImmortal;
            IsImmortal = false;
            TryKill();
            IsImmortal = isImmortal;
        }

        public bool TryKill()
        {
            DealDamage(HP, Team.Neitral);
            return IsImmortal == false;
        }

        public void SetImmortal(bool value)
        {
            _isImmortal = value;
        }
    }
}