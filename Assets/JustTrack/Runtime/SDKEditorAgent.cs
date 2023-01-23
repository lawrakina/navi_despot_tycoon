#if UNITY_EDITOR
using System;
using UnityEngine;

namespace JustTrack {
    internal class SDKEditorAgent : SDKBaseAgent {
        private bool EditorInitialized = false;

        internal SDKEditorAgent() {}

        protected override void PerformInit(string pApiKey, string pTrackingId, string pTrackingProvider, Action<AttributionResponse> pOnSuccess, Action<string> pOnFailure) {
            AttributionResponse fakeResponse = AttributionResponse.CreateFakeResponse();

            EditorInitialized = true;

            JustTrackSDKBehaviour.CallOnMainThread(() => {
                pOnSuccess(fakeResponse);
            });
        }

        protected override void PerformGetRetargetingParameters(Action<RetargetingParameters> pOnSuccess, Action<string> pOnFailure) {
            JustTrackSDKBehaviour.CallOnMainThread(() => {
                pOnSuccess(null);
            });
        }

        protected override PreliminaryRetargetingParameters PerformGetPreliminaryRetargetingParameters() {
            return null;
        }

        protected override void PerformRegisterAttributionListener(Action<AttributionResponse> pListener) {
            LogDebug("Registered attribution listener");
        }

        protected override void PerformRegisterRetargetingParameterListener(Action<RetargetingParameters> pListener) {
            LogDebug("Registered retargeting parameter listener");
        }

        protected override void PerformRegisterPreliminaryRetargetingListener(Action<PreliminaryRetargetingParameters> pListener) {
            LogDebug("Registered preliminary retargeting parameter listener");
        }

        protected override void PerformIronSourceIntegration(Action pOnSuccess, Action<string> pOnFailure) {
            JustTrackSDKBehaviour.CallOnMainThread(pOnSuccess);
        }

        protected override void HandleIronSourceRevenueEvent(string pAdUnit, string pAdNetwork, string pPlacement, string pAbTesting, string pSegmentName, string pInstanceName, double pRevenue) {
            LogDebug($"Handled ironSource event for ad unit {pAdUnit}, ad network {pAdNetwork}, placement {pPlacement}, ab testing {pAbTesting}, segment {pSegmentName}, and instance {pInstanceName} with {pRevenue} USD revenue");
        }

        protected override void PerformPublishEvent(UserEventBase pEvent) {
            LogDebug($"Publishing event {pEvent.Name} {pEvent.GetDimension(Dimension.CUSTOM_1)} {pEvent.GetDimension(Dimension.CUSTOM_2)} {pEvent.GetDimension(Dimension.CUSTOM_3)} {pEvent.Value} {pEvent.Unit}");
        }

        protected override void PerformGetAffiliateLink(string pChannel, Action<string> pOnSuccess, Action<string> pOnFailure) {
            JustTrackSDKBehaviour.CallOnMainThread(() => {
                pOnSuccess("https://example.com/affiliate");
            });
        }

        protected override void PerformPublishFirebaseInstanceId(string pFirebaseInstanceId) {
            LogDebug($"Publishing firebase instance id {pFirebaseInstanceId}");
        }

        public override bool IsInitialized() {
            return EditorInitialized;
        }

        public override void LogDebug(string pMessage) {
            Debug.Log(pMessage);
        }

        public override void LogInfo(string pMessage) {
            Debug.Log(pMessage);
        }

        public override void LogWarning(string pMessage) {
            Debug.LogWarning(pMessage);
        }

        public override void LogError(string pMessage) {
            Debug.LogError(pMessage);
        }

    }
}
#endif