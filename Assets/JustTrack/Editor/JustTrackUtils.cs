using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace JustTrack {
    internal class JustTrackUtils {
        private static bool? needsInitFromLegacy = null;
        private static DateTime? initFromLegacyDeadline = null;

        internal static bool PerformInitFromLegacy() {
            JustTrackSettings settings = GetOrCreateSettings();
            if (needsInitFromLegacy == null) {
                needsInitFromLegacy = settings.androidApiToken == "" && settings.iosApiToken == "";
                initFromLegacyDeadline = DateTime.Now.AddMinutes(1);
            }

            if (!(needsInitFromLegacy ?? false)) {
                return true;
            }

            if (initFromLegacyDeadline == null || initFromLegacyDeadline < DateTime.Now) {
                needsInitFromLegacy = false;
                return true;
            }

            if (InitFromLegacySettings()) {
                needsInitFromLegacy = false;

                return true;
            }

            return false;
        }

        internal static JustTrackSettings GetSettings() {
            return AssetDatabase.LoadAssetAtPath<JustTrackSettings>(JustTrackSettings.JustTrackSettingsPath);
        }

        internal static JustTrackSettings GetOrCreateSettings() {
            var settings = GetSettings();
            if (settings == null) {
                settings = ScriptableObject.CreateInstance<JustTrackSettings>();
                settings.androidApiToken = "";
                settings.iosApiToken = "";
                settings.androidTrackingProvider = AndroidTrackingProvider.GoogleAdvertiserId;
                settings.iosTrackingProvider = iOSTrackingProvider.Appsflyer;
                needsInitFromLegacy = !InitFromLegacySettings();
                if (needsInitFromLegacy ?? false) {
                    initFromLegacyDeadline = DateTime.Now.AddMinutes(1);
                } else {
                    PersistSettings(settings);
                }
            }
            return settings;
        }

        private static void PersistSettings(JustTrackSettings settings) {
            Directory.CreateDirectory(JustTrackSettings.JustTrackSettingsDirectory);

            AssetDatabase.CreateAsset(settings, JustTrackSettings.JustTrackSettingsPath);
            AssetDatabase.SaveAssets();
        }

        internal static SerializedObject GetSerializedSettings() {
            return new SerializedObject(GetOrCreateSettings());
        }

        internal static List<string> Validate(JustTrackSettings settings) {
            var errors = new List<string>();

            ValidateToken(settings.androidApiToken, "Android", errors);
            ValidateToken(settings.iosApiToken, "iOS", errors);

            if (settings.androidApiToken == "" && settings.iosApiToken == "") {
                errors.Add("Neither Android nor iOS API tokens are configured. You need to configure an API token for each platform to use the JustTrack SDK");
            }

            if (settings.androidApiToken != "") {
                switch (settings.androidTrackingProvider) {
                    case AndroidTrackingProvider.Appsflyer:
                        ValidateAppsflyerDetected("Android", errors);
                        break;
                    case AndroidTrackingProvider.ManualInitialization:
                        break;
                    case AndroidTrackingProvider.GoogleAdvertiserId:
                        break;
                    default:
                        errors.Add("Invalid tracking provider configured on Android");
                        break;
                }
            }

            if (settings.iosApiToken != "") {
                switch (settings.iosTrackingProvider) {
                    case iOSTrackingProvider.Appsflyer:
                        ValidateAppsflyerDetected("iOS", errors);
                        break;
                    case iOSTrackingProvider.ManualInitialization:
                        break;
                    default:
                        errors.Add("Invalid tracking provider configured on iOS");
                        break;
                }
            }

            ValidateIronSourceSettings(settings.androidApiToken != "", "Android", settings.androidIronSourceSettings, errors);
            ValidateIronSourceSettings(settings.iosApiToken != "", "iOS", settings.iosIronSourceSettings, errors);

            return errors;
        }

        private static void ValidateToken(string token, string platform, List<string> errors) {
            if (token == null) {
                errors.Add(platform + " API token is null, should either be empty string or valid API token (internal error)");
                return;
            }
            if (token == "") {
                return;
            }

            var parts = token.Split('-');
            if (parts.Length != 2) {
                errors.Add(platform + " API token does not consist of two parts, expected something of the form 'prod-tokendata' or 'sandbox-tokendata'");
            } else if (parts[0] != "prod" && parts[0] != "sandbox") {
                errors.Add(platform + " API token starts with unknown environment '" + parts[0] + "', expected 'prod' or 'sandbox'");
            } else if (parts[1].Length != 64) {
                errors.Add(platform + " API token has " + parts[1].Length + " instead of 64 characters of token data");
            }
        }

        private static void ValidateAppsflyerDetected(string platform, List<string> errors) {
            if (JustTrackUtils.DetectAppsflyer()) {
                return;
            }

            errors.Add("The JustTrack SDK is configured to use appsflyer as tracking id provider on " + platform + ", but it was not found in your game");
        }

        private static void ValidateIronSourceSettings(bool activeOnPlatform, string platform, IronSourceSettings settings, List<string> errors) {
            if (settings.appKey == "") {
                if (settings.enableBanner) {
                    errors.Add("IronSource will not be managed by the JustTrack SDK on platform " + platform + ", but is configured to load banners");
                }
                if (settings.enableInterstitial) {
                    errors.Add("IronSource will not be managed by the JustTrack SDK on platform " + platform + ", but is configured to load interstitials");
                }
                if (settings.enableRewardedVideo) {
                    errors.Add("IronSource will not be managed by the JustTrack SDK on platform " + platform + ", but is configured to load rewarded videos");
                }
                if (settings.enableOfferwall) {
                    errors.Add("IronSource will not be managed by the JustTrack SDK on platform " + platform + ", but is configured to load the offerwall");
                }
            } else if (!activeOnPlatform) {
                errors.Add("There is no API token configured for the JustTrack SDK on platform " + platform + ", but IronSource is configured to be loaded by the SDK");
            } else {
                ValidateIronSourceDetected(platform, errors);
                if (!settings.enableBanner && !settings.enableInterstitial && !settings.enableRewardedVideo && !settings.enableOfferwall) {
                    errors.Add("IronSource will be loaded without any banners, interstitials, rewarded videos, or the offerwall on platform " + platform + " as no ad unit was configured to be loaded");
                }
            }
        }

        private static void ValidateIronSourceDetected(string platform, List<string> errors) {
            if (JustTrackUtils.DetectIronSource()) {
                return;
            }

            errors.Add("The JustTrack SDK is configured to integrate with ironSource on " + platform + ", but it was not found in your game");
        }

        private static bool InitFromLegacySettings() {
            for (int sceneIndex = 0; sceneIndex < EditorSceneManager.sceneCount; sceneIndex++) {
                Scene scene = EditorSceneManager.GetSceneAt(sceneIndex);
                foreach (GameObject go in scene.GetRootGameObjects()) {
                    JustTrackSDKBehaviour sdk = go.GetComponentInChildren<JustTrackSDKBehaviour>();
                    if (sdk != null) {
                        // delete old settings if they exist so we can save new settings
                        AssetDatabase.DeleteAsset(JustTrackSettings.JustTrackSettingsPath);

                        var settings = ScriptableObject.CreateInstance<JustTrackSettings>();
                        settings.androidApiToken = sdk.androidReleaseApiToken;
                        settings.iosApiToken = sdk.iosReleaseApiToken;
                        settings.androidTrackingProvider = sdk.androidTrackingProvider;
                        settings.iosTrackingProvider = sdk.iosTrackingProvider;
                        settings.androidIronSourceSettings.appKey = sdk.androidIronsourceAppKey;
                        settings.androidIronSourceSettings.enableBanner = sdk.androidIronsourceEnableBanner;
                        settings.androidIronSourceSettings.enableInterstitial = sdk.androidIronsourceEnableInterstitial;
                        settings.androidIronSourceSettings.enableRewardedVideo = sdk.androidIronsourceEnableRewardedVideo;
                        settings.androidIronSourceSettings.enableOfferwall = sdk.androidIronsourceEnableOfferwall;
                        settings.iosIronSourceSettings.appKey = sdk.iosIronsourceAppKey;
                        settings.iosIronSourceSettings.enableBanner = sdk.iosIronsourceEnableBanner;
                        settings.iosIronSourceSettings.enableInterstitial = sdk.iosIronsourceEnableInterstitial;
                        settings.iosIronSourceSettings.enableRewardedVideo = sdk.iosIronsourceEnableRewardedVideo;
                        settings.iosIronSourceSettings.enableOfferwall = sdk.iosIronsourceEnableOfferwall;
                        PersistSettings(settings);

                        return true;
                    }
                }
            }

            // hmm... didn't find it... then let's retry later when new scenes will be loaded, maybe Unity is still
            // loading right now and the scene with the SDK (or any scene) is not yet loaded

            return false;
        }

        internal static void OnUnityLoaded(JustTrackSettings settings) {
            RunCodeGenerator(JustTrackUtils.NeedsIronSourceIntegrationCode(), false);

            if (settings.androidTrackingProvider == AndroidTrackingProvider.ManualInitialization) {
                const string KEY_USE_ANDROID_MANUAL_PROVIDER = "io.justtrack.unity.useAndroidManualProvider";
                if (!SessionState.GetBool(KEY_USE_ANDROID_MANUAL_PROVIDER, false)) {
                    var result = EditorUtility.DisplayDialog("JustTrack SDK", "You are using 'Manual Initialization' as the tracking provider on Android. This will block the SDK start until you provided a tracking id. You can use the 'Google Advertiser Id' tracking provider to automatically take care of providing a tracking id instead", "Switch to Google Advertiser Id", "Continue using Manual Intialization");
                    if (result) {
                        settings.androidTrackingProvider = AndroidTrackingProvider.GoogleAdvertiserId;
                    } else {
                        SessionState.SetBool(KEY_USE_ANDROID_MANUAL_PROVIDER, true);
                    }
                }
            }
        }

        internal static void RunCodeGenerator(bool integrateWithIronSource, bool force) {
            new JustTrackCodeGenerator(integrateWithIronSource).Run(force);
        }

        internal static bool DetectAppsflyer() {
            try {
                return AssetDatabase.FindAssets("t:Prefab AppsFlyerObject").Length > 0 && Reflection.GetAppsflyerAssembly() != null;
            } catch (Exception) {
                return false;
            }
        }

        internal static bool DetectIronSource() {
            return Reflection.GetIronSourceAssembly() != null;
        }

        internal static void ConfigurePreprocessorDefines(BuildTargetGroup targetGroup, bool trackingProviderIsAppsflyer, bool enableIronsource) {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            string newDefines = GeneratePreprocessorDefines(defines, trackingProviderIsAppsflyer, enableIronsource, Enum.GetName(typeof(BuildTargetGroup), targetGroup));
            if (newDefines != defines) {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, newDefines);
                try {
                    // Sometimes the defines are not updated directly... we have to press ctrl+s first. Avoid that need by doing it automatically.
                    EditorApplication.ExecuteMenuItem("File/Save Project");
                } catch (Exception) {
                    Debug.LogError("Could not save project automatically. Please save project by hand to ensure preprocessor defines are up to date.");
                }
            }
        }

        private static string GeneratePreprocessorDefines(string currentDefines, bool trackingProviderIsAppsflyer, bool enableIronsource, string platform) {
            return BuildPreprocessorDefines(currentDefines, platform, new Dictionary<string, bool> {
                ["JUSTTRACK_APPSFLYER_INTEGRATION"] = trackingProviderIsAppsflyer,
                ["JUSTTRACK_IRONSOURCE_INTEGRATION"] = enableIronsource,
                // keep them for now to ensure we drop those defines again, remove with 4.5.0 or so
                ["JUSTTRACK_DEVELOPMENT_USE_PROD_ENDPOINT"] = false,
                ["JUSTTRACK_DEVELOPMENT_USE_SANDBOX_ENDPOINT"] = false,
                ["JUSTTRACK_RELEASE_USE_PROD_ENDPOINT"] = false,
                ["JUSTTRACK_RELEASE_USE_SANDBOX_ENDPOINT"] = false,
            });
        }

        private static string BuildPreprocessorDefines(string currentDefines, string platform, Dictionary<string, bool> wantedDefines) {
            List<string> defines = new List<string>();
            foreach (var define in currentDefines.Split(';')) {
                if (wantedDefines.ContainsKey(define)) {
                    // define managed by us
                    if (wantedDefines[define]) {
                        // we want to keep it
                        defines.Add(define);
                    } else {
                        Debug.Log("Dropping " + define + " preprocessor symbol for platform " + platform);
                    }
                    // remove it in any case, afterwards we don't need to iterate over the defines again
                    wantedDefines.Remove(define);
                } else {
                    // unknown define, keep it as it is
                    defines.Add(define);
                }
            }
            foreach (var wantedDefine in wantedDefines) {
                if (wantedDefine.Value) {
                    // we wanted to have this, but didn't find it yet, so add it
                    defines.Add(wantedDefine.Key);
                    Debug.Log("Adding " + wantedDefine.Key + " preprocessor symbol for platform " + platform);
                }
            }
            return String.Join(";", defines.ToArray());
        }

        internal static bool NeedsIronSourceIntegrationCode() {
            var settings = JustTrackUtils.GetOrCreateSettings();
            bool hasAndroid = settings.androidApiToken != "" && settings.androidIronSourceSettings.appKey != "";
            bool hasIOS = settings.iosApiToken != "" && settings.iosIronSourceSettings.appKey != "";

            return (hasAndroid && IsIL2CPP(true)) || (hasIOS && IsIL2CPP(false));
        }

        internal static bool IsIL2CPP(bool isAndroid) {
            var backend = PlayerSettings.GetScriptingBackend(isAndroid ? BuildTargetGroup.Android : BuildTargetGroup.iOS);
            return backend == ScriptingImplementation.IL2CPP;
        }
    }
}
