using System.IO;
using UnityEditor;
using UnityEngine;

namespace Sunday
{
    [CustomEditor(typeof(Settings))]
    public class SettingsEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box((Texture)AssetDatabase.LoadAssetAtPath("Assets/SundaySDK/Editor/Images/sunday_logo_medium.png", typeof(Texture)), new GUILayoutOption[] { GUILayout.Width(512), GUILayout.Height(128) });
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            base.OnInspectorGUI();

#if UNITY_STANDALONE
            EditorGUILayout.HelpBox("Standalone is not supported by the SDK. Change target to either Android or iOS", MessageType.Error);
#endif

            GUILayout.Space(20);

            if (GUILayout.Button("Load from CSV"))
            {
                LoadFromCSV();
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Save Settings"))
            {
                (serializedObject.targetObject as Settings).ValidateAll();
            }

#if UNITY_ANDROID
            GUILayout.Space(20);

            if (GUILayout.Button("Resolve dependencies! \n(required)"))
            {
                SundaySDKUtilityEditor.FixAndroidLibraries();
            }
            EditorGUILayout.HelpBox("<b>Important!</b> Resolve libraries before making a build. If the resolution fails check your External Tools paths in Preferences.", MessageType.None);
            GUIStyle myStyle = GUI.skin.GetStyle("HelpBox");
            myStyle.richText = true;
            GUILayout.Space(20);
#endif

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }

        public void LoadFromCSV()
        {
            string path = EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
            if (path.Length != 0)
            {
                string fileContent = File.ReadAllText(path);
                Settings settings = serializedObject.targetObject as Settings;

                settings.appsFlyerDevKey = ReadCSVLine(fileContent, 1);
                settings.iOSAppId = ReadCSVLine(fileContent, 2);
                settings.gameAnalyticsAndroidGameKey = ReadCSVLine(fileContent, 3);
                settings.gameAnalyticsAndroidSecretKey = ReadCSVLine(fileContent, 4);
                settings.gameAnalyticsIOSGameKey = ReadCSVLine(fileContent, 5);
                settings.gameAnalyticsIOSSecretKey = ReadCSVLine(fileContent, 6);
                settings.justTrackAndroidToken = ReadCSVLine(fileContent, 7);
                settings.justTrackIOSToken = ReadCSVLine(fileContent, 8);
                settings.ironSourceAndroidAppKey = ReadCSVLine(fileContent, 9);
                settings.ironSourceIOSAppKey = ReadCSVLine(fileContent, 10);
                settings.adMobAndroidID = ReadCSVLine(fileContent, 11);
                settings.adMobIosID = ReadCSVLine(fileContent, 12);

                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(serializedObject.targetObject);
            }
        }

        private string ReadCSVLine(string input, int line)
        {
            string ret = "";

            if (input != string.Empty)
            {
                string[] rows = input.Split('\r');

                if (line > 0 && line < rows.Length)
                {
                    string[] columns = rows[line].Split(',');
                    if (columns.Length > 1)
                    {
                        ret = columns[1];
                    }
                }
            }
            return ret;
        }
    }
}
