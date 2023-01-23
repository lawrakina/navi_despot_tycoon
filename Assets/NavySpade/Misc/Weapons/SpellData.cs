using System;
using System.Collections.Generic;
using NavySpade.Modules.Utils.Serialization.SerializeReferenceExtensions.Runtime.Obsolete.SR;
using UnityEngine;

namespace Project19.Weapons
{
    public enum PickupType
    {
        Water = 0,
        Fire = 1,
        Lighting = 2
    }

    public interface ISpellCraft
    {
        
    }

    [CustomSerializeReferenceName("Крафт из пикапов")]
    public class CraftViaPickup : ISpellCraft
    {
        public PickupType PickupType;
    }
    
    [CustomSerializeReferenceName("Крафт из спелов")]
    
    public class CraftViaSpell : ISpellCraft
    {
        public int SpellIndex;
    }
    
    [Serializable]
    public class SpellCraft
    {
        [SR] [SerializeReference] private ISpellCraft _mode;

        public ISpellCraft Mode => _mode;

        public List<PickupType> GetBasicElements()
        {
            if (_mode == null)
                return null;

            if (_mode is CraftViaPickup craftViaPickup)
            {
                return new List<PickupType>() {craftViaPickup.PickupType};
            }
            else if (_mode is CraftViaSpell craftViaSpell)
            {
                var cfg = SpellsConfig.Instance;
                var list = new List<PickupType>();
                foreach (var dataCraft in cfg.Spells[craftViaSpell.SpellIndex].Data.Craft)
                {
                    list.AddRange(dataCraft.GetBasicElements());
                }

                return list;
            }

            return null;
        }
    }
    
    [Serializable]
    public class SpellData
    {
        [Tooltip("используется просто для отображения в этом конфиги и нигде больше")]
        [SerializeField] private string name;

        [SerializeField] private Sprite _icon;

        [SerializeField] private PickupType _mainType;
        
        [SR] [SerializeReference] private SpellDataBase _spellData;
        
        [SerializeField] private bool _isInfinityAmmo;
        [SerializeField] private bool _isActivateSpell;

        public bool IsInfinityAmmo => _isInfinityAmmo;
        public bool IsActivateSpell => _isActivateSpell;

        public SpellDataBase Data => _spellData;

        public string Name => name;

        public Sprite Icon => _icon;

        public PickupType MainType => _mainType;
    }
    
    public abstract class SpellDataBase<T> : SpellDataBase where T : StatBase
    {
        [SerializeField] private T _stats;

        public T Stats => _stats;
    }

    public abstract class SpellDataBase
    {
        [SerializeField] private SpellCraft[] _craft;
        [SerializeField] private WeaponBase _weaponBasePrefab;
        public SpellCraft[] Craft => _craft;

        public WeaponBase WeaponBasePrefab => _weaponBasePrefab;
    }

    [Serializable]
    public class StatBase
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _ragdollVelocity;
        [SerializeField] private Vector2 _cameraShakePowerMinMax;
        [SerializeField] private float _cameraShakeDuration;
        
        [SerializeField] private float _scaleMultiplay = 1;
        [SerializeField] private int _shotDelayInMiliSecond;
        [SerializeField] private float _shotSpeed;
        
        [SerializeField] private float _distance = 10;

        [SerializeField] private Vector2 _cubicDistanceOffset = new Vector2(0, 2.5f);
        [SerializeField] private Vector2 _cubicDistance;

        [SerializeField] private int _bulletCount;

        public int Damage => _damage;

        public float ScaleMultiplay => _scaleMultiplay;
        public float ShotDelay => _shotDelayInMiliSecond / 1000f;

        public float ShotSpeed => _shotSpeed;

        public Vector2 Distance => _cubicDistance;
        public float CircularDistance => _distance;

        public int BulletCount => _bulletCount;

        public float RagdollVelocity => _ragdollVelocity;

        public Vector2 CubicDistanceOffset => _cubicDistanceOffset;

        public Vector2 CameraShakePowerMinMax => _cameraShakePowerMinMax;

        public float CameraShakeDuration => _cameraShakeDuration;
    }
}