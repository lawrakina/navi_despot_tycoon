using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

#if UNITY_EDITOR
using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using UnityEditor;
#endif

using JustTrack;

namespace Sunday
{
    [System.Serializable]
    public class SundayJusttrackIronsourceSettings
    {
        public bool ironsourceShouldInitialize = true;
        public bool isBannerEnabled = true;
        public bool isInterstitialEnabled = true;
        public float interstitialCooldown = 30;
        public bool isRewardedVideoEnabled = true;
        public bool isOfferwallEnabled = true;
    }

    public class Settings : ScriptableObject
    {
        public const string SUNDAY_SETTINGS_PATH = "Assets/Resources/SundaySDK/Settings.asset";
        private const string GAMEANALYTICS_SETTINGS_PATH = "Assets/Resources/GameAnalytics/Settings.asset";
        private const string JUSTTRACK_SETTINGS_PATH = "Assets/JustTrack/Resources/JustTrackSettings.asset";

        private const string IRON_SOURCE_MEDIATION_SETTINGS_PATH = "Assets/IronSource/Resources/IronSourceMediationSettings.asset";
        private const string IRON_SOURCE_NETWORK_SETTINGS_PATH = "Assets/IronSource/Resources/IronSourceMediatedNetworkSettings.asset";

        static Settings _instance = null;
        public static Settings Instance
        {
            get
            {
                if (_instance==null)
                {
                    _instance = Resources.Load<Settings>("SundaySDK/Settings");
                    if(_instance == null)
                    {
                        _instance = new Settings(); //Use empty settings with default values if settings was not defined.
                    }
                }
                return _instance;
            }
        }

        [Header("Tracking")]

        public string appsFlyerDevKey = "N7kaNvpnoRHLC8ovLRC2Z";
        public string iOSAppId = "";

        public string gameAnalyticsAndroidGameKey;
        public string gameAnalyticsAndroidSecretKey;
        public string gameAnalyticsIOSGameKey;
        public string gameAnalyticsIOSSecretKey;
        public string justTrackAndroidToken;
        public string justTrackIOSToken;

        [Header("Ads")]
        public string ironSourceAndroidAppKey;
        public string ironSourceIOSAppKey;

        public SundayJusttrackIronsourceSettings testGroup1Settings;
        public SundayJusttrackIronsourceSettings testGroup2Settings;
        public SundayJusttrackIronsourceSettings testGroup3Settings;

        public IronSourceBannerSize bannerSize = IronSourceBannerSize.BANNER;
        public IronSourceBannerPosition bannerPosition = IronSourceBannerPosition.BOTTOM;

        public string adMobAndroidID;
        public string adMobIosID;

#if UNITY_EDITOR

        public void ValidateAll()
        {
            InitializeSettings();
            ReconfigureGameAnalytics();
            ReconfigureJustTrack();

            ChangeAndroidManifest(adMobAndroidID);
        }

