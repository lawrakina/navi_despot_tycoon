using System;
using UnityEngine;

namespace Project19.Weapons.Water
{
    [CustomSerializeReferenceName("Вода")]
    public class WaterData : SpellDataBase<WaterStat>
    {
        
    }
    
    [Serializable]
    public class WaterStat : StatBase
    {
        [SerializeField] private float _punchForce;

        public float PunchForce => _punchForce;
    }
}