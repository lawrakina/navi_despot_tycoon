using System;

namespace JustTrack {
    internal abstract class SDKBaseAgent : SDKAgent {
        protected string apiKey { get; private set; }
        protected string trackingId { get; private set; }
        protected string trackingProvider { get; private set; }
        protected Action<AttributionResponse> onAttributionSuccess = null;
        protected Action<string> onAttributionFailure = null;
        // Lock guarding the fields above:
        protected readonly object syncLock = new object();

        protected bool needTrackingId { get; private set; }

        protected SDKBaseAgent() {
            apiKey = "";
            trackingId = "";
            needTrackingId = false;
        }

        public void SetTrackingId(string pTrackingId, string pTrackingProvider) {
            lock(syncLock) {
                trackingId = pTrackingId;
                trackingProvider = pTrackingProvider;
                if (apiKey == "") {
                    return;
                }
            }
            TryInitialize();
        }

        public void Initialize(string pApiKey, bool pNeedTrackingId, Action<AttributionResponse> pOnSuccess, Action<string> pOnFailure) {
            lock(syncLock) {
                needTrackingId = pNeedTrackingId;
                apiKey = pApiKey;
                if (onAttributionSuccess == null) {
                    onAttributionSuccess = pOnSuccess;
                } else {
                    onAttributionSuccess += pOnSuccess;
                }
                if (onAttributionFailure == null) {
                    onAttributionFailure = pOnFailure;
                } else {
                    onAttributionFailure += pOnFailure;
                }
            }
            TryInitialize();
        }

        private readonly object initInProgressLock = new object();

        protected void TryInitialize() {
            string localApiKey = "";
            string localTrackingId = "";
            string localTrackingProvider = "";
            bool localNeedTrackingId = false;
            lock(syncLock) {
                localApiKey = apiKey;
                localTrackingId = trackingId;
                localTrackingProvider = trackingProvider;
                localNeedTrackingId = needTrackingId;
            }
            if (localApiKey == "") {
                return;
            }
            if (localNeedTrackingId && localTrackingId == "") {
                LogDebug("Still waiting for parameters to initialize");
                return;
            }
            lock(initInProgressLock) {
                if (IsInitialized()) {
                    return;
                }
                PerformInit(localApiKey, localTrackingId, trackingProvider, (attribution) => {
                    Action<AttributionResponse> toCall = null;
                    lock(syncLock) {
                        toCall = onAttributionSuccess;
                        onAttributionSuccess = null;
                        onAttributionFailure = null;
                    }
                    if (toCall != null) {
                        toCall(attribution);
                    }
                }, (error) => {
                    Action<string> toCall = null;
                    lock(syncLock) {
                        toCall = onAttributionFailure;
                        onAttributionSuccess = null;
                        onAttributionFailure = null;
                    }
                    if (toCall != null) {
                        toCall(error);
                    }
                });
                LogDebug("Initialized with tracking id '" + localTrackingId + "' from '" + localTrackingProvider + "'");
            }
            CheckAfterInit();
        }

        protected abstract void PerformInit(string pApiKey, string pTrackingId, string pTrackingProvider, Action<AttributionResponse> pOnSuccess, Action<string> pOnFailure);

        public void GetRetargetingParameters(Action<RetargetingParameters> pOnSuccess, Action<string> pOnFailure) {
            if (!IsInitialized()) {
                WaitForInititalization(() => {
                    GetRetargetingParameters(pOnSuccess, pOnFailure);
                });
                return;
            }

            PerformGetRetargetingParameters(pOnSuccess, pOnFailure);
        }

        protected abstract void PerformGetRetargetingParameters(Action<RetargetingParameters> pOnSuccess, Action<string> pOnFailure);

        public PreliminaryRetargetingParameters GetPreliminaryRetargetingParameters() {
            if (!IsInitialized()) {
                return null;
            }

            return PerformGetPreliminaryRetargetingParameters();
        }

        protected abstract PreliminaryRetargetingParameters PerformGetPreliminaryRetargetingParameters();

        public void RegisterAttributionListener(Action<AttributionResponse> pListener) {
            if (!IsInitialized()) {
                WaitForInititalization(() => {
                    RegisterAttributionListener(pListener);
                });
                return;
            }

            PerformRegisterAttributionListener(pListener);
        }

        protected abstract void PerformRegisterAttributionListener(Action<AttributionResponse> pListener);

        public void RegisterRetargetingParameterListener(Action<RetargetingParameters> pListener) {
            if (!IsInitialized()) {
                WaitForInititalization(() => {
                    RegisterRetargetingParameterListener(pListener);
                });
                return;
            }

            PerformRegisterRetargetingParameterListener(pListener);
        }

