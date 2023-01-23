using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace JustTrack {
    public class JustTrackMenu {
        [MenuItem("JustTrack/Show Settings", false, 100)]
        public static void ShowSettings() {
            SettingsService.OpenProjectSettings(JustTrackSettingsIMGUIRegister.settingsPath);
        }

        [MenuItem("JustTrack/Validate Configuration", false, 101)]
        public static void ValidateConfiguration() {
            Debug.ClearDeveloperConsole();
            ClearConsole();

            var valid = true;
            foreach (string error in JustTrackUtils.Validate(JustTrackUtils.GetOrCreateSettings())) {
                Debug.LogError(error);
                valid = false;
            }

            if (valid) {
                Debug.Log("JustTrack configuration is valid");
            }
        }


        [MenuItem("JustTrack/Create SDK Instance", false, 102)]
        public static void CreateInstance() {
            var path = "Packages/io.justtrack.justtrack-unity-sdk/Prefabs/JustTrackSDK.prefab";
            var fallback = "Assets/JustTrack/Prefabs/JustTrackSDK.prefab";
            UnityEngine.Object prefab;

            if (File.Exists(path)) {
                prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            } else {
                prefab = AssetDatabase.LoadAssetAtPath(fallback, typeof(GameObject));
            }
            PrefabUtility.InstantiatePrefab(prefab);
        }

        [MenuItem("JustTrack/Android Docs", false, 200)]
        public static void AndroidDocs() {
            Application.OpenURL("https://docs.justtrack.io/sdk/android-sdk-readme");
        }

        [MenuItem("JustTrack/iOS Docs", false, 201)]
        public static void iOSDocs() {
            Application.OpenURL("https://docs.justtrack.io/sdk/ios-sdk-readme");
        }

        [MenuItem("JustTrack/Unity Docs", false, 202)]
        public static void UnityDocs() {
            Application.OpenURL("https://docs.justtrack.io/sdk/unity-sdk-readme");
        }

        private static void ClearConsole() {
            try {
                var assembly = Assembly.GetAssembly(typeof(SceneView));
                var type = assembly.GetType("UnityEditor.LogEntries");
                var method = type.GetMethod("Clear");
                method.Invoke(new object(), null);
            } catch (Exception) {
                // well... then it seems like we don't clear the console today
            }
        }
    }
}
