using GameAnalyticsSDK;
using System;
using UnityEngine;

namespace Sunday
{
    public class SundaySDKInitializer
    {
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            var sdkGo = new GameObject("SundaySDK");
            sdkGo.AddComponent<SundaySDKBehavior>();
        }
    }

    public class SundaySDKBehavior : MonoBehaviour
    {
        public static SundaySDKBehavior Instance { get; private set; }

        public static Action<string> OnRewardedVideoSuccess;
        public static Action<bool> OnRewardedVideoAvailabilityChanged;
        public static Action OnAdSoundIn;
        public static Action OnAdSoundOut;

        public int ActiveTestGroupIndex { get => activeTestGroupIndex; private set => activeTestGroupIndex = value; }
        private int activeTestGroupIndex = 1;
        [HideInInspector] public SundayJusttrackIronsourceSettings activeTestGroupSettings;

        public static string interstitialPlacement;
        public static string rewardedPlacement;
        public static float lastInterstitialShowTime;
        public static string lastRewardedType;

        public JustTrack.JustTrackSDKBehaviour JustTrackSDKBehaviourInstance { get; private set; }

        private void Awake()
        {
            if(Instance != null)
            {
                Debug.LogWarning("SundaySDK: Multiple instances detected, destroying self.");
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
        }

        public static int GetTestGroup(string pAdvertiserId)
        {
            if (pAdvertiserId != "")
            {
                string hex = pAdvertiserId.Substring(pAdvertiserId.Length - 3);
                int val = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                return 1 + (val % 3);
            }
            else
            {
                return 1; //default to first group
            }
        }

        public static string GetAdvertiserId()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass client = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
            AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity);

            return adInfo.Call<string>("getId").ToString();
#else
            return "";
#endif
        }

        private void Initialize()
        {
            string advertisingId = GetAdvertiserId();

#if UNITY_IOS
            iOSBridge.RequestAuthorizationTracking();
#endif

            bool isGameAnalyticsValid = true;
#if UNITY_ANDROID
            isGameAnalyticsValid = (Settings.Instance.gameAnalyticsAndroidGameKey != string.Empty) && (Settings.Instance.gameAnalyticsAndroidSecretKey != string.Empty);
#elif UNITY_IOS
            isGameAnalyticsValid = (Settings.Instance.gameAnalyticsIOSGameKey != string.Empty) && (Settings.Instance.gameAnalyticsIOSSecretKey != string.Empty);
#endif

            if(isGameAnalyticsValid)
            {
                //GameAnalytics
                var gaGo = new GameObject("GameAnalytics");
                gaGo.transform.parent = transform;
                gaGo.AddComponent<GameAnalyticsSDK.Events.GA_SpecialEvents>();
                gaGo.AddComponent<GameAnalyticsSDK.GameAnalytics>();
                GameAnalyticsSDK.GameAnalytics.Initialize();
                GameAnalyticsSDK.GameAnalytics.SetCustomDimension01(GetTestGroup(advertisingId).ToString());
            }

            //Appsflyer
            var afGo = new GameObject("AppsFlyer");
            afGo.transform.parent = transform;
            afGo.AddComponent<Internal.AppsFlyerManualInitializer>();

            ActiveTestGroupIndex = GetTestGroup(advertisingId);

            switch (ActiveTestGroupIndex)
            {
                case 1:
                    activeTestGroupSettings = Settings.Instance.testGroup1Settings;
                    break;
                case 2:
                    activeTestGroupSettings = Settings.Instance.testGroup2Settings;
                    break;
                case 3:
                    activeTestGroupSettings = Settings.Instance.testGroup3Settings;
                    break;
                default:
                    activeTestGroupSettings = Settings.Instance.testGroup1Settings;
                    break;
            }

            var settings = Resources.Load<JustTrack.JustTrackSettings>(JustTrack.JustTrackSettings.JustTrackSettingsResource);
#if UNITY_ANDROID
            settings.androidIronSourceSettings.appKey = activeTestGroupSettings.ironsourceShouldInitialize ? Settings.Instance.ironSourceAndroidAppKey : "";
            settings.androidIronSourceSettings.enableBanner = activeTestGroupSettings.isBannerEnabled;
            settings.androidIronSourceSettings.enableInterstitial = activeTestGroupSettings.isInterstitialEnabled;
            settings.androidIronSourceSettings.enableRewardedVideo = activeTestGroupSettings.isRewardedVideoEnabled;
            settings.androidIronSourceSettings.enableOfferwall = activeTestGroupSettings.isOfferwallEnabled;
#elif UNITY_IOS
            settings.iosIronSourceSettings.appKey = activeTestGroupSettings.ironsourceShouldInitialize ? Settings.Instance.ironSourceIOSAppKey : "";
            settings.iosIronSourceSettings.enableBanner = activeTestGroupSettings.isBannerEnabled;
            settings.iosIronSourceSettings.enableInterstitial = activeTestGroupSettings.isInterstitialEnabled;
            settings.iosIronSourceSettings.enableRewardedVideo = activeTestGroupSettings.isRewardedVideoEnabled;
            settings.iosIronSourceSettings.enableOfferwall = activeTestGroupSettings.isOfferwallEnabled;

#endif

            bool isJustTrackValid = true;
#if UNITY_ANDROID
            isJustTrackValid = Settings.Instance.justTrackAndroidToken != string.Empty;
#elif UNITY_IOS
            isJustTrackValid = Settings.Instance.justTrackIOSToken != string.Empty;
#endif

            if (isJustTrackValid)
            {
                var jtGo = new GameObject("JustTrack");
                jtGo.transform.parent = transform;
                JustTrackSDKBehaviourInstance = jtGo.AddComponent<JustTrack.JustTrackSDKBehaviour>();

                if (activeTestGroupSettings.ironsourceShouldInitialize)
                {
                    JustTrack.JustTrackSDKBehaviour.OnIronSourceInitialized(() => {
                        if (activeTestGroupSettings.isInterstitialEnabled)
                        {
                            IronSource.Agent.loadInterstitial();
                            SundaySDK.Tracking.TrackInterstitial(SundaySDK.Tracking.AdEvent.Load, "IronSrc", "IronSrc", interstitialPlacement);
                        }

                        if (activeTestGroupSettings.isBannerEnabled)
                        {
                            IronSource.Agent.loadBanner(Settings.Instance.bannerSize, Settings.Instance.bannerPosition);
                            IronSource.Agent.displayBanner();
                        }
                    });
                }
            }
        }

        private void OnEnable()
        {
            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;

            IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
            IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        }

        //Invoked when the initialization process has failed.
        //@param description - string - contains information about the failure.
        void InterstitialAdLoadFailedEvent(IronSourceError error)
        {
        }
        //Invoked when the ad fails to show.
        //@param description - string - contains information about the failure.
        void InterstitialAdShowFailedEvent(IronSourceError error)
        {
        }
        // Invoked when end user clicked on the interstitial ad
        void InterstitialAdClickedEvent()
        {
            SundaySDK.Tracking.TrackInterstitial(SundaySDK.Tracking.AdEvent.Click, "IronSrc", "IronSrc", interstitialPlacement);
        }
        //Invoked when the interstitial ad closed and the user goes back to the application screen.
        void InterstitialAdClosedEvent()
        {
            OnAdSoundOut?.Invoke();
            lastInterstitialShowTime = Time.unscaledTime;
            IronSource.Agent.loadInterstitial();
            SundaySDK.Tracking.TrackInterstitial(SundaySDK.Tracking.AdEvent.Close, "IronSrc", "IronSrc", interstitialPlacement);
        }
        //Invoked when the Interstitial is Ready to shown after load function is called
        void InterstitialAdReadyEvent()
        {

        }
        //Invoked when the Interstitial Ad Unit has opened
        void InterstitialAdOpenedEvent()
        {
            OnAdSoundIn?.Invoke();
            lastInterstitialShowTime = Time.unscaledTime;
            SundaySDK.Tracking.TrackInterstitial(SundaySDK.Tracking.AdEvent.Open, "IronSrc", "IronSrc", interstitialPlacement);
        }
        //Invoked right before the Interstitial screen is about to open. NOTE - This event is available only for some of the networks. 
        //You should treat this event as an interstitial impression, but rather use InterstitialAdOpenedEvent
        void InterstitialAdShowSucceededEvent()
        {
            SundaySDK.Tracking.TrackInterstitial(SundaySDK.Tracking.AdEvent.Show, "IronSrc", "IronSrc", interstitialPlacement);
        }

        //Invoked when the RewardedVideo ad view has opened.
        //Your Activity will lose focus. Please avoid performing heavy 
        //tasks till the video ad will be closed.
        void RewardedVideoAdOpenedEvent()
        {
            OnAdSoundIn?.Invoke();
            SundaySDK.Tracking.TrackRewarded(SundaySDK.Tracking.AdEvent.Open, "IronSrc", "IronSrc", rewardedPlacement);
        }
        //Invoked when the RewardedVideo ad view is about to be closed.
        //Your activity will now regain its focus.
        void RewardedVideoAdClosedEvent()
        {
            OnAdSoundOut?.Invoke();
            lastInterstitialShowTime = Time.unscaledTime;
            SundaySDK.Tracking.TrackRewarded(SundaySDK.Tracking.AdEvent.Close, "IronSrc", "IronSrc", rewardedPlacement);
        }
        //Invoked when there is a change in the ad availability status.
        //@param - available - value will change to true when rewarded videos are available. 
        //You can then show the video by calling showRewardedVideo().
        //Value will change to false when no videos are available.
        void RewardedVideoAvailabilityChangedEvent(bool available)
        {
            //Change the in-app 'Traffic Driver' state according to availability.
            //bool rewardedVideoAvailability = available;

            OnRewardedVideoAvailabilityChanged?.Invoke(available);
        }

        //Invoked when the user completed the video and should be rewarded. 
        //If using server-to-server callbacks you may ignore this events and wait for 
        // the callback from the  ironSource server.
        //@param - placement - placement object which contains the reward data
        void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            OnRewardedVideoSuccess?.Invoke(lastRewardedType);
        }
        //Invoked when the Rewarded Video failed to show
        //@param description - string - contains information about the failure.
        void RewardedVideoAdShowFailedEvent(IronSourceError error)
        {
        }

        // ----------------------------------------------------------------------------------------
        // Note: the events below are not available for all supported rewarded video ad networks. 
        // Check which events are available per ad network you choose to include in your build. 
        // We recommend only using events which register to ALL ad networks you include in your build. 
        // ----------------------------------------------------------------------------------------

        //Invoked when the video ad starts playing. 
        void RewardedVideoAdStartedEvent()
        {
            SundaySDK.Tracking.TrackRewarded(SundaySDK.Tracking.AdEvent.Open, "IronSrc", "IronSrc", rewardedPlacement);
        }
        //Invoked when the video ad finishes playing. 
        void RewardedVideoAdEndedEvent()
        {
            SundaySDK.Tracking.TrackRewarded(SundaySDK.Tracking.AdEvent.Close, "IronSrc", "IronSrc", rewardedPlacement);
        }
        //Invoked when the video ad is clicked. 
        void RewardedVideoAdClickedEvent(IronSourcePlacement ironSourcePlacement)
        {
            SundaySDK.Tracking.TrackRewarded(SundaySDK.Tracking.AdEvent.Click, "IronSrc", "IronSrc", rewardedPlacement);
        }

        private void BannerAdLoadedEvent()
        {
            SundaySDK.Tracking.TrackBanner(SundaySDK.Tracking.AdEvent.Load, "IronSrc", "IronSrc", Settings.Instance.bannerPosition.ToString());
        }

        private void BannerAdClickedEvent()
        {
            SundaySDK.Tracking.TrackBanner(SundaySDK.Tracking.AdEvent.Click, "IronSrc", "IronSrc", Settings.Instance.bannerPosition.ToString());
        }
    }
}