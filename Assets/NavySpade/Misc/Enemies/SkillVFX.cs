using System.Collections;
using Misc.Enemies;
using UnityEngine;

namespace Project19.Enemies
{
    public class SkillVFX : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private GameObject _vfx;

        private AIConfig _config;
        
        private void Awake()
        {
            _config = AIConfig.Instance;
            _enemy.Dead += EnemyOnDead;
            _vfx.SetActive(false);
        }

        private void EnemyOnDead(Enemy.DeathState state)
        {
            if (state == Enemy.DeathState.Die)
            {
                StartCoroutine(Timer());
            }
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(_config.StartSkillVFXDelay);
            _vfx.SetActive(true);
        }
    }
}