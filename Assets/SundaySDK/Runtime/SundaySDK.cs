using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sunday;
using GameAnalyticsSDK;
using System;

public static class SundaySDK
{
    public static int ActiveTestGroupIndex
    {
        get
        {
            if(SundaySDKBehavior.Instance != null)
            {
                return SundaySDKBehavior.Instance.ActiveTestGroupIndex;
            }
            else
            {
                return 0;
            }
        }
    }

    public static SundayJusttrackIronsourceSettings ActiveTestGroupSettings
    {
        get
        {
            if (SundaySDKBehavior.Instance != null)
            {
                return SundaySDKBehavior.Instance.activeTestGroupSettings;
            }
            else
            {
                return null;
            }
        }
    }

    public static class Tracking
    {
        public enum ProgressEvent
        {
            Start,
            Fail,
            Complete
        }

        internal enum AdEvent
        {
            Load,
            Open,
            Close,
            Click,
            Show
        }

        public static void TrackLevelStart(int level, string param1 = "", string param2 = "")
        {
            TrackProgress(ProgressEvent.Start, level, param1, param2);
        }

        public static void TrackLevelComplete(int level, string param1 = "", string param2 = "")
        {
            TrackProgress(ProgressEvent.Complete, level, param1, param2);
        }

        public static void TrackLevelFail(int level, string param1 = "", string param2 = "")
        {
            TrackProgress(ProgressEvent.Fail, level, param1, param2);
        }


        /// <summary>
        /// The most simple event
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        public static void TrackEvent(string eventName)
        {
            GameAnalytics.NewDesignEvent(eventName);
            JustTrack.JustTrackSDK.PublishEvent(eventName);
        }

        /// <summary>
        /// Send event with more details
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="category"></param>
        /// <param name="element"></param>
        /// <param name="action"></param>
        public static void TrackEvent(string eventName, string category, string element, string action)
        {
            string eventString = string.Format("{0}:{1}:{2}:{3}", eventName, category, element, action);
            GameAnalytics.NewDesignEvent(eventString);

            var events = new JustTrack.EventDetails(eventName, category, element, action);
            JustTrack.JustTrackSDK.PublishEvent(events);
        }

        /// <summary>
        /// Send and event with a numeric relevant value
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="category"></param>
        /// <param name="element"></param>
        /// <param name="action"></param>
        /// <param name="value"></param>
        public static void TrackEventCount(string eventName, string category, string element, string action, float value)
        {
            string eventString = string.Format("{0}:{1}:{2}:{3}", eventName, category, element, action);
            GameAnalytics.NewDesignEvent(eventString, value);

            var events = new JustTrack.CustomUserEvent(new JustTrack.EventDetails(eventName, category, element, action)).SetValueAndUnit(value, JustTrack.Unit.Count);
            JustTrack.JustTrackSDK.PublishEvent(events);
        }

        /// <summary>
        /// Send a custom event with relevant duration information
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="category"></param>
        /// <param name="element"></param>
        /// <param name="action"></param>
        /// <param name="value"></param>
        public static void TrackEventDuration(string eventName, string category, string element, string action, double value)
        {
            string eventString = string.Format("{0}:{1}:{2}:{3}", eventName, category, element, action);
            GameAnalytics.NewDesignEvent(eventString, (float)value);

            var events = new JustTrack.CustomUserEvent(new JustTrack.EventDetails(eventName, category, element, action)).SetValueAndUnit(value, JustTrack.Unit.Seconds);
            JustTrack.JustTrackSDK.PublishEvent(events);
        }

        private static GAProgressionStatus GetGameAnalyticsStatus(ProgressEvent progressEvent)
        {
            GAProgressionStatus status;
            switch (progressEvent)
            {
                case ProgressEvent.Start:
                    status = GAProgressionStatus.Start;
                    break;
                case ProgressEvent.Fail:
                    status = GAProgressionStatus.Fail;
                    break;
                case ProgressEvent.Complete:
                    status = GAProgressionStatus.Complete;
                    break;
                default:
                    status = GAProgressionStatus.Undefined;
                    break;
            }
            return status;
        }

        /// <summary>
        /// Track the progress of the game
        /// </summary>
        /// <param name="progressEvent"> Select weather it's a starting failing or completion event.</param>
        /// <param name="level"> The number of the current level.</param>
        /// <param name="eventName"></param>
        /// <param name="details"> Used only by GameAnalytics as a second parameter </param>
        public static void TrackProgress(ProgressEvent progressEvent, int level, string eventName = null, string details = "")
        {
            string levelId = string.Format("Level_{0}", level);
            string progress = "";

            JustTrack.UserEvent justrackEvent = null;
            switch (progressEvent)
            {
                case ProgressEvent.Start:
                    progress = "start";
                    justrackEvent = new JustTrack.ProgressionLevelStartEvent((eventName != null) ? eventName : levelId, levelId);
                    break;
                case ProgressEvent.Fail:
                    progress = "fail";
                    justrackEvent = new JustTrack.ProgressionLevelFailEvent((eventName != null) ? eventName : levelId, levelId);
                    break;
                case ProgressEvent.Complete:
                    progress = "finish";
                    justrackEvent = new JustTrack.ProgressionLevelFinishEvent((eventName != null) ? eventName : levelId, levelId);
                    break;
            }
            Debug.Log("Track Progress:" + progress);

            var status = GetGameAnalyticsStatus(progressEvent);

            if (string.IsNullOrEmpty(eventName))
            {
                GameAnalytics.NewProgressionEvent(status, levelId);
            }
            else
            {
                GameAnalytics.NewProgressionEvent(status, levelId, eventName, details);
            }

            JustTrack.JustTrackSDK.PublishEvent(justrackEvent);
        }

