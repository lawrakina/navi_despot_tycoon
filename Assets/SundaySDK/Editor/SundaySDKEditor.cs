using UnityEditor;
using UnityEngine;

namespace Sunday
{
    [CustomEditor(typeof(SundaySDKBehavior))]
    public class SundaySDKEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box((Texture)AssetDatabase.LoadAssetAtPath("Assets/SundaySDK/Editor/Images/sunday_logo_medium.png", typeof(Texture)), new GUILayoutOption[] { GUILayout.Width(512), GUILayout.Height(128) });
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Settings"))
            {
                Settings.InitializeSettings();
                SundaySDKUtilityEditor.SelectSettings();
            }
        }
    }
}
