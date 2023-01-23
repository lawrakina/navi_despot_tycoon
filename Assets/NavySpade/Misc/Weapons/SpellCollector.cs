using System;
using System.Collections.Generic;
using System.Linq;
using Core.Damagables;
using JetBrains.Annotations;
using NavySpade.Modules.Extensions.CsharpTypes;
using UnityEngine;

namespace Project19.Weapons
{
    public class SpellCollector : MonoBehaviour
    {
        public event Action OnWeaponShot;
        public event Action<bool> OnShot;
        public event Action<WeaponBase> OnWeaponChange;
        public event Action<PickupType> OnTakePickup, OnRemovePickup;

        [CanBeNull]
        public event Action<WeaponBase> OnSetActiveSpell;

        private int? _inventSpellIndex;
        [SerializeField] private List<PickupType> _spells = new List<PickupType>();
        [SerializeField] private Transform _weaponParent;

        [SerializeField] private Damageable damageable;

        /// <summary>
        ///
        /// </summary>
        public int LocalTear => _usingSpell == null ? 0 : _spells.Count - _usingSpell.Data.Craft.Length;

        public bool IsAutoChangeWeapon
        {
            get => _isAutoChangeWeapon;
            set
            {
                if (value)
                {
                    if (IsAutoChangeWeapon)
                    {
                        if (_inventSpellIndex == null)
                        {
                            var first = _spells[0];
                            SetStartSpell(first);
                        }
                        else
                        {
                            SetStartSpell(_selectedWeaponBase.AttachedSpell.MainType);
                        }
                    }
                }
                
                _isAutoChangeWeapon = value;
            }
        }

        public WeaponBase SelectedWeaponBase => _selectedWeaponBase;

        private bool IsDefaultSpell => _inventSpellIndex == null;

        private bool _isUsing;
        private bool _isExternalSpell;
        
        private SpellData _usingSpell;
        private WeaponBase _selectedWeaponBase;
        private WeaponBase _selectedWeaponPrefab;

        private WeaponBase _activatedSpell;

        private SpellsConfig _config;
        private bool _isAutoChangeWeapon = true;


        private void Awake()
        {
            _config = SpellsConfig.Instance;
        }

        public void TakePickup(PickupType spell)
        {
            _spells.Add(spell);
            OnTakePickup?.Invoke(spell);

            if(IsAutoChangeWeapon == false)
                return;

            var newSpell = SimpleFindUsingSpell(_usingSpell, _spells, _config.StartSpells, out var index);

            if (newSpell != null)
            {
                _usingSpell = newSpell;

                if (_selectedWeaponBase != null)
                {
                    SelectSpell(_usingSpell, index);
                    AddCombination(_usingSpell, SelectedWeaponBase, spell);
                }
            }

            if (_selectedWeaponBase == null)
            {
                SetStartSpell(spell);
            }
        }

        public void SetStartSpell()
        {
            if(_config == null)
                Awake();

            var spell = _config.StartSpells.RandomElement();

            SetStartSpell(spell);
        }

        public void InvokeActivatedSpell()
        {
            if(_activatedSpell == null)
                return;
            
            _activatedSpell.StartShot();
        }

        public void RemoveActivationSpell()
        {
            if (_activatedSpell != null)
            {
                Destroy(_activatedSpell.gameObject);
            }
        }

        public void RemoveAllSpells()
        {
            foreach (var spell in _spells)
            {
                OnRemovePickup?.Invoke(spell);
            }
            
            _spells.Clear();
            RemoveSpell();
        }

        private void RemoveSpell(bool isSilentRemove = false)
        {
            if(_selectedWeaponBase == null)
                return;

            Destroy(_selectedWeaponBase.gameObject);

            if (_isUsing && isSilentRemove == false)
            {
                OnShot?.Invoke(false);
            }
            
            _usingSpell = null;
            _selectedWeaponBase = null;
            _selectedWeaponPrefab = null;
            _inventSpellIndex = null;

            if (_spells.Count > 0)
            {
                var spellForRemove = _spells[0];
                _spells.Remove(spellForRemove);
                OnRemovePickup?.Invoke(spellForRemove);
            }

            if(isSilentRemove == false || _isExternalSpell)
                OnWeaponChange?.Invoke(null);
            
            _isExternalSpell = false;
        }

        private void SetStartSpell(PickupType pickupType)
        {
            var spell = _config.GetStartSpell(pickupType);
            SetStartSpell(spell);
        }

        private void SetStartSpell(SpellData spell)
        {
            _usingSpell = spell;

            _inventSpellIndex = null;
            
            SelectSpell(_usingSpell, null);
        }

        [CanBeNull]
        private SpellData SimpleFindUsingSpell(SpellData currentSpell, List<PickupType> has, SpellData[] availableSpells, out int? index)
        {
            var first = has[0];

            foreach (var spell in availableSpells)
            {
                if (spell.MainType == first)
                {
                    index = null;
                    return spell;
                }
            }

            index = null;
            return null;
        }

