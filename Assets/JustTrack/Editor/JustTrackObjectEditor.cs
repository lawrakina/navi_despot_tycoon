using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JustTrack {
    [CustomEditor(typeof(JustTrackSDKBehaviour))]
    public class JustTrackObjectEditor : Editor {
        bool justTrackFoldout = true;
        int selectedApiTokenPlatform = 0;

        bool ironsourceFoldout = true;
        int selectedIronsourcePlatform = 0;

        public override void OnInspectorGUI() {
            try {
                RenderGUI(JustTrackUtils.GetSerializedSettings(), ref justTrackFoldout, ref selectedApiTokenPlatform, ref ironsourceFoldout, ref selectedIronsourcePlatform);
            } catch (Exception e) {
                EditorGUILayout.HelpBox("Failed to render settings: " + e.Message + "\n" + e.StackTrace, MessageType.Error);
            }
        }

        internal static void RenderGUI(SerializedObject serializedObject, ref bool justTrackFoldout, ref int selectedApiTokenPlatform, ref bool ironsourceFoldout, ref int selectedIronsourcePlatform) {
            SerializedProperty androidApiToken = serializedObject.FindProperty("androidApiToken");
            SerializedProperty iosApiToken = serializedObject.FindProperty("iosApiToken");
            SerializedProperty androidTrackingProvider = serializedObject.FindProperty("androidTrackingProvider");
            SerializedProperty iosTrackingProvider = serializedObject.FindProperty("iosTrackingProvider");
            SerializedProperty androidIronSourceSettings = serializedObject.FindProperty("androidIronSourceSettings");
            SerializedProperty iosIronSourceSettings = serializedObject.FindProperty("iosIronSourceSettings");

            SerializedProperty androidIronsourceAppKey = androidIronSourceSettings.FindPropertyRelative("appKey");
            SerializedProperty androidIronsourceEnableBanner = androidIronSourceSettings.FindPropertyRelative("enableBanner");
            SerializedProperty androidIronsourceEnableInterstitial = androidIronSourceSettings.FindPropertyRelative("enableInterstitial");
            SerializedProperty androidIronsourceEnableRewardedVideo = androidIronSourceSettings.FindPropertyRelative("enableRewardedVideo");
            SerializedProperty androidIronsourceEnableOfferwall = androidIronSourceSettings.FindPropertyRelative("enableOfferwall");
            SerializedProperty iosIronsourceAppKey = iosIronSourceSettings.FindPropertyRelative("appKey");
            SerializedProperty iosIronsourceEnableBanner = iosIronSourceSettings.FindPropertyRelative("enableBanner");
            SerializedProperty iosIronsourceEnableInterstitial = iosIronSourceSettings.FindPropertyRelative("enableInterstitial");
            SerializedProperty iosIronsourceEnableRewardedVideo = iosIronSourceSettings.FindPropertyRelative("enableRewardedVideo");
            SerializedProperty iosIronsourceEnableOfferwall = iosIronSourceSettings.FindPropertyRelative("enableOfferwall");

            serializedObject.Update();

            string[] platformNames = {"Android", "iOS"};
            var androidTrackingProviderValues = Enum.GetValues(AndroidTrackingProvider.ManualInitialization.GetType());
            var iosTrackingProviderValues = Enum.GetValues(iOSTrackingProvider.ManualInitialization.GetType());

            ////////////
            // HEADER //
            ////////////

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Box("logo.png", 512, 128);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            ///////////////
            // API TOKEN //
            ///////////////

            justTrackFoldout = EditorGUILayout.Foldout(justTrackFoldout, new GUIContent("JustTrack SDK Settings"), true);

            if (justTrackFoldout) {
                EditorGUILayout.Space();
                selectedApiTokenPlatform = GUILayout.Toolbar(selectedApiTokenPlatform, platformNames);

                string packageId = "";
                switch (selectedApiTokenPlatform) {
                    case 0:
                        packageId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
                        break;
                    case 1:
                        packageId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
                        break;
                }
                EditorGUILayout.LabelField(platformNames[selectedApiTokenPlatform] + " Package Id:", packageId);

                EditorGUILayout.HelpBox("You have to configure the correct API token for each platform you want to build for. An API token should be of the form 'prod-...64 characters...'.", MessageType.Info);
                switch (selectedApiTokenPlatform) {
                    case 0:
                        EditorGUILayout.PropertyField(androidApiToken, new GUIContent("Android Api Token"));
                        break;
                    case 1:
                        EditorGUILayout.PropertyField(iosApiToken, new GUIContent("iOS Api Token"));
                        break;
                }

                EditorGUILayout.Space();

                ///////////////////////
                // TRACKING PROVIDER //
                ///////////////////////

                switch (selectedApiTokenPlatform) {
                    case 0: {
                        SerializedProperty trackingProviderProperty = androidTrackingProvider;
                        AndroidTrackingProvider trackingProvider = (AndroidTrackingProvider) EditorGUILayout.EnumPopup("Tracking provider", (AndroidTrackingProvider) androidTrackingProviderValues.GetValue(trackingProviderProperty.enumValueIndex));
                        int trackingProviderIndex = Array.IndexOf(androidTrackingProviderValues, trackingProvider);
                        trackingProviderProperty.enumValueIndex = trackingProviderIndex;
                        if (trackingProvider == AndroidTrackingProvider.ManualInitialization) {
                            EditorGUILayout.Space();
                            EditorGUILayout.HelpBox("Be careful, if you don't provide a tracking id yourself, the SDK will not start. Use Google Advertiser Id as the tracking id provider if the JustTrack SDK should automatically take care of this.", MessageType.Warning);

                            GUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Use GoogleAdvertiserId as tracking provider", new GUILayoutOption[] { GUILayout.Width(300) })) {
                                androidTrackingProvider.enumValueIndex = Array.IndexOf(androidTrackingProviderValues, AndroidTrackingProvider.GoogleAdvertiserId);
                            }
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        }
                        if (JustTrackUtils.DetectAppsflyer()) {
                            if (trackingProvider == AndroidTrackingProvider.ManualInitialization) {
                                EditorGUILayout.Space();
                                EditorGUILayout.HelpBox("The Appsflyer SDK was detected, but tracking is set to manual initialization.", MessageType.Warning);
                            }
                            if (trackingProvider == AndroidTrackingProvider.GoogleAdvertiserId) {
                                EditorGUILayout.Space();
                                EditorGUILayout.HelpBox("The Appsflyer SDK was detected but is not used to provide additional data to the JustTrack SDK. Use Appsflyer as the tracking id provider to fix this.", MessageType.Warning);
                            }
                        } else {
                            if (trackingProvider == AndroidTrackingProvider.Appsflyer) {
                                EditorGUILayout.Space();
                                EditorGUILayout.HelpBox("No Appsflyer SDK was detected, but it is set to be used to provide the tracking id.", MessageType.Warning);
                            }
                        }
                        break;
                    }
                    case 1: {
                        SerializedProperty trackingProviderProperty = iosTrackingProvider;
                        iOSTrackingProvider trackingProvider = (iOSTrackingProvider) EditorGUILayout.EnumPopup("Tracking provider", (iOSTrackingProvider) iosTrackingProviderValues.GetValue(trackingProviderProperty.enumValueIndex));
                        int trackingProviderIndex = Array.IndexOf(iosTrackingProviderValues, trackingProvider);
                        trackingProviderProperty.enumValueIndex = trackingProviderIndex;
                        if (trackingProvider == iOSTrackingProvider.ManualInitialization) {
                            EditorGUILayout.Space();
                            EditorGUILayout.HelpBox("Be careful, if you don't provide a tracking id yourself, the SDK will not start.", MessageType.Warning);
                        }
                        if (JustTrackUtils.DetectAppsflyer()) {
                            if (trackingProvider == iOSTrackingProvider.ManualInitialization) {
                                EditorGUILayout.Space();
                                EditorGUILayout.HelpBox("The Appsflyer SDK was detected, but tracking is set to manual initialization.", MessageType.Warning);
                            }
                        } else {
                            if (trackingProvider == iOSTrackingProvider.Appsflyer) {
                                EditorGUILayout.Space();
                                EditorGUILayout.HelpBox("No Appsflyer SDK was detected, but it is set to be used to provide the tracking id.", MessageType.Warning);
                            }
                        }
                        break;
                    }
                }
            }

            ///////////////////
            // DOCUMENTATION //
            ///////////////////

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("For more information on setting up the JustTrack SDK check out the relevant docs.", MessageType.None);
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("JustTrack Android Docs", new GUILayoutOption[] { GUILayout.Width(150) })) {
                Application.OpenURL("https://docs.justtrack.io/sdk/android-sdk-readme");
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("JustTrack iOS Docs", new GUILayoutOption[] { GUILayout.Width(150) })) {
                Application.OpenURL("https://docs.justtrack.io/sdk/ios-sdk-readme");
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("JustTrack Unity Docs", new GUILayoutOption[] { GUILayout.Width(150) })) {
                Application.OpenURL("https://docs.justtrack.io/sdk/unity-sdk-readme");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            ////////////////
            // IRONSOURCE //
            ////////////////

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Box("ironsrc_logo.png", 512, 128);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (JustTrackUtils.DetectIronSource()) {
                bool hasAndroid = androidApiToken.stringValue != "";
                bool hasIOS = iosApiToken.stringValue != "";
                if (androidIronsourceAppKey.stringValue == "" && hasAndroid) {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("IronSource SDK was detected, but no app key was set for Android.", MessageType.Warning);
                }
                if (iosIronsourceAppKey.stringValue == "" && hasIOS) {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("IronSource SDK was detected, but no app key was set for iOS.", MessageType.Warning);
                }

                if (JustTrackUtils.NeedsIronSourceIntegrationCode() && Reflection.GetIronSourceAdapter() == null) {
                    EditorGUILayout.HelpBox("No JustTrack IronSource Adapter was found in your assembly. You can generate one to ensure you can successfully integrate the JustTrack SDK with the IronSource SDK at runtime.", MessageType.Error);

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Generate IronSource Adapter", new GUILayoutOption[] { GUILayout.Width(200) })) {
                        JustTrackUtils.RunCodeGenerator(true, true);
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            } else {
                if (androidIronsourceAppKey.stringValue != "") {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("No IronSource SDK was detected, but an app key was set for Android.", MessageType.Warning);
                }
                if (iosIronsourceAppKey.stringValue != "") {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("No IronSource SDK was detected, but an app key was set for iOS.", MessageType.Warning);
                }
            }

            EditorGUILayout.Space();
            ironsourceFoldout = EditorGUILayout.Foldout(ironsourceFoldout, new GUIContent("JustTrack IronSource Integration"), true);
            if (ironsourceFoldout) {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("This component will automatically integrate the JustTrack SDK with the IronSource SDK and initialize the latter as soon as the integration was successful. Fill in the app key and all the ad units you want to enable per platform here. If you don't want to use IronSource, leave the fields blank.", MessageType.Info);
                EditorGUILayout.Space();
                selectedIronsourcePlatform = GUILayout.Toolbar(selectedIronsourcePlatform, platformNames);

                EditorGUILayout.HelpBox("Fill in your IronSource app key here.", MessageType.Info);
                switch (selectedIronsourcePlatform) {
                    case 0:
                        EditorGUILayout.PropertyField(androidIronsourceAppKey, new GUIContent("Android App Key"));
                        break;
                    case 1:
                        EditorGUILayout.PropertyField(iosIronsourceAppKey, new GUIContent("iOS App Key"));
                        break;
                }

                EditorGUILayout.HelpBox("Enable IronSource Ad Units", MessageType.None);
                switch (selectedIronsourcePlatform) {
                    case 0:
                        EditorGUILayout.PropertyField(androidIronsourceEnableBanner, new GUIContent("Banner"));
                        EditorGUILayout.PropertyField(androidIronsourceEnableInterstitial, new GUIContent("Interstitial"));
                        EditorGUILayout.PropertyField(androidIronsourceEnableRewardedVideo, new GUIContent("Rewarded Video"));
                        EditorGUILayout.PropertyField(androidIronsourceEnableOfferwall, new GUIContent("Offerwall"));
                        if (androidIronsourceAppKey.stringValue != "") {
                            var anyEnabled = androidIronsourceEnableBanner.boolValue || androidIronsourceEnableInterstitial.boolValue || androidIronsourceEnableRewardedVideo.boolValue || androidIronsourceEnableOfferwall.boolValue;
                            if (!anyEnabled) {
                                EditorGUILayout.Space();
                                EditorGUILayout.HelpBox("No IronSource ad units are enabled, but an app key was set.", MessageType.Warning);
                            }
                        }
                        break;
                    case 1:
                        EditorGUILayout.PropertyField(iosIronsourceEnableBanner, new GUIContent("Banner"));
                        EditorGUILayout.PropertyField(iosIronsourceEnableInterstitial, new GUIContent("Interstitial"));
                        EditorGUILayout.PropertyField(iosIronsourceEnableRewardedVideo, new GUIContent("Rewarded Video"));
                        EditorGUILayout.PropertyField(iosIronsourceEnableOfferwall, new GUIContent("Offerwall"));
                        if (iosIronsourceAppKey.stringValue != "") {
                            var anyEnabled = iosIronsourceEnableBanner.boolValue || iosIronsourceEnableInterstitial.boolValue || iosIronsourceEnableRewardedVideo.boolValue || iosIronsourceEnableOfferwall.boolValue;
                            if (!anyEnabled) {
                                EditorGUILayout.Space();
                                EditorGUILayout.HelpBox("No IronSource ad units are enabled, but an app key was set.", MessageType.Warning);
                            }
                        }
                        break;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        internal static void Box(string image, int width, int height) {
            var path = "Packages/io.justtrack.justtrack-unity-sdk/Editor/" + image;
            var fallback = "Assets/JustTrack/Editor/" + image;

            if (File.Exists(path)) {
                GUILayout.Box((Texture) AssetDatabase.LoadAssetAtPath(path, typeof(Texture)), new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) });
            } else {
                GUILayout.Box((Texture) AssetDatabase.LoadAssetAtPath(fallback, typeof(Texture)), new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) });
            }
        }
    }
}