using System;
using System.Collections.Generic;
using Core.Meta.Analytics;
using EventSystem.Runtime.Core.Managers;
using NavySpade.Core.Runtime.Levels.Configuration;
using NavySpade.Core.Runtime.Levels.Data;
using NavySpade.Core.Runtime.Levels.Generation.Abstract;
using NavySpade.Meta.Runtime.Analytics;
using NavySpade.Modules.Saving.Runtime;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels
{
    public class LevelManager : MonoBehaviour
    {
        [field: SerializeField] public List<LevelGeneratorBase> Generators { get; private set; }

        private void Awake()
        {
            _instance = this;
        }

        public static event Action LevelLoaded;
        public static event Action<int> LevelIndexChanged;

        [Trackable(PASSED_LEVELS_KEY)] private const string PASSED_LEVELS_KEY = "Level Passed";

        private static LevelManager _instance;
        private static LevelsConfig _config;
        private static bool _isFirstInit = true;

        private static int? _levelIndex;

        public static int LevelIndex
        {
            get
            {
                _levelIndex ??= SaveManager.Load<int>("LevelIndex");
                return (int)_levelIndex;
            }
            set
            {
                _levelIndex = value;
                LevelIndexChanged?.Invoke(value);
                SaveManager.Save("LevelIndex", value);
            }
        }

        private static LevelsConfig Config => LevelsConfig.Instance;

        public static LevelDataBase CurrentLevelData { get; private set; }

        public static int CurrentLevelIndex { get; set; }

        /// <summary>
        /// Для возможности перематовать уровень.
        /// </summary>
        public static int LastOpenedLevelIndex
        {
            get => SaveManager.Load<int>("RealLevelIndex");
            set => SaveManager.Save("RealLevelIndex", value);
        }

        public static LevelDataBase LastLevelPrefab
        {
            get
            {
                if (LevelIndex < Config.Tutorial.Length)
                    return Config.Tutorial[LevelIndex];

                var deltaLevel = LevelIndex - Config.Tutorial.Length;
                deltaLevel = (int)Mathf.Repeat(deltaLevel, Config.Main.Length);

                return Config.Main[deltaLevel];
            }
        }

        public static LevelDataBase PreviewLevelPrefab(int levelIndex)
        {
            levelIndex--;

            if (levelIndex < 0)
            {
                throw new ArithmeticException();
            }

            if (levelIndex < Config.Tutorial.Length)
            {
                return Config.Tutorial[levelIndex];
            }

            var deltaLevel = levelIndex - Config.Tutorial.Length;
            deltaLevel = (int)Mathf.Repeat(deltaLevel, Config.Main.Length);
            return Config.Main[deltaLevel];
        }

        public static LevelDataBase NextLevelPrefab(int levelIndex)
        {
            levelIndex++;
            if (levelIndex < 0)
            {
                throw new ArithmeticException();
            }

            if (levelIndex < Config.Tutorial.Length)
            {
                return Config.Tutorial[levelIndex];
            }

            var deltaLevel = levelIndex - Config.Tutorial.Length;
            deltaLevel = (int)Mathf.Repeat(deltaLevel, Config.Main.Length);

            return Config.Main[deltaLevel];
        }

        public static void LoadPreviewLevel()
        {
            var preview = PreviewLevelPrefab(CurrentLevelIndex);
            CurrentLevelIndex--;

            Generate(preview);
        }

        public static void LoadNextLevel()
        {
            var preview = NextLevelPrefab(CurrentLevelIndex);
            CurrentLevelIndex++;

            Generate(preview);
        }

        public static void NextLevel()
        {
            if (CurrentLevelIndex == LastOpenedLevelIndex)
            {
                UnlockNextLevel();
                Restart();
            }
            else
            {
                LoadNextLevel();
            }
        }

        public static bool IsTutorialLevelIndex(int index)
        {
            if (index >= Config.Tutorial.Length)
            {
                return false;
            }

            return CurrentLevelData == Config.Tutorial[index];
        }

        public static void UnlockNextLevel()
        {
            LevelIndex++;
            var realLevelIndex = LevelIndex - Config.Tutorial.Length;
            LastOpenedLevelIndex++;

            if (realLevelIndex >= Config.Main.Length)
            {
                LevelIndex = Config.Tutorial.Length;
            }
        }

        public static void Restart()
        {
            CurrentLevelIndex = LastOpenedLevelIndex;
            Generate(LastLevelPrefab);
        }

        public static void Generate(LevelDataBase level)
        {
            if (_isFirstInit == false)
            {
                _instance.Generators.ForEach(e => e.CleanUp());
            }

            _instance.Generators.ForEach(e => e.Generate(level));
            LevelLoaded?.Invoke();

            CurrentLevelData = level;

            _isFirstInit = false;
        }

        private void OnEnable()
        {
            VariableTracker.StartTrack(PASSED_LEVELS_KEY, LevelIndex, ref LevelIndexChanged);
        }

        private void OnDisable()
        {
            VariableTracker.EndTrack(PASSED_LEVELS_KEY);
        }
    }
}