using System;
using UnityEngine;

namespace Project19.Weapons.Meteor
{
    [CustomSerializeReferenceName("Метеор")]
    [Serializable]
    public class MeteorData : SpellDataBase<MeteorStats>
    {
        
    }

    [Serializable]
    public class MeteorStats : StatBase
    {
        [SerializeField] private float _aoe;
        
        [SerializeField] [Range(1, 89)] private float _angle;
        [SerializeField] private float _destoyYPos = 0;

        public float Angle => _angle;
        public float DestroyYPos => _destoyYPos;
        public float Aoe => _aoe;
    }
}