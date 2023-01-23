using System;
using System.Linq;

namespace JustTrack {
    public class JustTrackSDK {
        #if UNITY_EDITOR
        internal static SDKAgent AGENT = new SDKEditorAgent();
        #elif UNITY_ANDROID
        internal static SDKAgent AGENT = new SDKAndroidAgent();
        #elif UNITY_IOS
        internal static SDKAgent AGENT = SDKiOSAgent.INSTANCE;
        #endif

        private static event Action<AttributionResponse> s_OnAttributionResponse;

        public static event Action<AttributionResponse> OnAttributionResponse {
            add {
                if (s_OnAttributionResponse == null || !s_OnAttributionResponse.GetInvocationList().Contains(value)) {
                    s_OnAttributionResponse += value;
                }
            }

            remove {
                if (s_OnAttributionResponse.GetInvocationList().Contains(value)) {
                    s_OnAttributionResponse -= value;
                }
            }
        }

        private static event Action<RetargetingParameters> s_OnRetargetingParameters;

        public static event Action<RetargetingParameters> OnRetargetingParameters {
            add {
                if (s_OnRetargetingParameters == null || !s_OnRetargetingParameters.GetInvocationList().Contains(value)) {
                    s_OnRetargetingParameters += value;
                }
            }

            remove {
                if (s_OnRetargetingParameters.GetInvocationList().Contains(value)) {
                    s_OnRetargetingParameters -= value;
                }
            }
        }

        private static event Action<PreliminaryRetargetingParameters> s_OnPreliminaryRetargetingParameters;

        public static event Action<PreliminaryRetargetingParameters> OnPreliminaryRetargetingParameters {
            add {
                if (s_OnPreliminaryRetargetingParameters == null || !s_OnPreliminaryRetargetingParameters.GetInvocationList().Contains(value)) {
                    s_OnPreliminaryRetargetingParameters += value;
                }
            }

            remove {
                if (s_OnPreliminaryRetargetingParameters.GetInvocationList().Contains(value)) {
                    s_OnPreliminaryRetargetingParameters -= value;
                }
            }
        }

        // Set the tracking id of the current user. It should be a unique id per user.
        // You only have to call this if you are not using the prefab or if you have the tracking provider set to "Manual Initialization".
        public static void SetTrackingId(string pTrackingId, string pTrackingProvider) {
            AGENT.SetTrackingId(pTrackingId, pTrackingProvider);
        }

        // Initialize the SDK. You don't have to call this yourself if you are using the prefab.
        internal static void Init(string pApiKey, bool pNeedTrackingId, Action<AttributionResponse> pOnSuccess, Action<string> pOnFailure) {
            AGENT.Initialize(pApiKey, pNeedTrackingId, pOnSuccess, pOnFailure);
            AGENT.RegisterAttributionListener((attribution) => {
                // already on the main thread
                if (s_OnAttributionResponse != null) {
                    s_OnAttributionResponse.Invoke(attribution);
                }
            });
            AGENT.RegisterRetargetingParameterListener((parameters) => {
                // already on the main thread
                if (s_OnRetargetingParameters != null) {
                    s_OnRetargetingParameters.Invoke(parameters);
                }
            });
            AGENT.RegisterPreliminaryRetargetingListener((parameters) => {
                // already on the main thread
                if (s_OnPreliminaryRetargetingParameters != null) {
                    s_OnPreliminaryRetargetingParameters.Invoke(parameters);
                }
            });
        }

        public static void GetRetargetingParameters(Action<RetargetingParameters> pOnSuccess, Action<string> pOnFailure) {
            AGENT.GetRetargetingParameters(pOnSuccess, pOnFailure);
        }

        public static PreliminaryRetargetingParameters GetPreliminaryRetargetingParameters() {
            return AGENT.GetPreliminaryRetargetingParameters();
        }

        // Configure the IronSource SDK to use the user id from JustTrack and forward ad impressions as user events.
        // You only have to call this if you are not using the prefab (and IronSource). If you call this yourself, you have
        // to wait for any callback to get called before you initialize IronSource itself.
        public static void IntegrateWithIronSource(Action pOnSuccess, Action<string> pOnFailure) {
            AGENT.IntegrateWithIronSource(pOnSuccess, pOnFailure);
        }

        // Publish a new user event to the server.
        public static void PublishEvent(string pEvent) {
            if (pEvent == null) {
                return;
            }
            AGENT.PublishEvent(new UserEventBase(new EventDetails(pEvent)));
        }

        // Publish a new user event to the server.
        public static void PublishEvent(EventDetails pEvent) {
            if (pEvent == null) {
                return;
            }
            AGENT.PublishEvent(new UserEventBase(pEvent));
        }

        // Publish a new user event to the server.
        public static void PublishEvent(UserEvent pEvent) {
            if (pEvent == null) {
                return;
            }
            AGENT.PublishEvent(pEvent.GetBase());
        }

        // Get a link which can be used to invite another user to your app.
        public static void GetAffiliateLink(string pChannel, Action<string> pOnSuccess, Action<string> pOnFailure) {
            AGENT.GetAffiliateLink(pChannel, pOnSuccess, pOnFailure);
        }

        // Forward the firebase instance id (see https://firebase.google.com/docs/reference/android/com/google/firebase/analytics/FirebaseAnalytics#public-taskstring-getappinstanceid
        // to how to obtain one) to the JustTrack backend.
        public static void PublishFirebaseInstanceId(string pFirebaseInstanceId) {
            AGENT.PublishFirebaseInstanceId(pFirebaseInstanceId);
        }

        private JustTrackSDK() {}
    }
}