        protected abstract void PerformRegisterRetargetingParameterListener(Action<RetargetingParameters> pListener);

        public void RegisterPreliminaryRetargetingListener(Action<PreliminaryRetargetingParameters> pListener) {
            if (!IsInitialized()) {
                WaitForInititalization(() => {
                    RegisterPreliminaryRetargetingListener(pListener);
                });
                return;
            }

            PerformRegisterPreliminaryRetargetingListener(pListener);
        }

        protected abstract void PerformRegisterPreliminaryRetargetingListener(Action<PreliminaryRetargetingParameters> pListener);

        public void IntegrateWithIronSource(Action pOnSuccess, Action<string> pOnFailure) {
            if (!IsInitialized()) {
                WaitForInititalization(() => {
                    IntegrateWithIronSource(pOnSuccess, pOnFailure);
                });
                return;
            }

            PerformIronSourceIntegration(pOnSuccess, pOnFailure);
        }

        protected virtual void PerformIronSourceIntegration(Action pOnSuccess, Action<string> pOnFailure) {
            #if JUSTTRACK_IRONSOURCE_INTEGRATION
                JustTrackSDKBehaviour.GetAttribution((attribution) => {
                    Reflection.IronSourceSetUserId(attribution.UserId);
                    Reflection.IronSourceAddImpressionListener((impressionData) => {
                        HandleIronSourceRevenueEvent(impressionData.adUnit, impressionData.adNetwork, impressionData.placement, impressionData.abTesting, impressionData.segmentName, impressionData.instanceName, impressionData.revenue);
                    });
                    // already on main thread
                    pOnSuccess();
                }, (error) => {
                    // register the impression listener in any case - if we could not fetch an attribution and later get one (because of bad network)
                    // we still want to trigger those / at least record the events.
                    Reflection.IronSourceAddImpressionListener((impressionData) => {
                        HandleIronSourceRevenueEvent(impressionData.adUnit, impressionData.adNetwork, impressionData.placement, impressionData.abTesting, impressionData.segmentName, impressionData.instanceName, impressionData.revenue);
                    });
                    pOnFailure(error);
                });
            #else
                LogError("No support for IronSource available");
                JustTrackSDKBehaviour.CallOnMainThread(pOnSuccess);
            #endif
        }

        protected abstract void HandleIronSourceRevenueEvent(string pAdUnit, string pAdNetwork, string pPlacement, string pAbTesting, string pSegmentName, string pInstanceName, double pRevenue);

        public void PublishEvent(UserEventBase pEvent) {
            if (!IsInitialized()) {
                WaitForInititalization(() => {
                    PublishEvent(pEvent);
                });
                return;
            }

            PerformPublishEvent(pEvent);
        }

        protected abstract void PerformPublishEvent(UserEventBase pEvent);

        public void GetAffiliateLink(string pChannel, Action<string> pOnSuccess, Action<string> pOnFailure) {
            if (!IsInitialized()) {
                WaitForInititalization(() => {
                    GetAffiliateLink(pChannel, pOnSuccess, pOnFailure);
                });
                return;
            }

            PerformGetAffiliateLink(pChannel, pOnSuccess, pOnFailure);
        }

        protected abstract void PerformGetAffiliateLink(string pChannel, Action<string> pOnSuccess, Action<string> pOnFailure);

        public void PublishFirebaseInstanceId(string pFirebaseInstanceId) {
            if (!IsInitialized()) {
                WaitForInititalization(() => {
                    PublishFirebaseInstanceId(pFirebaseInstanceId);
                });
                return;
            }

            PerformPublishFirebaseInstanceId(pFirebaseInstanceId);
        }

        protected abstract void PerformPublishFirebaseInstanceId(string pFirebaseInstanceId);

        public abstract void LogDebug(string pMessage);
        public abstract void LogInfo(string pMessage);
        public abstract void LogWarning(string pMessage);
        public abstract void LogError(string pMessage);

        private Action onInit = null;
        // Lock guarding the static fields above:
        private readonly object initLock = new object();

        public abstract bool IsInitialized();

        private void WaitForInititalization(Action pOnInit) {
            lock(initLock) {
                if (onInit == null) {
                    onInit = pOnInit;
                } else {
                    onInit += pOnInit;
                }
            }
            CheckAfterInit();
        }

        protected void CheckAfterInit() {
            Action toCall = null;
            lock(initLock) {
                if (!IsInitialized()) {
                    return;
                }
                toCall = onInit;
                onInit = null;
            }
            if (toCall != null) {
                toCall();
            }
        }
    }
}
