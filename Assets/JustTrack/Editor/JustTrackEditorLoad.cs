using UnityEditor;
using UnityEngine;

namespace JustTrack {
    class JustTrackEditorLoad {
        private static bool androidUsesIL2CPP = false;
        private static bool iOSUsesIL2CPP = false;

        [InitializeOnLoadMethod]
        static void OnProjectLoadedInEditor() {
            // ensure settings are created and everything is initalized nicely
            JustTrackUtils.GetOrCreateSettings();
            androidUsesIL2CPP = JustTrackUtils.IsIL2CPP(true);
            iOSUsesIL2CPP = JustTrackUtils.IsIL2CPP(false);
            if (JustTrackUtils.PerformInitFromLegacy()) {
                // only do this if we already have initialized the SDK settings
                OnIL2CPPChanged();

                foreach (string error in JustTrackUtils.Validate(JustTrackUtils.GetOrCreateSettings())) {
                    Debug.LogError(error);
                }
            }

            EditorApplication.update += Update;
        }

        private static void Update() {
            // only reconfigure the SDK if we have already initalized it!
            if (!JustTrackUtils.PerformInitFromLegacy()) {
                return;
            }

            if ((androidUsesIL2CPP != JustTrackUtils.IsIL2CPP(true)) || (iOSUsesIL2CPP != JustTrackUtils.IsIL2CPP(false))) {
                androidUsesIL2CPP = JustTrackUtils.IsIL2CPP(true);
                iOSUsesIL2CPP = JustTrackUtils.IsIL2CPP(false);
                OnIL2CPPChanged();
            }

            JustTrackSettings settings = JustTrackUtils.GetOrCreateSettings();

            JustTrackUtils.ConfigurePreprocessorDefines(
                BuildTargetGroup.Android,
                AndroidTrackingProvider.Appsflyer == settings.androidTrackingProvider,
                settings.androidIronSourceSettings.appKey != ""
            );
            JustTrackUtils.ConfigurePreprocessorDefines(
                BuildTargetGroup.iOS,
                iOSTrackingProvider.Appsflyer == settings.iosTrackingProvider,
                settings.iosIronSourceSettings.appKey != ""
            );
        }

        private static void OnIL2CPPChanged() {
            JustTrackUtils.OnUnityLoaded(JustTrackUtils.GetOrCreateSettings());
        }
    }
}
