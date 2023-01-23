using System.Collections;
using DG.Tweening;
using Misc.Enemies;
using Misc.FakeShadow;
using Misc.Physic;
using UnityEngine;

namespace Project19.Enemies
{
    [RequireComponent(typeof(Ragdoll))]
    public class EnemyDeathAnimation : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;

        [SerializeField] private Collider _mainEnemyCollider;
        [SerializeField] private Renderer _rendererToChangeMaterial;
        [SerializeField] private FakeShadow _disableShadowByDeath;

        private Ragdoll _ragdoll;
        private AIConfig _config;

        private void Awake()
        {
            _ragdoll = GetComponent<Ragdoll>();
            _config = AIConfig.Instance;
        }

        private void OnEnable()
        {
            _enemy.Dead += OnDeath;
        }

        private void OnDisable()
        {
            _enemy.Dead -= OnDeath;
        }

        private void OnDeath(Enemy.DeathState state)
        {
            if (state != Enemy.DeathState.Die)
                return;

            _rendererToChangeMaterial.material = _config.DeathMaterial;
            _ragdoll.IsRagdollActive = true;
            _ragdoll.CenterBody.velocity = _enemy.Damagable.RagdollDir;
            _mainEnemyCollider.enabled = false;
            _disableShadowByDeath.gameObject.SetActive(false);

            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(_config.CorpseTime);

            while (_ragdoll.CenterBody.velocity.magnitude > _config.MINVelocity)
            {
                yield return null;
            }

            _ragdoll.IsRagdollActive = false;

            transform.DOMoveY(_config.DisposeYPoint, _config.DisposeTime).onComplete += () => { _enemy.Destroy(); };
        }
    }
}