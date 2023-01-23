using System;
using UnityEngine;

namespace JustTrack {
    public enum AndroidTrackingProvider {
        Appsflyer            = 0x0000,
        ManualInitialization = 0xFFFE,
        GoogleAdvertiserId   = 0xFFFF,
    }

    public enum iOSTrackingProvider {
        Appsflyer            = 0x0000,
        ManualInitialization = 0xFFFF,
    }

    public class JustTrackSDKBehaviour : MonoBehaviour {
        // Invoked once the attribution has been retrieved from the backend if not null.
        private static Action<AttributionResponse> onInitialized = null;

        // Invoked if an error occurs during attribution if not null.
        private static Action<string> onError = null;

        // Stores the attribution response after the SDK has been initialized.
        private static AttributionResponse attributionResponse = null;

        // Stores the error if getting the attribution fails.
        private static string initError = null;

        // The following settings are deprecated and will not be used anymore.
        // We will remove them in the next version, we are only keeping them to
        // make it easier to upgrade (because you can carry over your settings
        // like that) to the next version with the SDK doing all the work.

        public string androidReleaseApiToken;
        public string androidDevelopmentApiToken;
        public string iosReleaseApiToken;
        public string iosDevelopmentApiToken;

        public AndroidTrackingProvider androidTrackingProvider;
        public iOSTrackingProvider iosTrackingProvider;

        public string androidIronsourceAppKey;
        public string iosIronsourceAppKey;
        public bool androidIronsourceEnableBanner;
        public bool androidIronsourceEnableInterstitial;
        public bool androidIronsourceEnableRewardedVideo;
        public bool androidIronsourceEnableOfferwall;
        public bool iosIronsourceEnableBanner;
        public bool iosIronsourceEnableInterstitial;
        public bool iosIronsourceEnableRewardedVideo;
        public bool iosIronsourceEnableOfferwall;

        // Lock guarding the attribution results and callbacks.
        private static readonly object syncLock = new object();

        void Awake() {
            var settings = JustTrackSettings.loadFromResources();

            if (settings == null) {
                Debug.LogError("There are no settings defined for the JustTrack SDK, can not initialize");
                return;
            }

            gameObject.name = "JustTrackSDKBehaviour";
            DontDestroyOnLoad(gameObject);

            #if UNITY_IOS
                var apiToken = settings.iosApiToken;
                var trackingProvider = JustTrackSDKBehaviour.ToTrackingProvider(settings.iosTrackingProvider);
            #else
                var apiToken = settings.androidApiToken;
                var trackingProvider = JustTrackSDKBehaviour.ToTrackingProvider(settings.androidTrackingProvider);
            #endif

            bool needTrackingId = false;
            switch (trackingProvider) {
                case TrackingProvider.Appsflyer:
                    #if JUSTTRACK_APPSFLYER_INTEGRATION
                        needTrackingId = true;
                    #else
                        // if there is no support compiled into the game, we can't use it
                        needTrackingId = false;
                    #endif
                    break;
                case TrackingProvider.ManualInitialization:
                    needTrackingId = true;
                    break;
                case TrackingProvider.AutomaticInitialization:
                    needTrackingId = false;
                    break;
                default:
                    break;
            }
            #if UNITY_IOS
            // iOS always needs a tracking id right now
            needTrackingId = true;
            #endif

            if (apiToken == null) {
                Debug.LogError("There is no API token defined for the JustTrack SDK, can not initialize");
                return;
            }

            JustTrackSDK.Init(apiToken, needTrackingId, (response) => {
                lock(syncLock) {
                    attributionResponse = response;
                    if (onInitialized != null) {
                        var toCall = onInitialized;
                        CallOnMainThread(() => {
                            toCall(response);
                        });
                    } else {
                        JustTrackSDK.AGENT.LogDebug("Got JustTrack user id " + response.UserId);
                    }
                    onInitialized = null;
                    onError = null;
                }
            }, (error) => {
                lock(syncLock) {
                    initError = error;
                    if (onError != null) {
                        var toCall = onError;
                        CallOnMainThread(() => {
                            toCall(error);
                        });
                    } else {
                        JustTrackSDK.AGENT.LogError("Failed to initialize JustTrack SDK: " + error);
                    }
                    onInitialized = null;
                    onError = null;
                }
            });
            InitTrackingProvider(settings);
            InitIronsource(settings);
        }

        // Retrieve the attribution produced by the SDK. If the SDK already can provide an attribution, your
        // callbacks are immediately invoked with the attribution result. Otherwise they are stored and called
        // as soon as a result is available.
        // You can call this method as many times as you want - each invocation will add your delegates to
        // the list of delegates to call as soon as the attribution is available.
        public static void GetAttribution(Action<AttributionResponse> pOnInitialized, Action<string> pOnError) {
            lock(syncLock) {
                if (attributionResponse != null) {
                    CallOnMainThread(() => {
                        pOnInitialized(attributionResponse);
                    });
                } else if (initError != null) {
                    CallOnMainThread(() => {
                        pOnError(initError);
                    });
                } else {
                    if (onInitialized == null) {
                        onInitialized = pOnInitialized;
                    } else {
                        onInitialized += pOnInitialized;
                    }
                    if (onError == null) {
                        onError = pOnError;
                    } else {
                        onError += pOnError;
                    }
                }
            }
        }

