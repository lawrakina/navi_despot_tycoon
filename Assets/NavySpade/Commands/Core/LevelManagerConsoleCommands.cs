using System;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Core.Runtime.Levels.Configuration;
using QFSW.QC;
using UnityEngine;

namespace NavySpade.Commands.Core
{
    [CommandPrefix("main.levels.")]
    public static class LevelManagerConsoleCommands
    {
        [Command("index")]
        public static int LevelIndex
        {
            get => LevelManager.LevelIndex;
            set => LevelManager.LevelIndex = value;
        }

        [Command("load.next")]
        public static void NextLevel()
        {
            LevelManager.LoadNextLevel();
        }

        [Command("load.preview")]
        public static void PreviewLevel()
        {
            LevelManager.LoadPreviewLevel();
        }

        [Command("load")]
        public static void LoadLevel(int index)
        {
            LevelManager.LevelIndex = index;
            LevelManager.Restart();
        }

        [Command("restart")]
        public static void RestartLevel()
        {
            LevelManager.Restart();
        }

        [Command("load")]
        public static void LoadLevel(string name)
        {
            var config = LevelsConfig.Instance;
            var foundedIndexes = Array.FindIndex(config.Tutorial, (level) => level.name == name);

            if (foundedIndexes == -1)
            {
                foundedIndexes = Array.FindIndex(config.Main, level => level.name == name);

                if (foundedIndexes == -1)
                {
                    Debug.LogError("level with the name is not exist");
                    return;
                }

                foundedIndexes += config.Tutorial.Length;
            }

            LoadLevel(foundedIndexes);
        }

        [Command("printall")]
        public static void PrintAllLevels()
        {
            var config = LevelsConfig.Instance;
            var tutorialLength = config.Tutorial.Length;

            Debug.Log("tutorials:");
            for (var i = 0; i < config.Tutorial.Length; i++)
            {
                Debug.Log($"{i} : {config.Tutorial[i].name}");
            }

            Debug.Log("mains:");
            for (var i = 0; i < config.Main.Length; i++)
            {
                Debug.Log($"{i + tutorialLength} : {config.Main[i].name}");
            }
        }
    }
}