        [CanBeNull]
        private SpellData FindUsingSpell(SpellData currentSpell, List<PickupType> has, SpellData[] availableSpells, out int? index)
        {
            SpellData data = null;
            index = null;

            var used = new List<PickupType>(has.Count);

            for (var i = 0; i < availableSpells.Length; i++)
            {
                var spell = availableSpells[i];
                var canUseTheSpell = true;
                
                if (_inventSpellIndex != null)
                {
                    var mode = spell.Data.Craft.Where((s) => s.Mode is CraftViaSpell).ToArray();

                    if (mode.Length == 0 ||
                        mode.All((s1) => (s1.Mode as CraftViaSpell).SpellIndex != _inventSpellIndex.Value))
                    {
                        canUseTheSpell = false;
                    }
                }

                used.Clear();
                used.AddRange(has);
                foreach (var craft in spell.Data.Craft)
                {
                    if (craft.Mode is CraftViaPickup craftViaPickup)
                    {
                        if (used.Contains(craftViaPickup.PickupType))
                        {
                            used.Remove(craftViaPickup.PickupType);
                        }
                        else
                        {
                            canUseTheSpell = false;
                            break;
                        }
                    }
                    else if (craft.Mode is CraftViaSpell craftViaSpell)
                    {
                        if (_inventSpellIndex == null)
                        {
                            canUseTheSpell = false;
                            break;
                        }

                        if (craftViaSpell.SpellIndex != _inventSpellIndex)
                        {
                            canUseTheSpell = false;
                            break;
                        }
                    }
                }

                if (canUseTheSpell == false)
                    continue;

                index = i;
                data = spell;
            }

            return data;
        }

        public void SelectSpell(SpellData spell)
        {
            _isExternalSpell = true;
            SelectSpell(spell, null);
        }
        
        private void SelectSpell(SpellData spell, int? indexInDefaultSpellConfig)
        {
            var prefab = spell.Data.WeaponBasePrefab;
            
            if (spell.IsActivateSpell)
            {
                var type = _selectedWeaponBase.AttachedSpell.MainType;
                var startSpell = _config.StartSpells.First((s) => s.MainType == type);
                SetStartSpell(startSpell);

                if (_activatedSpell != null)
                {
                    _activatedSpell.OnEndUse();
                    Destroy(_activatedSpell.gameObject);
                }

                _activatedSpell = Instantiate(prefab, _weaponParent);
                _activatedSpell.Init(damageable, spell);
                _activatedSpell.OnStartUse();
                
                _activatedSpell.OnBulletCountChange += i =>
                {
                    if (i <= 0)
                    {
                        if(_activatedSpell != null)
                            Destroy(_activatedSpell.gameObject);
                        
                        _activatedSpell = null;
                        OnSetActiveSpell?.Invoke(null);
                    }
                };
                
                OnSetActiveSpell?.Invoke(_activatedSpell);
                
                return;
            }

            if (SelectedWeaponBase != null)
            {
                SelectedWeaponBase.OnEndUse();
                _selectedWeaponBase.OnBulletCountChange -= OnBulletCountChange;
                Destroy(SelectedWeaponBase.gameObject);
            }

            _selectedWeaponPrefab = prefab;
            _inventSpellIndex = indexInDefaultSpellConfig;


            _selectedWeaponBase = Instantiate(prefab, _weaponParent);

            SelectedWeaponBase.Init(damageable, spell);
            SelectedWeaponBase.OnStartUse();
            
            SelectedWeaponBase.OnBulletCountChange += OnBulletCountChange;

            if (_isUsing)
            {
                StartUse_Internal();
            }


            OnWeaponChange?.Invoke(SelectedWeaponBase);
        }

        private void OnBulletCountChange(int value)
        {
            if (value > 0)
            {
                OnWeaponShot?.Invoke();
                return;
            }

            RemoveSpell(_spells.Count != 1);
            
            OnWeaponShot?.Invoke();
            
            if (_spells.Count <= 0)
            {
                return;
            }
            
            if (IsAutoChangeWeapon)
            {
                if (_inventSpellIndex == null)
                {
                    var first = _spells[0];
                    SetStartSpell(first);
                }
                else
                {
                    SetStartSpell(_selectedWeaponBase.AttachedSpell.MainType);
                }
            }
        }

        private void AddCombination(SpellData spell, WeaponBase weapon, PickupType type)
        {
            if (_spells.Count <= spell.Data.Craft.Length)
                return;

            weapon.AddSynergySpell(type);
        }

        public void StartUse()
        {
            if (SelectedWeaponBase == null)
                return;

            if (_isUsing)
                return;

            StartUse_Internal();

            _isUsing = true;
        }

        private void StartUse_Internal()
        {
            SelectedWeaponBase.StartShot();
            OnShot?.Invoke(true);
        }

        public void EndUse()
        {
            if (SelectedWeaponBase == null)
                return;

            if (_isUsing == false)
                return;

            SelectedWeaponBase.EndShot();
            OnShot?.Invoke(false);

            _isUsing = false;
        }
    }
}