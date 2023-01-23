using System.Runtime.InteropServices;
using UnityEngine;

namespace Sunday
{
    public class iOSBridge 
    {
#if UNITY_IOS
        [DllImport("__Internal")] private static extern void TrackingAuthorizationRequest();
        [DllImport("__Internal")] private static extern int GetTrackingStatus();
        [DllImport("__Internal")] private static extern string GetIDFA();
#endif

        /// <summary>
        /// The enumerated states of an authorization tracking request.
        /// </summary>
        public enum AuthorizationTrackingStatus
        {
            NOT_DETERMINED = 0,
            RESTRICTED,
            DENIED,
            AUTHORIZED
        }

        /// <summary>
        /// This method allows you to <a href="https://developer.apple.com/documentation/apptrackingtransparency/attrackingmanager/3547037-requesttrackingauthorization">request the user permission dialogue</a>.
        /// </summary>
        public static void RequestAuthorizationTracking()
        {
#if UNITY_IOS
            Debug.Log("Requesting Authorization");
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                TrackingAuthorizationRequest();
            }
#endif
        }

        /// <summary>
        /// This method allows you to check the app tracking transparency (ATT) <a href="https://developer.apple.com/documentation/apptrackingtransparency/attrackingmanager/3547038-trackingauthorizationstatus">authorization status</a>.
        /// </summary>
        /// <returns>An <c>AuthorizationTrackingStatus</c> enum value.</returns>
        public static AuthorizationTrackingStatus GetAuthorizationTrackingStatus()
        {
#if UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return (AuthorizationTrackingStatus)GetTrackingStatus();
            }
#endif
            return AuthorizationTrackingStatus.NOT_DETERMINED;
        }

        /// <summary>
        /// This method allows you to check the app tracking transparency (ATT) <a href="https://developer.apple.com/documentation/apptrackingtransparency/attrackingmanager/3547038-trackingauthorizationstatus">authorization status</a>.
        /// </summary>
        /// <returns>An <c>AuthorizationTrackingStatus</c> enum value.</returns>
        public static string IDFA
        {
            get
            {
#if UNITY_IOS
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return GetIDFA();
                }
#endif
                return "";
            }
        }
    }
}
