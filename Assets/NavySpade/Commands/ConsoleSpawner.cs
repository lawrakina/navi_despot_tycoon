using NavySpade.Commands.Configuration;
using UnityEngine;

namespace NavySpade.Commands
{
    public class ConsoleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parent;

        private static ConsoleConfig Config => ConsoleConfig.Instance;

        private void Awake()
        {
            if (Config.EnableDebugConsole)
            {
                Instantiate(_prefab, _parent);
            }
        }
    }
}