        internal static void TrackInterstitial(AdEvent adEvent, string sdkName, string newtwork, string placement)
        {
            switch (adEvent)
            {
                case AdEvent.Click:
                    GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.Interstitial, newtwork, placement);
                    break;
                case AdEvent.Load:
                    GameAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.Interstitial, newtwork, placement);
                    break;
                case AdEvent.Show:
                    GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial, newtwork, placement);
                    break;
            }

            switch (adEvent)
            {
                case AdEvent.Load:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdInterstitialLoadEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Open:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdInterstitialOpenEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Close:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdInterstitialCloseEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Show:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdInterstitialShowEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Click:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdInterstitialClickEvent(sdkName, newtwork, placement));
                    break;
            }
        }

        internal static void TrackRewarded(AdEvent adEvent, string sdkName, string newtwork, string placement)
        {
            switch (adEvent)
            {
                case AdEvent.Click:
                    GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.RewardedVideo, newtwork, placement);
                    break;
                case AdEvent.Load:
                    GameAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.RewardedVideo, newtwork, placement);
                    break;
                case AdEvent.Show:
                    GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, newtwork, placement);
                    break;
            }

            switch (adEvent)
            {
                case AdEvent.Load:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdRewardedLoadEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Open:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdRewardedOpenEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Close:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdRewardedCloseEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Show:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdRewardedShowEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Click:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdRewardedClickEvent(sdkName, newtwork, placement));
                    break;
            }
        }

        internal static void TrackBanner(AdEvent adEvent, string sdkName, string newtwork, string placement)
        {
            switch (adEvent)
            {
                case AdEvent.Click:
                    GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.Banner, newtwork, placement);
                    break;
                case AdEvent.Load:
                    GameAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.Banner, newtwork, placement);
                    break;
            }

            switch (adEvent)
            {
                case AdEvent.Load:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdBannerLoadEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Open:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdBannerOpenEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Close:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdBannerCloseEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Show:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdBannerShowEvent(sdkName, newtwork, placement));
                    break;
                case AdEvent.Click:
                    JustTrack.JustTrackSDK.PublishEvent(new JustTrack.AdBannerClickEvent(sdkName, newtwork, placement));
                    break;
            }
        }
    }

    public static class Monetization
    {
        public static Action<string> OnRewardedVideoSuccess { get { return SundaySDKBehavior.OnRewardedVideoSuccess; } set { SundaySDKBehavior.OnRewardedVideoSuccess = value; } }
        public static Action<bool> OnRewardedVideoAvailabilityChanged { get { return SundaySDKBehavior.OnRewardedVideoAvailabilityChanged; } set { SundaySDKBehavior.OnRewardedVideoAvailabilityChanged = value; } }
        public static Action OnAdSoundIn { get { return SundaySDKBehavior.OnAdSoundIn; } set { SundaySDKBehavior.OnAdSoundIn = value; } }
        public static Action OnAdSoundOut { get { return SundaySDKBehavior.OnAdSoundOut; } set { SundaySDKBehavior.OnAdSoundOut = value; } }

        public static bool IsRewardedVideoAvailable
        {
            get
            {
                return SundaySDKBehavior.Instance != null && ActiveTestGroupSettings.isRewardedVideoEnabled && IronSource.Agent.isRewardedVideoAvailable();
            }
        }

        public static void ShowInterstitial(string placementName, bool ignoreInterval = false)
        {
            if (SundaySDKBehavior.Instance == null || !ActiveTestGroupSettings.isInterstitialEnabled) return;

            SundaySDKBehavior.interstitialPlacement = placementName;

            if (ignoreInterval || Time.unscaledTime > SundaySDKBehavior.lastInterstitialShowTime + SundaySDKBehavior.Instance.activeTestGroupSettings.interstitialCooldown)
            {
                IronSource.Agent.showInterstitial(SundaySDKBehavior.interstitialPlacement);
            }
        }

        public static void ShowRewardedVideo(string rewardedType, string placementName)
        {
            if (SundaySDKBehavior.Instance == null) return;

            if (!ActiveTestGroupSettings.isRewardedVideoEnabled)
            {
                OnRewardedVideoSuccess?.Invoke(rewardedType);
                return;
            }

            SundaySDKBehavior.rewardedPlacement = placementName;
            SundaySDKBehavior.lastRewardedType = rewardedType;
            IronSource.Agent.showRewardedVideo(placementName);
            SundaySDK.Tracking.TrackRewarded(SundaySDK.Tracking.AdEvent.Show, "IronSrc", "IronSrc", SundaySDKBehavior.rewardedPlacement);
        }
    }
}
