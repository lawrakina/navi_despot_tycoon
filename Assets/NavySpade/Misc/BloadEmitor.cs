using Core.Damagables;
using Misc.Damagables;
using UnityEngine;

namespace Misc
{
    [RequireComponent(typeof(Damageable))]
    public class BloadEmitor : MonoBehaviour
    {
        [SerializeField] private GameObject _bloadPrefab;
        

        private Damageable _damagable;

        private void Awake()
        {
            _damagable = GetComponent<Damageable>();
        }

        private void OnEnable()
        {
            _damagable.TakeDamage += OnTakeDamage;
        }

        private void OnDisable()
        {
            _damagable.TakeDamage -= OnTakeDamage;
        }

        private void OnTakeDamage(int count)
        {
            Instantiate(_bloadPrefab, transform.position, Quaternion.identity);
        }
    }
}