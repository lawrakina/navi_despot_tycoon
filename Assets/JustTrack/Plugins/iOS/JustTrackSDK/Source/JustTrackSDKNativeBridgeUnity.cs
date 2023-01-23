#if UNITY_IOS
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using JustTrack;

internal class JustTrackSDKNativeBridgeUnity : MonoBehaviour {
    #region Declare external C interface

    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_init(string apiToken, string trackingId, string trackingProvider, int inactivityTimeFrameHours, int reAttributionTimeFrameDays, int reFetchAttributionDelaySeconds);
    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_get_retargeting_parameters();
    [DllImport("__Internal")]
    private static extern string _justtrack_sdk_rp_get_preliminary_retargeting_parameters();
    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_publish_event(string name, string category, string element, string action, string dimensions);
    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_publish_unit_event(string name, string category, string element, string action, string dimensions, double value, string unit);
    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_get_affiliate_link(string channel);
    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_publish_firebase_instance_id(string firebaseInstanceId);
    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_handle_ironsource_revenue_event(string adUnit, string adNetwork, string placement, string abTesting, string segmentName, string instanceName, double revenue);
    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_log_debug(string message);
    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_log_info(string message);
    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_log_warning(string message);
    [DllImport("__Internal")]
    private static extern void _justtrack_sdk_rp_log_error(string message);

    #endregion

    #region Wrapped methods and properties
    private bool initialized = false;

    internal void Init(string apiToken, string trackingId, string trackingProvider, Action<string> onSuccess, Action<string> onError) {
        onAttributionDone = onSuccess;
        onAttributionError = onError;
        _justtrack_sdk_rp_init(apiToken, trackingId, trackingProvider, 48, 14, 5);
        initialized = true;
    }

    internal void GetRetargetingParameters(Action<string> onSuccess, Action<string> onError) {
        onGetRetargetingParametersDone = onSuccess;
        onGetRetargetingParametersError = onError;
        _justtrack_sdk_rp_get_retargeting_parameters();
    }

    internal string GetPreliminaryRetargetingParameters() {
        return _justtrack_sdk_rp_get_preliminary_retargeting_parameters();
    }

    internal void HandleIronSourceRevenueEvent(string adUnit, string adNetwork, string placement, string abTesting, string segmentName, string instanceName, double revenue) {
        _justtrack_sdk_rp_handle_ironsource_revenue_event(adUnit, adNetwork, placement, abTesting, segmentName, instanceName, revenue);
    }

    internal void PublishEvent(EventDetails name, UserEventDimensions dimensions) {
        _justtrack_sdk_rp_publish_event(
            name.Name,
            name.Category != null ? name.Category : "",
            name.Element != null ? name.Element : "",
            name.Action != null ? name.Action : "",
            dimensions.Encode()
        );
    }

    internal void PublishUnitEvent(EventDetails name, UserEventDimensions dimensions, double value, string unit) {
        _justtrack_sdk_rp_publish_unit_event(
            name.Name,
            name.Category != null ? name.Category : "",
            name.Element != null ? name.Element : "",
            name.Action != null ? name.Action : "",
            dimensions.Encode(),
            value,
            unit
        );
    }

    internal void GetAffiliateLink(string channel, Action<string> onSuccess, Action<string> onError) {
        onGetAffiliateLinkDone = onSuccess;
        onGetAffiliateLinkError = onError;
        _justtrack_sdk_rp_get_affiliate_link(channel);
    }

    internal void PublishFirebaseInstanceId(string firebaseInstanceId) {
        _justtrack_sdk_rp_publish_firebase_instance_id(firebaseInstanceId);
    }

    internal void LogDebug(string message) {
        _justtrack_sdk_rp_log_debug(message);
    }

    internal void LogInfo(string message) {
        _justtrack_sdk_rp_log_info(message);
    }

    internal void LogWarning(string message) {
        _justtrack_sdk_rp_log_warning(message);
    }

    internal void LogError(string message) {
        _justtrack_sdk_rp_log_error(message);
    }

    internal bool IsInitialized() {
        return initialized;
    }

    #endregion

    #region Singleton implementation

    private static JustTrackSDKNativeBridgeUnity _instance;

    internal static JustTrackSDKNativeBridgeUnity Instance {
        get {
            if (_instance == null) {
                var obj = new GameObject("JustTrackSDKNativeBridgeUnity");
                _instance = obj.AddComponent<JustTrackSDKNativeBridgeUnity>();
            }
            return _instance;
        }
    }

    void Awake() {
        if (_instance != null) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Delegates

    private Action<string> onAttributionDone;
    private Action<string> onAttributionError;
    private Action<string> onGetRetargetingParametersDone;
    private Action<string> onGetRetargetingParametersError;
    private Action<string> onGetAffiliateLinkDone;
    private Action<string> onGetAffiliateLinkError;

    internal void OnAttributionDone(string response) {
        if (onAttributionDone != null) {
            onAttributionDone.Invoke(response);
        }
        onAttributionDone = null;
        onAttributionError = null;
    }

    internal void OnAttributionError(string error) {
        if (onAttributionError != null) {
            onAttributionError.Invoke(error);
        }
        onAttributionDone = null;
        onAttributionError = null;
    }

    internal void OnGetRetargetingParametersDone(string response) {
        if (onGetRetargetingParametersDone != null) {
            onGetRetargetingParametersDone.Invoke(response);
        }
        onGetRetargetingParametersDone = null;
        onGetRetargetingParametersError = null;
    }

    internal void OnGetRetargetingParametersError(string error) {
        if (onGetRetargetingParametersError != null) {
            onGetRetargetingParametersError.Invoke(error);
        }
        onGetRetargetingParametersDone = null;
        onGetRetargetingParametersError = null;
    }

    internal void OnAttributionListenerReceived(string response) {
        SDKiOSAgent.INSTANCE.OnAttributionListenerReceived(response);
    }

    internal void OnRetargetingParametersListenerReceived(string response) {
        SDKiOSAgent.INSTANCE.OnRetargetingParametersListenerReceived(response);
    }

    internal void OnPreliminaryRetargetingParametersListenerReceived(string response) {
        SDKiOSAgent.INSTANCE.OnPreliminaryRetargetingParametersListenerReceived(response);
    }

    internal void OnValidatePreliminaryRetargetingParametersDone(string response) {
        SDKiOSAgent.INSTANCE.OnValidatePreliminaryRetargetingParametersDone(response);
    }

    internal void OnValidatePreliminaryRetargetingParametersError(string response) {
        SDKiOSAgent.INSTANCE.OnValidatePreliminaryRetargetingParametersError(response);
    }

    internal void OnGetAffiliateLinkDone(string link) {
        if (onGetAffiliateLinkDone != null) {
            onGetAffiliateLinkDone.Invoke(link);
        }
        onGetAffiliateLinkDone = null;
        onGetAffiliateLinkError = null;
    }

    internal void OnGetAffiliateLinkError(string error) {
        if (onGetAffiliateLinkError != null) {
            onGetAffiliateLinkError.Invoke(error);
        }
        onGetAffiliateLinkDone = null;
        onGetAffiliateLinkError = null;
    }

    internal void OnHandleError(string error) {
        JustTrackSDK.AGENT.LogError("JustTrackSDK caught error: " + error);
    }

    #endregion
}
#endif