        public static void InitializeSettings()
        {
            if (!Directory.Exists(Application.dataPath + "/Resources"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Resources");
            }
            if (!Directory.Exists(Application.dataPath + "/Resources/SundaySDK"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Resources/SundaySDK");
            }

            if (!File.Exists(SUNDAY_SETTINGS_PATH))
            {
                Settings settings = ScriptableObject.CreateInstance<Settings>();
                AssetDatabase.CreateAsset(settings, SUNDAY_SETTINGS_PATH);

                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void ReconfigureGameAnalytics()
        {
            if (!Directory.Exists(Application.dataPath + "/Resources"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Resources");
            }
            if (!Directory.Exists(Application.dataPath + "/Resources/GameAnalytics"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Resources/GameAnalytics");
            }

            GameAnalyticsSDK.Setup.Settings settings;

            if (File.Exists(GAMEANALYTICS_SETTINGS_PATH))
            {
                settings = AssetDatabase.LoadAssetAtPath<GameAnalyticsSDK.Setup.Settings>(GAMEANALYTICS_SETTINGS_PATH);
            }
            else
            {
                settings = ScriptableObject.CreateInstance<GameAnalyticsSDK.Setup.Settings>();
                AssetDatabase.CreateAsset(settings, GAMEANALYTICS_SETTINGS_PATH);
            }

            if (settings != null)
            {
                for (int i = settings.Platforms.Count - 1; i >= 0; --i)
                {
                    settings.RemovePlatformAtIndex(i);
                }

                //Add android
                if (!string.IsNullOrWhiteSpace(gameAnalyticsAndroidGameKey) && !string.IsNullOrWhiteSpace(gameAnalyticsAndroidSecretKey))
                {
                    settings.AddPlatform(RuntimePlatform.Android);
                    int androidIndex = settings.Platforms.IndexOf(RuntimePlatform.Android);
                    settings.UpdateGameKey(androidIndex, gameAnalyticsAndroidGameKey);
                    settings.UpdateSecretKey(androidIndex, gameAnalyticsAndroidSecretKey);
                }

                //Add ios
                if (!string.IsNullOrWhiteSpace(gameAnalyticsIOSGameKey) && !string.IsNullOrWhiteSpace(gameAnalyticsIOSSecretKey))
                {
                    settings.AddPlatform(RuntimePlatform.IPhonePlayer);
                    int androidIndex = settings.Platforms.IndexOf(RuntimePlatform.IPhonePlayer);
                    settings.UpdateGameKey(androidIndex, gameAnalyticsIOSGameKey);
                    settings.UpdateSecretKey(androidIndex, gameAnalyticsIOSSecretKey);
                }

                settings.UsePlayerSettingsBuildNumber = true;

                settings.CustomDimensions01 = new List<string>();
                settings.CustomDimensions01.Add("0");
                settings.CustomDimensions01.Add("1");
                settings.CustomDimensions01.Add("2");
                settings.CustomDimensions01.Add("3");

                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void ReconfigureJustTrack()
        {
            if (!Directory.Exists(Application.dataPath + "/JustTrack"))
            {
                Directory.CreateDirectory(Application.dataPath + "/JustTrack");
            }
            if (!Directory.Exists(Application.dataPath + "/JustTrack/Resources"))
            {
                Directory.CreateDirectory(Application.dataPath + "/JustTrack/Resources");
            }

            JustTrackSettings settings;

            if (File.Exists(JUSTTRACK_SETTINGS_PATH))
            {
                settings = AssetDatabase.LoadAssetAtPath<JustTrackSettings>(JUSTTRACK_SETTINGS_PATH);
            }
            else
            {
                settings = ScriptableObject.CreateInstance<JustTrackSettings>();
                AssetDatabase.CreateAsset(settings, JUSTTRACK_SETTINGS_PATH);
            }

            if (settings != null)
            {
                settings.androidApiToken = justTrackAndroidToken;
                settings.iosApiToken = justTrackIOSToken;
                settings.androidTrackingProvider = AndroidTrackingProvider.Appsflyer;
                settings.iosTrackingProvider = iOSTrackingProvider.Appsflyer;

                settings.androidIronSourceSettings.appKey = ironSourceAndroidAppKey;
                if (ironSourceAndroidAppKey != "")
                {
                    settings.androidIronSourceSettings.enableBanner = true;
                    settings.androidIronSourceSettings.enableInterstitial = true;
                    settings.androidIronSourceSettings.enableOfferwall = true;
                    settings.androidIronSourceSettings.enableRewardedVideo = true;
                }
                else
                {
                    settings.androidIronSourceSettings.enableBanner = false;
                    settings.androidIronSourceSettings.enableInterstitial = false;
                    settings.androidIronSourceSettings.enableOfferwall = false;
                    settings.androidIronSourceSettings.enableRewardedVideo = false;
                }

                settings.iosIronSourceSettings.appKey = ironSourceIOSAppKey;
                if (ironSourceIOSAppKey != "")
                {
                    settings.iosIronSourceSettings.enableBanner = true;
                    settings.iosIronSourceSettings.enableInterstitial = true;
                    settings.iosIronSourceSettings.enableOfferwall = true;
                    settings.iosIronSourceSettings.enableRewardedVideo = true;
                }
                else
                {
                    settings.iosIronSourceSettings.enableBanner = false;
                    settings.iosIronSourceSettings.enableInterstitial = false;
                    settings.iosIronSourceSettings.enableOfferwall = false;
                    settings.iosIronSourceSettings.enableRewardedVideo = false;
                }

                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
#endif

#if UNITY_EDITOR

        public static void ChangeAndroidManifest(string admobId)
        {
            const string manifestPath = "/Plugins/Android/AndroidManifest.xml";
            XNamespace AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
            XNamespace AndroidXmlToolsNamespace = "http://schemas.android.com/tools";
            XNamespace androidNs = AndroidXmlNamespace;
            XNamespace androidToolsNs = AndroidXmlToolsNamespace;


            if (File.Exists(Application.dataPath + manifestPath))
            {
                XmlDocument document = new XmlDocument();

                document.Load(Application.dataPath + manifestPath);

                var nsResolver = new XmlNamespaceManager(new NameTable());
                nsResolver.AddNamespace("android", androidNs.NamespaceName);

                XmlNode node = document.SelectSingleNode("/manifest/application/meta-data[@android:name='com.google.android.gms.ads.APPLICATION_ID']", nsResolver);
                if (node != null)
                {
                    var value = node.Attributes["android:value"];
                    value.Value = admobId;
                }

                document.Save(Application.dataPath + manifestPath);
            }
        }

        /*
        private void ChangeIronSourceSettings()
        {
            if (!Directory.Exists("Assets/IronSource/Resources"))
            {
                Directory.CreateDirectory("Assets/IronSource/Resources");
            }

            IronSourceMediationSettings mediationSettings;

            if (File.Exists(IRON_SOURCE_MEDIATION_SETTINGS_PATH))
            {
                mediationSettings = AssetDatabase.LoadAssetAtPath<IronSourceMediationSettings>(IRON_SOURCE_MEDIATION_SETTINGS_PATH);
            }
            else
            {
                mediationSettings = ScriptableObject.CreateInstance<IronSourceMediationSettings>();
                AssetDatabase.CreateAsset(mediationSettings, IRON_SOURCE_MEDIATION_SETTINGS_PATH);
            }

            if (mediationSettings != null)
            {
                //Add android
                if (!string.IsNullOrWhiteSpace(ironSourceAndroidAppKey))
                {
                    mediationSettings.AndroidAppKey = ironSourceAndroidAppKey;
                }

                //Add ios
                if (!string.IsNullOrWhiteSpace(ironSourceIOSAppKey))
                {
                    mediationSettings.IOSAppKey = ironSourceIOSAppKey;
                }

                mediationSettings.EnableIronsourceSDKInitAPI = false;
                mediationSettings.AddIronsourceSkadnetworkID = true;

                EditorUtility.SetDirty(mediationSettings);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        */

#endif

    }
}
