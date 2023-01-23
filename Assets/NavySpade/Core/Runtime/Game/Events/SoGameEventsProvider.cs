using NavySpade.Core.Runtime.Game;
using UnityEngine;
using Utils.SO.Events.Events;

namespace Core.Game.Events
{
    [RequireComponent(typeof(GameStateHandler))]
    public class SoGameEventsProvider : MonoBehaviour
    {
        [SerializeField] public VoidEvent _gameStarted;
        [SerializeField] public VoidEvent _gameEnded;

        private GameStateHandler _handler;

        private void Awake()
        {
            _handler = GetComponent<GameStateHandler>();
        }

        private void OnEnable()
        {
            _handler.Started += OnGameStarted;
            _handler.Ended += OnGameEnded;
        }

        private void OnDisable()
        {
            _handler.Started -= OnGameStarted;
            _handler.Ended -= OnGameEnded;
        }

        private void OnGameStarted()
        {
            if (_gameStarted)
                _gameStarted.Invoke();
        }

        private void OnGameEnded()
        {
            if (_gameEnded)
                _gameEnded.Invoke();
        }
    }
}