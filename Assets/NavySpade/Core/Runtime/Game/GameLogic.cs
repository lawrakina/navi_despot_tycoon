using Core.Game;
using Core.Game.Configuration;
using Core.Player;
using EventSystem.Runtime.Core.Dispose;
using EventSystem.Runtime.Core.Managers;
using NaughtyAttributes;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Modules.Extensions.UnityTypes;
using NavySpade.Modules.Sound.Runtime.Core;
using UnityEngine;

namespace NavySpade.Core.Runtime.Game
{
    public class GameLogic : ExtendedMonoBehavior
    {
        #region Variables

        public static GameLogic Instance { get; private set; }

        [field: SerializeField] public GameStateHandler States { get; private set; }
        [SerializeField] private InGameEarnedCurrency _earned;

        private readonly EventDisposal _dispose = new EventDisposal();

        #endregion

        private void Awake()
        {
            Instance = this;

            #region Init events

            EventManager.Add(MainEnumEvent.NextLevel, NextLevel).AddTo(_dispose);
            EventManager.Add(MainEnumEvent.Restart, Restart).AddTo(_dispose);
            EventManager.Add(MainEnumEvent.Win, LevelWin).AddTo(_dispose);
            EventManager.Add(MainEnumEvent.Fail, LevelFail).AddTo(_dispose);
            EventManager.Add(MainEnumEvent.Pause, SetPause).AddTo(_dispose);

            #endregion
        }

        private void Start()
        {
            EventManager.Invoke(MainEnumEvent.Restart);
        }

        #region Start game

        private void StartGame()
        {
            PlayerStats.PlaysCount++;
            GlobalParameters.AttemptCount++;

            CallStartGameEvents();
        }

        private void CallStartGameEvents()
        {
            EventManager.Invoke(PopupsEnum.OpenStartGame);
        }

        private void PrepareGame()
        {
            EventManager.Invoke(MainEnumEvent.Clear);
            EventManager.Invoke(MainEnumEvent.PrepareLevel, LevelManager.CurrentLevelData);
            EventManager.Invoke(MainEnumEvent.PreparePlayer, LevelManager.CurrentLevelData);
            EventManager.Invoke(MainEnumEvent.PrepareGame);
            LevelManager.Restart();
        }

        #endregion

        #region Pause

        private void SetPause()
        {
            States.SetPause(!States.IsPaused);
        }

        #endregion

        #region Restart game

        private void Restart()
        {
            PrepareGame();
            StartGame();
        }

        private void NextLevel()
        {
            LevelManager.UnlockNextLevel();
            EventManager.Invoke(MainEnumEvent.Restart);
        }

        #endregion

        #region End game

        private void LevelFail()
        {
            if (States.IsStarted == false)
            {
                return;
            }

            CallEndGameEvents();
            CallLevelFailEvents();

            InvokeAtTime(GameLoopDelaysConfig.Instance.AfterLose, LevelFailPopup);
        }

        private void LevelFailPopup()
        {
            SoundPlayer.PlaySoundFx("Lose");
            EventManager.Invoke(PopupsEnum.OpenLosePopup);
        }

        private void LevelWin()
        {
            if (States.IsStarted == false)
            {
                return;
            }

            PlayerStats.WinsCount++;
            CallEndGameEvents();
            CallLevelWinEvents();

            InvokeAtTime(GameLoopDelaysConfig.Instance.AfterWin, LevelWinPopup);
        }

        private void LevelWinPopup()
        {
            SoundPlayer.PlaySoundFx("Win");
            EventManager.Invoke(PopupsEnum.OpenWinPopup, _earned.GenerateReward());
        }

        private void CallEndGameEvents()
        {
            EventManager.Invoke(GameStatesEM.EndGame);
        }

        private void CallLevelWinEvents()
        {
            EventManager.Invoke(GameStatesEM.OnWin);
        }

        private void CallLevelFailEvents()
        {
            EventManager.Invoke(GameStatesEM.OnFail);
        }

        #endregion

        private void OnDestroy()
        {
            #region Remove event subscribe

            _dispose.Dispose();

            #endregion
        }

        [Button]
        public void Reset()
        {
            States = GetComponentInChildren<GameStateHandler>();
            _earned = GetComponentInChildren<InGameEarnedCurrency>();
        }
    }
}