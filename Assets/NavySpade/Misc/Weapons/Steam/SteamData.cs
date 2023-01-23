using System;
using Project19.Weapons.Water;
using UnityEngine;

namespace Project19.Weapons.Steam
{
    [CustomSerializeReferenceName("Пар")]
    [Serializable]
    public class SteamData : SpellDataBase<SteamStat>
    {
        [SerializeField] private float _disposeTime;

        public float DisposeTime => _disposeTime;
    }

    [Serializable]
    public class SteamStat : WaterStat
    {
        [SerializeField] private int _lifetimeInMilisecond;

        public float Lifetime => _lifetimeInMilisecond / 1000f;
    }
}