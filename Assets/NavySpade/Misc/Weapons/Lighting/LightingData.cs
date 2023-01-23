using System;
using UnityEngine;

namespace Project19.Weapons.Lighting
{
    [CustomSerializeReferenceName("Молния")]
    public class LightingData : SpellDataBase<LightingStat>
    {
        [SerializeField] private float _maxDistanceToRicochet;

        public float MAXDistanceToRicochet => _maxDistanceToRicochet;
    }

    [Serializable]
    public class LightingStat : StatBase
    {
        [SerializeField] private int _ricochetCount;

        public int RicochetCount => _ricochetCount;
    }
}