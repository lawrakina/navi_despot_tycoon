using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Sunday
{
    public static class SundaySDKUtilityEditor
    {

        [MenuItem("SundaySDK / Settings", priority = 10)]
        internal static void SelectSettings()
        {
            Sunday.Settings.InitializeSettings();
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(Settings.SUNDAY_SETTINGS_PATH);
        }

#if UNITY_ANDROID
        /*internal static void FixAndroidSdkPath()
        {
            EditorPrefs.SetBool("SdkUseEmbedded", false);
            EditorPrefs.SetBool("JdkUseEmbedded", false);

            //Debug.Log("Previous JDK Path : " + EditorPrefs.GetString("JdkPath"));
            EditorPrefs.SetBool("JdkUseEmbedded", false);
            string jdkPath = Path.Combine(Path.GetDirectoryName(EditorApplication.applicationPath), "Data", "PlaybackEngines", "AndroidPlayer", "OpenJDK");
            EditorPrefs.SetString("JdkPath", jdkPath);

            //Debug.Log("Previous Android SDK Path : " + EditorPrefs.GetString("AndroidSdkRoot"));
            string androidSdkPath = Path.Combine(Path.GetDirectoryName(EditorApplication.applicationPath), "Data", "PlaybackEngines", "AndroidPlayer", "SDK");
            EditorPrefs.SetString("AndroidSdkRoot", androidSdkPath);
            Debug.Log("Android SDK Path : " + androidSdkPath);

            
            EditorPrefs.SetBool("NdkUseEmbedded", true);
            EditorPrefs.SetBool("GradleUseEmbedded", true);

            //Restart Editor
            EditorApplication.OpenProject(Directory.GetCurrentDirectory());
        }*/

        private static void DeleteAndResolveAndroidLibraries()
        {
            EditorApplication.ExecuteMenuItem("Assets/External Dependency Manager/Android Resolver/Delete Resolved Libraries");
            EditorApplication.ExecuteMenuItem("Assets/External Dependency Manager/Android Resolver/Force Resolve");
        }

        /*[MenuItem("SundaySDK / Fix Android SDK Path", priority = 30)]
        internal static void FixAndroidSDKPath()
        {
            FixAndroidSdkPath();
        }*/

        [MenuItem("SundaySDK / Resolve Libraries", priority = 30)]
        internal static void ResolveAndroidLibraries()
        {
            DeleteAndResolveAndroidLibraries();
        }
#endif
        internal static void FixAndroidLibraries()
        {
#if UNITY_ANDROID
            DeleteAndResolveAndroidLibraries();
#endif
        }
    }
}

