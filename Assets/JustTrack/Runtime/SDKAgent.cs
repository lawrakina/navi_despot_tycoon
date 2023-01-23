using System;

namespace JustTrack {
    internal interface SDKAgent {
        void SetTrackingId(string pTrackingId, string pTrackingProvider);
        void Initialize(string pApiKey, bool pNeedTrackingId, Action<AttributionResponse> pOnSuccess, Action<string> pOnFailure);
        void GetRetargetingParameters(Action<RetargetingParameters> pOnSuccess, Action<string> pOnFailure);
        PreliminaryRetargetingParameters GetPreliminaryRetargetingParameters();
        void RegisterAttributionListener(Action<AttributionResponse> pListener);
        void RegisterRetargetingParameterListener(Action<RetargetingParameters> pListener);
        void RegisterPreliminaryRetargetingListener(Action<PreliminaryRetargetingParameters> pListener);
        void IntegrateWithIronSource(Action pOnSuccess, Action<string> pOnFailure);
        void PublishEvent(UserEventBase pEvent);
        void GetAffiliateLink(string pChannel, Action<string> pOnSuccess, Action<string> pOnFailure);
        void PublishFirebaseInstanceId(string pFirebaseInstanceId);
        bool IsInitialized();
        void LogDebug(string pMessage);
        void LogInfo(string pMessage);
        void LogWarning(string pMessage);
        void LogError(string pMessage);
    }
}