        private static Action actionsOnMainThead = null;
        private static readonly object onMainThreadLock = new object();

        public static void CallOnMainThread(Action pAction) {
            lock(onMainThreadLock) {
                if (actionsOnMainThead == null) {
                    actionsOnMainThead = pAction;
                } else {
                    actionsOnMainThead += pAction;
                }
            }
        }

        void Update() {
            Action action = null;
            lock(onMainThreadLock) {
                action = actionsOnMainThead;
                actionsOnMainThead = null;
            }
            if (action != null) {
                action();
            }
        }

        private void InitTrackingProvider(JustTrackSettings settings) {
            #if UNITY_EDITOR
                JustTrackSDK.AGENT.LogDebug("Setting up fake tracking provider for editor");
                JustTrackSDK.SetTrackingId("editor-tracking-id", "editor-tracking-provider");
            #else
            #if UNITY_IOS
            var trackingProvider = JustTrackSDKBehaviour.ToTrackingProvider(settings.iosTrackingProvider);
            #else
            var trackingProvider = JustTrackSDKBehaviour.ToTrackingProvider(settings.androidTrackingProvider);
            #endif

            switch (trackingProvider) {
                case TrackingProvider.Appsflyer:
                    #if JUSTTRACK_APPSFLYER_INTEGRATION
                        string appsflyerId = Reflection.GetAppsflyerId();
                        JustTrackSDK.AGENT.LogDebug("Got tracking id '" + appsflyerId + "'");
                        JustTrackSDK.SetTrackingId(appsflyerId, "appsflyer");
                    #else
                        JustTrackSDK.AGENT.LogError("No support for appsflyer compiled in, but integration via appsflyer was requested");
                    #endif
                    break;
                case TrackingProvider.ManualInitialization:
                    JustTrackSDK.AGENT.LogDebug("Manual tracking id initialization was requested");
                    // do nothing, we were promised that this is taken care of
                    break;
                case TrackingProvider.AutomaticInitialization:
                    // do nothing, we automatic take care of that on the native code side
                    break;
            }
            #endif
        }

        // Set to true once IronSource has been initialized by the SDK.
        public static bool IronSourceInitialized {
            get {
                bool result;
                lock(ironSourceLock) {
                    result = isIronSourceInitialized;
                }
                return result;
            }

            private set {
                Action toCall = null;
                lock(ironSourceLock) {
                    isIronSourceInitialized = value;
                    if (isIronSourceInitialized) {
                        toCall = onIronSourceInitialized;
                        onIronSourceInitialized = null;
                    }
                }
                if (toCall != null) {
                    CallOnMainThread(toCall);
                }
            }
        }

        // Call the action once IronSource has been successfully initialized.
        // If IronSource has already been initialized, the action is called in
        // the near future.
        public static void OnIronSourceInitialized(Action pAction) {
            lock(ironSourceLock) {
                if (isIronSourceInitialized) {
                    CallOnMainThread(pAction);
                    return;
                }
                if (onIronSourceInitialized == null) {
                    onIronSourceInitialized = pAction;
                } else {
                    onIronSourceInitialized += pAction;
                }
            }
        }

        private static bool isIronSourceInitialized = false;
        private static Action onIronSourceInitialized = null;

        // Lock guarding the ironsource initialized flag and callbacks
        private static readonly object ironSourceLock = new object();

        private void InitIronsource(JustTrackSettings settings) {
            #if JUSTTRACK_IRONSOURCE_INTEGRATION
            #if UNITY_IOS
            var ironSourceSettings = settings.iosIronSourceSettings;
            #else
            var ironSourceSettings = settings.androidIronSourceSettings;
            #endif

            if (ironSourceSettings.appKey == "") {
                JustTrackSDK.AGENT.LogWarning("No IronSource app key set, not initializing");
                return;
            }

            JustTrackSDK.IntegrateWithIronSource(() => {
                JustTrackSDK.AGENT.LogDebug("Successfully integrated with IronSource");
                Reflection.IronSourceInit(ironSourceSettings);
                IronSourceInitialized = true;
            }, (error) => {
                JustTrackSDK.AGENT.LogError("Failed to integrate IronSource: " + error);
                Reflection.IronSourceInit(ironSourceSettings);
                IronSourceInitialized = true;
            });
            #endif
        }

        internal static TrackingProvider ToTrackingProvider(AndroidTrackingProvider provider) {
            switch (provider) {
                case AndroidTrackingProvider.Appsflyer:
                    return TrackingProvider.Appsflyer;
                case AndroidTrackingProvider.GoogleAdvertiserId:
                    return TrackingProvider.AutomaticInitialization;
                case AndroidTrackingProvider.ManualInitialization:
                    return TrackingProvider.ManualInitialization;
            }
            return TrackingProvider.ManualInitialization;
        }

        internal static TrackingProvider ToTrackingProvider(iOSTrackingProvider provider) {
            switch (provider) {
                case iOSTrackingProvider.Appsflyer:
                    return TrackingProvider.Appsflyer;
                case iOSTrackingProvider.ManualInitialization:
                    return TrackingProvider.ManualInitialization;
            }
            return TrackingProvider.ManualInitialization;
        }
    }

    internal enum TrackingProvider {
        Appsflyer,
        ManualInitialization,
        AutomaticInitialization,
    }
}