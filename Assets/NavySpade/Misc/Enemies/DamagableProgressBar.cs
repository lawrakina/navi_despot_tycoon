using Core.Damagables;
using Core.UI;
using Misc.Damagables;
using UnityEngine;

namespace Project19.Enemies
{
    [RequireComponent(typeof(Progressbar))]
    public class DamagableProgressBar : MonoBehaviour
    {
        [SerializeField] private Damageable _damagable;

        private Progressbar _progressbar;

        private void Awake()
        {
            _progressbar = GetComponent<Progressbar>();
        }

        private void Start()
        {
            UpdateProgressBar();
        }

        private void OnEnable()
        {
            _damagable.OnHPChange += UpdateProgressBar;
        }

        private void OnDisable()
        {
            _damagable.OnHPChange -= UpdateProgressBar;
        }

        private void UpdateProgressBar()
        {
            UpdateProgressBar(_damagable.HP);
        }

        private void UpdateProgressBar(int value)
        {
            _progressbar.SetupProgressbar(_damagable.MAXHp, _damagable.HP);
            _progressbar.UpdateProgressbar(value);
        }
    }
}