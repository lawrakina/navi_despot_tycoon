#if UNITY_ANDROID
using System;
using UnityEngine;

namespace JustTrack {
    internal class SDKAndroidAgent : SDKBaseAgent {
        internal const string PACKAGE = "io.justtrack";
        internal const string BUILDER_CLASS = "JustTrackSdkBuilder";
        internal const string EVENT_CLASS = "UserEvent";

        private AndroidJavaObject INSTANCE;

        internal SDKAndroidAgent() {}

        protected override void PerformInit(string pApiKey, string pTrackingId, string pTrackingProvider, Action<AttributionResponse> pOnSuccess, Action<string> pOnFailure) {
            var builder = new AndroidJavaObject($"{PACKAGE}.{BUILDER_CLASS}", CurrentActivity(), pApiKey);
            if (pTrackingId != null && pTrackingId != "") {
                builder.Call<AndroidJavaObject>("setTrackingId", pTrackingId, pTrackingProvider);
            }
            INSTANCE = builder.Call<AndroidJavaObject>("build");
            Action<AndroidJavaObject> onResolve = (attribution) => {
                var response = AttributionResponse.FromAndroidObject(attribution);
                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    pOnSuccess(response);
                });
            };
            var responseFuture = INSTANCE.Call<AndroidJavaObject>("attributeUser");
            INSTANCE.Call("toPromise", responseFuture, new Promise(onResolve, pOnFailure));
        }

        protected override void PerformGetRetargetingParameters(Action<RetargetingParameters> pOnSuccess, Action<string> pOnFailure) {
            Action<AndroidJavaObject> onResolve = (parameters) => {
                var response = parameters == null ? null : RetargetingParameters.FromAndroidObject(parameters);
                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    pOnSuccess(response);
                });
            };
            var responseFuture = INSTANCE.Call<AndroidJavaObject>("getRetargetingParameters");
            INSTANCE.Call("toPromise", responseFuture, new Promise(onResolve, pOnFailure));
        }

        protected override PreliminaryRetargetingParameters PerformGetPreliminaryRetargetingParameters() {
            AndroidJavaObject parameters = INSTANCE.Call<AndroidJavaObject>("getPreliminaryRetargetingParameters");

            if (parameters == null) {
                return null;
            }

            return PreliminaryRetargetingParameters.FromAndroidObject(parameters, INSTANCE);
        }

        protected override void PerformRegisterAttributionListener(Action<AttributionResponse> pListener) {
            INSTANCE.Call<AndroidJavaObject>("registerAttributionListener", new AttributionListener((attribution) => {
                var response = AttributionResponse.FromAndroidObject(attribution);
                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    pListener(response);
                });
            }));
        }

        protected override void PerformRegisterRetargetingParameterListener(Action<RetargetingParameters> pListener) {
            INSTANCE.Call<AndroidJavaObject>("registerRetargetingParametersListener", new RetargetingParametersListener((parameters) => {
                var response = RetargetingParameters.FromAndroidObject(parameters);
                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    pListener(response);
                });
            }));
        }

        protected override void PerformRegisterPreliminaryRetargetingListener(Action<PreliminaryRetargetingParameters> pListener) {
            INSTANCE.Call<AndroidJavaObject>("registerPreliminaryRetargetingParametersListener", new PreliminaryRetargetingParametersListener((parameters) => {
                var response = PreliminaryRetargetingParameters.FromAndroidObject(parameters, INSTANCE);
                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    pListener(response);
                });
            }));
        }

        protected override void HandleIronSourceRevenueEvent(string pAdUnit, string pAdNetwork, string pPlacement, string pAbTesting, string pSegmentName, string pInstanceName, double pRevenue) {
            INSTANCE.Call("handleRevenueEvent", pAdUnit, "ironsource", pAdNetwork, pPlacement, pAbTesting, pSegmentName, pInstanceName, pRevenue);
        }

        protected override void PerformPublishEvent(UserEventBase pEvent) {
            AndroidJavaObject eventObject = AndroidEventBuilder.buildEvent(pEvent);
            INSTANCE.Call<AndroidJavaObject>("publishEvent", eventObject);
        }

        protected override void PerformGetAffiliateLink(string pChannel, Action<string> pOnSuccess, Action<string> pOnFailure) {
            var responseFuture = INSTANCE.Call<AndroidJavaObject>("getAffiliateLink", pChannel);
            INSTANCE.Call("toPromise", responseFuture, new StringPromise(pOnSuccess, pOnFailure));
        }

        protected override void PerformPublishFirebaseInstanceId(string pFirebaseInstanceId) {
            INSTANCE.Call<AndroidJavaObject>("publishFirebaseInstanceId", pFirebaseInstanceId);
        }

        private AndroidJavaObject CurrentActivity() {
            return new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                .GetStatic<AndroidJavaObject>("currentActivity");
        }

        public override void LogDebug(string pMessage) {
            if (INSTANCE != null) {
                INSTANCE.Call("logDebug", pMessage);
            } else {
                Debug.Log(pMessage);
            }
        }

        public override void LogInfo(string pMessage) {
            if (INSTANCE != null) {
                INSTANCE.Call("logInfo", pMessage);
            } else {
                Debug.Log(pMessage);
            }
        }

        public override void LogWarning(string pMessage) {
            if (INSTANCE != null) {
                INSTANCE.Call("logWarning", pMessage);
            } else {
                Debug.LogWarning(pMessage);
            }
        }

        public override void LogError(string pMessage) {
            if (INSTANCE != null) {
                INSTANCE.Call("logError", pMessage);
            } else {
                Debug.LogError(pMessage);
            }
        }

        public override bool IsInitialized() {
            return INSTANCE != null;
        }
    }
}
#endif