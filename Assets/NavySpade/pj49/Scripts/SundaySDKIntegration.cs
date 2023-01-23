using System.Collections;
using EventSystem.Runtime.Core.Managers;
using NavySpade.Core.Runtime.Levels;
using UnityEngine;

namespace pj40
{
    public class SundaySDKIntegration : MonoBehaviour
    {
        private int _currentLevelPlaytime;
        private bool _isLevelStarted;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            LevelManager.LevelLoaded += OnResetLevel;
            EventManager.Add(GameStatesEM.OnFail, OnLevelFailed);
            EventManager.Add(GameStatesEM.OnWin, OnLevelWin);
        }

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                _currentLevelPlaytime++;
            }
        }

        private void OnDestroy()
        {
            LevelManager.LevelLoaded -= OnResetLevel;

            EventManager.Remove(GameStatesEM.OnFail, OnLevelFailed);
            EventManager.Remove(GameStatesEM.OnWin, OnLevelWin);
        }

        private void OnResetLevel()
        {
            _currentLevelPlaytime = 0;
            
            print("sdk1 startLevel");
            
            SundaySDK.Tracking.TrackLevelStart(LevelManager.LevelIndex);
            _isLevelStarted = true;
        }

        private void OnLevelFailed()
        {
            if(_isLevelStarted == false)
                return;
            
            print("sdk1 endLevel lose");
            SundaySDK.Tracking.TrackLevelFail(LevelManager.LevelIndex);
        }

        private void OnLevelWin()
        {
            if(_isLevelStarted == false)
                return;
            
            print("sdk1 endLevel win");
            SundaySDK.Tracking.TrackLevelComplete(LevelManager.LevelIndex);
        }
    }
}