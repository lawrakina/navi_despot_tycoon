using System;
using UnityEngine;

namespace JustTrack {
    [Serializable]
    public struct IronSourceSettings {
        [SerializeField]
        public string appKey;

        [SerializeField]
        public bool enableBanner;

        [SerializeField]
        public bool enableInterstitial;

        [SerializeField]
        public bool enableRewardedVideo;

        [SerializeField]
        public bool enableOfferwall;
    }

    public class JustTrackSettings : ScriptableObject {
        public const string JustTrackSettingsDirectory = "Assets/JustTrack/Resources";
        public const string JustTrackSettingsResource = "JustTrackSettings";
        public const string JustTrackSettingsPath = JustTrackSettingsDirectory + "/" + JustTrackSettingsResource + ".asset";

        [SerializeField]
        public string androidApiToken;

        [SerializeField]
        public string iosApiToken;

        [SerializeField]
        public AndroidTrackingProvider androidTrackingProvider;

        [SerializeField]
        public iOSTrackingProvider iosTrackingProvider;

        [SerializeField]
        public IronSourceSettings androidIronSourceSettings;

        [SerializeField]
        public IronSourceSettings iosIronSourceSettings;

        internal static JustTrackSettings loadFromResources() {
            return Resources.Load<JustTrackSettings>(JustTrackSettings.JustTrackSettingsResource);
        }
    }
}
