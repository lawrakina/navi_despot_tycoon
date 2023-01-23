using UnityEngine;

namespace Project19.Weapons.LightingAirStrike
{
    [CustomSerializeReferenceName("Каскад молний")]
    public class LightingAirStrikeData : SpellDataBase<StatBase>
    {
        [SerializeField] private int _shotLifetimeInMiliseconds;
        
        [SerializeField] private int _shotsCount;

        public int ShotsCount => _shotsCount;

        public float ShotLifetime => _shotLifetimeInMiliseconds / 1000f;
    }
}