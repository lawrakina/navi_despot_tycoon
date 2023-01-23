#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace JustTrack {
    internal class SDKiOSAgent : SDKBaseAgent {
        internal static SDKiOSAgent INSTANCE = new SDKiOSAgent();

        private SDKiOSAgent() {}

        [Serializable]
        public class iOSAttributionResponse {
            public string userId;
            public string installId;
            public string userType;
            public string type;
            public string campaignId;
            public string campaignName;
            public string campaignType;
            public string channelId;
            public string channelName;
            public string channelIncent;
            public string networkId;
            public string networkName;
            public string sourceId;
            public string sourceBundleId;
            public string sourcePlacement;
            public string adsetId;
            public string recruiterAdvertiserId;
            public string recruiterUserId;
            public string recruiterPackageId;
            public string recruiterPlatform;
            public string createdAt;
        }

        [Serializable]
        public class iOSRetargetingParameters {
            public string wasAlreadyInstalled;
            public string url;
            public string parameters;
            public string promoCode;
        }

        [Serializable]
        public class iOSRetargetingParameter {
            public string parameter;
            public string value;
        }

        [Serializable]
        public class iOSRetargetingParametersList {
            public iOSRetargetingParameter[] parameters;
        }

        [Serializable]
        public class iOSPreliminaryRetargetingParameters {
            public string preliminaryId;
            public string wasAlreadyInstalled;
            public string url;
            public string parameters;
            public string promoCode;
        }

        [Serializable]
        public class iOSPreliminaryRetargetingParametersValidateError {
            public string preliminaryId;
            public string error;
        }

        [Serializable]
        public class iOSPreliminaryRetargetingParametersValidateResult {
            public string preliminaryId;
            public string response;
            public string parameters;
        }

        protected override void PerformInit(string pApiKey, string pTrackingId, string pTrackingProvider, Action<AttributionResponse> pOnSuccess, Action<string> pOnFailure) {
            Action<string> onSuccess = (attribution) => {
                var response = parseAttributionResponse(attribution);

                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    pOnSuccess(response);
                });
            };
            JustTrackSDKNativeBridgeUnity.Instance.Init(pApiKey, pTrackingId, pTrackingProvider, onSuccess, pOnFailure);
        }

        private AttributionResponse parseAttributionResponse(string attribution) {
                iOSAttributionResponse parsed = JsonUtility.FromJson<iOSAttributionResponse>(attribution);

                string userId = parsed.userId;
                string installId = parsed.installId;
                string userType = parsed.userType;
                string type = parsed.type;
                string campaignIdString = parsed.campaignId;
                string campaignName = parsed.campaignName;
                string campaignType = parsed.campaignType;
                string channelIdString = parsed.channelId;
                string channelName = parsed.channelName;
                string channelIncentString = parsed.channelIncent;
                string networkIdString = parsed.networkId;
                string networkName = parsed.networkName;
                string sourceId = parsed.sourceId;
                string sourceBundleId = parsed.sourceBundleId;
                string sourcePlacement = parsed.sourcePlacement;
                string adsetId = parsed.adsetId;
                string recruiterAdvertiserId = parsed.recruiterAdvertiserId;
                string recruiterUserId = parsed.recruiterUserId;
                string recruiterPackageId = parsed.recruiterPackageId;
                string recruiterPlatform = parsed.recruiterPlatform;
                string createdAtString = parsed.createdAt;

                int campaignId = int.Parse(campaignIdString);
                int channelId = int.Parse(channelIdString);
                bool channelIncent = channelIncentString == "true";
                int networkId = int.Parse(networkIdString);
                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime createdAt = DateTime.ParseExact(createdAtString, "yyyy-MM-dd'T'HH:mm:ssK", provider);
                return AttributionResponse.CreateResponse(userId, installId, userType, type, campaignId, campaignName, campaignType, channelId, channelName, channelIncent, networkId, networkName, sourceId, sourceBundleId, sourcePlacement, adsetId, recruiterAdvertiserId, recruiterUserId, recruiterPackageId, recruiterPlatform, createdAt);
        }

        protected override void PerformGetRetargetingParameters(Action<RetargetingParameters> pOnSuccess, Action<string> pOnFailure) {
            Action<string> onSuccess = (parameters) => {
                if (parameters == "") {
                    JustTrackSDKBehaviour.CallOnMainThread(() => {
                        pOnSuccess(null);
                    });
                    return;
                }

                var response = parseRetargetingParameters(parameters);
                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    pOnSuccess(response);
                });
            };
            JustTrackSDKNativeBridgeUnity.Instance.GetRetargetingParameters(onSuccess, pOnFailure);
        }

        private RetargetingParameters parseRetargetingParameters(string parameters) {
            iOSRetargetingParameters parsed = JsonUtility.FromJson<iOSRetargetingParameters>(parameters);

            string wasAlreadyInstalled = parsed.wasAlreadyInstalled;
            string url = parsed.url;
            string promoCode = parsed.promoCode;

            bool wasAlreadyInstalledBool = wasAlreadyInstalled == "true";
            iOSRetargetingParametersList parsedParameters = JsonUtility.FromJson<iOSRetargetingParametersList>(parsed.parameters);
            var parametersDict = new Dictionary<string, string>();
            foreach (var item in parsedParameters.parameters) {
                parametersDict.Add(item.parameter, item.value);
            }

            return RetargetingParameters.CreateRetargetingParameters(wasAlreadyInstalledBool, url, parametersDict, promoCode);
        }

        private Dictionary<String, PreliminaryRetargetingParameters> pendingPreliminaryParameters = new Dictionary<String, PreliminaryRetargetingParameters>();

        protected override PreliminaryRetargetingParameters PerformGetPreliminaryRetargetingParameters() {
            string json = JustTrackSDKNativeBridgeUnity.Instance.GetPreliminaryRetargetingParameters();
            if (json == "") {
                return null;
            }

            string preliminaryId;
            PreliminaryRetargetingParameters parameters = parsePreliminaryRetargetingParameters(json, out preliminaryId);

            lock(this) {
                pendingPreliminaryParameters.Add(preliminaryId, parameters);
            }

            return parameters;
        }

        private PreliminaryRetargetingParameters parsePreliminaryRetargetingParameters(string parameters, out string preliminaryId) {
            iOSPreliminaryRetargetingParameters parsed = JsonUtility.FromJson<iOSPreliminaryRetargetingParameters>(parameters);

            preliminaryId = parsed.preliminaryId;
            string wasAlreadyInstalled = parsed.wasAlreadyInstalled;
            string url = parsed.url;
            string promoCode = parsed.promoCode;

            bool wasAlreadyInstalledBool = wasAlreadyInstalled == "true";
            iOSRetargetingParametersList parsedParameters = JsonUtility.FromJson<iOSRetargetingParametersList>(parsed.parameters);
            var parametersDict = new Dictionary<string, string>();
            foreach (var item in parsedParameters.parameters) {
                parametersDict.Add(item.parameter, item.value);
            }

            return PreliminaryRetargetingParameters.CreatePreliminaryRetargetingParameters(wasAlreadyInstalledBool, url, parametersDict, promoCode);
        }

        private Action<AttributionResponse> OnAttributionCallback = null;
        private Action<RetargetingParameters> OnRetargetingCallback = null;
        private Action<PreliminaryRetargetingParameters> OnPreliminaryRetargetingCallback = null;

        protected override void PerformRegisterAttributionListener(Action<AttributionResponse> pListener) {
            lock(this) {
                if (OnAttributionCallback == null) {
                    OnAttributionCallback = pListener;
                } else {
                    OnAttributionCallback += pListener;
                }
            }
        }

        protected override void PerformRegisterRetargetingParameterListener(Action<RetargetingParameters> pListener) {
            lock(this) {
                if (OnRetargetingCallback == null) {
                    OnRetargetingCallback = pListener;
                } else {
                    OnRetargetingCallback += pListener;
                }
            }
        }

        protected override void PerformRegisterPreliminaryRetargetingListener(Action<PreliminaryRetargetingParameters> pListener) {
            lock(this) {
                if (OnPreliminaryRetargetingCallback == null) {
                    OnPreliminaryRetargetingCallback = pListener;
                } else {
                    OnPreliminaryRetargetingCallback += pListener;
                }
            }
        }

        internal void OnAttributionListenerReceived(string response) {
            var attribution = parseAttributionResponse(response);

            Action<AttributionResponse> localCallback = null;
            lock(this) {
                localCallback = OnAttributionCallback;
            }

            if (localCallback != null) {
                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    localCallback(attribution);
                });
            }
        }

        internal void OnRetargetingParametersListenerReceived(string response) {
            var parameters = parseRetargetingParameters(response);

            Action<RetargetingParameters> localCallback = null;
            lock(this) {
                localCallback = OnRetargetingCallback;
            }

            if (localCallback != null) {
                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    localCallback(parameters);
                });
            }
        }

        internal void OnPreliminaryRetargetingParametersListenerReceived(string response) {
            string preliminaryId;
            PreliminaryRetargetingParameters parameters = parsePreliminaryRetargetingParameters(response, out preliminaryId);

            Action<PreliminaryRetargetingParameters> localCallback = null;
            lock(this) {
                pendingPreliminaryParameters.Add(preliminaryId, parameters);
                localCallback = OnPreliminaryRetargetingCallback;
            }

            if (localCallback != null) {
                JustTrackSDKBehaviour.CallOnMainThread(() => {
                    localCallback(parameters);
                });
            }
        }

        internal void OnValidatePreliminaryRetargetingParametersDone(string response) {
            iOSPreliminaryRetargetingParametersValidateResult parsed = JsonUtility.FromJson<iOSPreliminaryRetargetingParametersValidateResult>(response);

            var preliminaryId = parsed.preliminaryId;
            var attribution = parseAttributionResponse(parsed.response);
            var parameters = parsed.parameters == null ? null : parseRetargetingParameters(parsed.parameters);
            var validateResult = ValidateResult.CreateValidateResult(parameters, attribution);

            lock(this) {
                if (pendingPreliminaryParameters.ContainsKey(preliminaryId)) {
                    pendingPreliminaryParameters[preliminaryId].Resolve(validateResult);
                    pendingPreliminaryParameters.Remove(preliminaryId);
                }
            }
        }

        internal void OnValidatePreliminaryRetargetingParametersError(string response) {
            iOSPreliminaryRetargetingParametersValidateError parsed = JsonUtility.FromJson<iOSPreliminaryRetargetingParametersValidateError>(response);

            var preliminaryId = parsed.preliminaryId;
            var error = parsed.error;

            lock(this) {
                if (pendingPreliminaryParameters.ContainsKey(preliminaryId)) {
                    pendingPreliminaryParameters[preliminaryId].Reject(error);
                    pendingPreliminaryParameters.Remove(preliminaryId);
                }
            }
        }

        protected override void HandleIronSourceRevenueEvent(string pAdUnit, string pAdNetwork, string pPlacement, string pAbTesting, string pSegmentName, string pInstanceName, double pRevenue) {
            JustTrackSDKNativeBridgeUnity.Instance.HandleIronSourceRevenueEvent(pAdUnit, pAdNetwork, pPlacement, pAbTesting, pSegmentName, pInstanceName, pRevenue);
        }

        protected override void PerformPublishEvent(UserEventBase pEvent) {
            if (pEvent.Unit != null) {
                var unit = "";
                switch (pEvent.Unit) {
                    case Unit.Count:
                        unit = "count";
                        break;
                    case Unit.Milliseconds:
                        unit = "milliseconds";
                        break;
                    case Unit.Seconds:
                        unit = "seconds";
                        break;
                }
                JustTrackSDKNativeBridgeUnity.Instance.PublishUnitEvent(pEvent.Name, pEvent.Dimensions, pEvent.Value, unit);
            } else {
                JustTrackSDKNativeBridgeUnity.Instance.PublishEvent(pEvent.Name, pEvent.Dimensions);
            }
        }

        protected override void PerformGetAffiliateLink(string pChannel, Action<string> pOnSuccess, Action<string> pOnFailure) {
            JustTrackSDKNativeBridgeUnity.Instance.GetAffiliateLink(pChannel, pOnSuccess, pOnFailure);
        }

        protected override void PerformPublishFirebaseInstanceId(string pFirebaseInstanceId) {
            JustTrackSDKNativeBridgeUnity.Instance.PublishFirebaseInstanceId(pFirebaseInstanceId);
        }

        public override bool IsInitialized() {
            return JustTrackSDKNativeBridgeUnity.Instance.IsInitialized();
        }

        public override void LogDebug(string pMessage) {
            JustTrackSDKNativeBridgeUnity.Instance.LogDebug(pMessage);
        }

        public override void LogInfo(string pMessage) {
            JustTrackSDKNativeBridgeUnity.Instance.LogInfo(pMessage);
        }

        public override void LogWarning(string pMessage) {
            JustTrackSDKNativeBridgeUnity.Instance.LogWarning(pMessage);
        }

        public override void LogError(string pMessage) {
            JustTrackSDKNativeBridgeUnity.Instance.LogError(pMessage);
        }
    }
}
#endif