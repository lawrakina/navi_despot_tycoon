using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace JustTrack {
    static class JustTrackSettingsIMGUIRegister {
        internal const string settingsPath = "Project/JustTrackIMGUISettings";

        [SettingsProvider]
        public static SettingsProvider CreateJustTrackSettingsProvider() {
            bool justTrackFoldout = true;
            int selectedApiTokenPlatform = 0;
            bool ironsourceFoldout = true;
            int selectedIronsourcePlatform = 0;

            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider(settingsPath, SettingsScope.Project) {
                // By default the last token of the path is used as display name if no label is provided.
                label = "JustTrack",
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) => {
                    try {
                        var settings = JustTrackUtils.GetSerializedSettings();
                        JustTrackObjectEditor.RenderGUI(settings, ref justTrackFoldout, ref selectedApiTokenPlatform, ref ironsourceFoldout, ref selectedIronsourcePlatform);
                    } catch (Exception e) {
                        EditorGUILayout.HelpBox("Failed to render settings: " + e.Message + "\n" + e.StackTrace, MessageType.Error);
                    }
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "JustTrack", "Some String" })
            };

            return provider;
        }
    }
}
