using NavySpade.Modules.Configuration.Runtime.SO;
using Project19.Weapons;
using UnityEngine;

namespace Project19.Enemies
{
    public class AIConfig : ObjectConfig<AIConfig>
    {
        [Header("движения к цели")]
        [SerializeField] private float updatePathTime;
        [SerializeField] private float _nextPathToleraste = 0.1f;

        [Header("Blood")] 
        [SerializeField] private GameObject _bloodPrefab;
        
        [Header("атака")] 
        [SerializeField] private float _distanceToAttack = 2f;
        [SerializeField] private int _delayBetweenAttacksImMiliceconds;
        [SerializeField] private SpellData _rangeEnemySpell;

        [Header("Death Animation")] 
        [SerializeField] private Material _deathMaterial;
        [SerializeField] private float _corpseTime;
        [SerializeField] private float _disposeYPoint;
        [SerializeField] private float _minVelocity = .1f;
        
        [SerializeField] private int _disposeTimeInMilisecond;
        [SerializeField] private int _startSkillVFXDelayInMilisecond;

        public float CorpseTime => _corpseTime;

        public float UpdatePathTime => updatePathTime;

        public float NextPathToleraste => _nextPathToleraste;

        public float DistanceToAttack => _distanceToAttack;

        public float DelayBetweenAttacks => _delayBetweenAttacksImMiliceconds / 1000f;

        public SpellData RangeEnemySpell => _rangeEnemySpell;

        public float DisposeYPoint => _disposeYPoint;

        public float DisposeTime => _disposeTimeInMilisecond / 1000f;

        public float StartSkillVFXDelay => _startSkillVFXDelayInMilisecond / 1000f;

        public Material DeathMaterial => _deathMaterial;

        public GameObject BloodPrefab => _bloodPrefab;

        public float MINVelocity => _minVelocity;
    }
}